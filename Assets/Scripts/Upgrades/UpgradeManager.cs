using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour {
    public UpgradeCategory category;
    public GameObject upgradePrefab;  // Reference to the prefab
    public Transform upgradeParent;   // Parent object to hold the upgrades
    public Upgrade[] upgrades;       // List of all available upgrades
    public PlayerStats playerStats;

    private void Start() {
        PopulateUpgrades();
    }

    public void PopulateUpgrades() {
        DestroyAllChildren();
        foreach (Upgrade upgrade in upgrades) {
            GameObject upgradeObject = Instantiate(upgradePrefab, upgradeParent);
            UpgradeUI ui = upgradeObject.GetComponent<UpgradeUI>();
            // Set the Text fields
            ui.nameText.text = upgrade.upgradeName;
            ui.descriptionText.text = upgrade.description;
            ui.costText.text = "Cost: " + upgrade.cost;
            ui.levelText.text = "lv. " + upgrade.level;

            // Set the button click event (you can link a method here to handle the upgrade)
            ui.purchaseButton.onClick.AddListener(() => HandleUpgradePurchase(upgrade));
        }
    }

    public void DestroyAllChildren() {
        foreach (Transform child in upgradeParent) {
            Destroy(child.gameObject);
        }
    }

    private void HandleUpgradePurchase(Upgrade upgrade) {
        bool upgradeSuccessful = false;
        switch(upgrade.currency) {
            case FloatingPopupManager.PopupType.Energy:
                if(playerStats.energy >= upgrade.cost) {
                    playerStats.ConsumeEnergy(upgrade.cost);
                    upgradeSuccessful = true;
                }
                break;
            case FloatingPopupManager.PopupType.Gold:
                if(playerStats.gold >= upgrade.cost) {
                    playerStats.ConsumeGold(upgrade.cost);
                    upgradeSuccessful = true;
                }
                break;
            default:
                Debug.LogError("Something went wrong as this upgrade costs an unknown currency");
                break;
        }

        if(upgradeSuccessful) {
            Debug.Log($"Purchased upgrade: {upgrade.upgradeName}");
            upgrade.level++;
            upgrade.cost *= 2;
            PopulateUpgrades();

            //TODO: check which upgrade it is, and affect the correct variable by the correct amount
        } else {
            Debug.Log($"Not enough resources to afford {upgrade.upgradeName}");
        }

    }
}