using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradeManager : MonoBehaviour {
    public UpgradeCategory category;
    public GameObject upgradePrefab;  // Reference to the prefab
    public Transform upgradeParent;   // Parent object to hold the upgrades
    public Upgrade[] upgrades;       // List of all available upgrades
    public PlayerStats playerStats;

    public List<UpgradeState> runtimeUpgrades;

    private void Start() {
        Initialize();
        PopulateUpgrades();
    }

    private void Initialize() {
        runtimeUpgrades = upgrades.Select(upgradeSO => new UpgradeState { data = upgradeSO }).ToList();
    }

    public void PopulateUpgrades() {
        DestroyAllChildren();
        foreach (UpgradeState upgradeState in runtimeUpgrades) {
            GameObject upgradeObject = Instantiate(upgradePrefab, upgradeParent);
            UpgradeUI ui = upgradeObject.GetComponent<UpgradeUI>();
            // Set the Text fields
            ui.nameText.text = upgradeState.data.upgradeName;
            ui.descriptionText.text = upgradeState.data.description;
            ui.costText.text = "Cost: " + upgradeState.CurrentCost;
            ui.levelText.text = "lv. " + upgradeState.level;

            // Set the button click event (you can link a method here to handle the upgrade)
            ui.purchaseButton.onClick.AddListener(() => HandleUpgradePurchase(upgradeState));
        }
    }

    public void DestroyAllChildren() {
        foreach (Transform child in upgradeParent) {
            Destroy(child.gameObject);
        }
    }

    private void HandleUpgradePurchase(UpgradeState upgradeState) {
        bool upgradeSuccessful = false;
        switch(upgradeState.data.currency) {
            case FloatingPopupManager.PopupType.Energy:
                if(playerStats.energy >= upgradeState.CurrentCost) {
                    playerStats.ConsumeEnergy(upgradeState.CurrentCost);
                    upgradeSuccessful = true;
                }
                break;
            case FloatingPopupManager.PopupType.Gold:
                if(playerStats.gold >= upgradeState.CurrentCost) {
                    playerStats.ConsumeGold(upgradeState.CurrentCost);
                    upgradeSuccessful = true;
                }
                break;
            default:
                Debug.LogError("Something went wrong as this upgrade costs an unknown currency");
                break;
        }

        if(upgradeSuccessful) {
            Debug.Log($"Purchased upgrade: {upgradeState.data.upgradeName}");
            upgradeState.level++;
            PopulateUpgrades();

            //TODO: check which upgrade it is, and affect the correct variable by the correct amount
        } else {
            Debug.Log($"Not enough resources to afford {upgradeState.data.upgradeName}");
        }

    }
}