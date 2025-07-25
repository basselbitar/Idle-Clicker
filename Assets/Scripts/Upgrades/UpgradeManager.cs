using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UV = Utilities.UpgradeableVariables;

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

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Comma)) {
            playerStats.AddEnergy(1000);
        }
        if (Input.GetKeyUp(KeyCode.M)) {
            playerStats.ConsumeEnergy(1000);
        }
        if (Input.GetKeyUp(KeyCode.Period)) {
            playerStats.AddGold(100);
        }

        if (Input.GetKey(KeyCode.V)) {
            playerStats.ConsumeEnergy(10);
            playerStats.ConsumeGold(10);
        }

        if (Input.GetKeyUp(KeyCode.B)) {
            playerStats.AddEnergy(10);
        }

        if (Input.GetKeyUp(KeyCode.Z)) {
            if (runtimeUpgrades[0].data.category == UpgradeCategory.Maze) {
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PopulateUpgrades();
            }
        }

        if (Input.GetKeyUp(KeyCode.X)) {
            if (runtimeUpgrades[0].data.category == UpgradeCategory.Mouse) {
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PerformUpgrade(runtimeUpgrades[0]);
                PopulateUpgrades();
            }
        }
    }

    private void Initialize() {
        //Debug.Log(upgrades.Select(upgradeSO => new UpgradeState { data = upgradeSO }).ToList()[0].data.upgradeName);
        //Debug.Log(upgrades.Select(upgradeSO => new UpgradeState { data = upgradeSO }).ToList()[1].data.upgradeName);
        //Debug.Log(upgrades.Select(upgradeSO => new UpgradeState { data = upgradeSO }).ToList()[2].data.upgradeName);
        runtimeUpgrades = upgrades.Select(upgradeSO => new UpgradeState { data = upgradeSO }).ToList();
    }

    public void PopulateUpgrades() {
        DestroyAllChildren();
        foreach (UpgradeState upgradeState in runtimeUpgrades) {
            GameObject upgradeObject = Instantiate(upgradePrefab, upgradeParent);
            UpgradeUI ui = upgradeObject.GetComponent<UpgradeUI>();
            ui.icon.sprite = upgradeState.data.icon;
            // Set the Text fields
            ui.nameText.text = upgradeState.data.upgradeName;
            ui.descriptionText.text = upgradeState.Description; // gets the current description based on the level
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
        if(upgradeState.level >= upgradeState.data.maxLevel) {
            return;
        }

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
            //Debug.Log($"Purchased upgrade: {upgradeState.data.upgradeName}");
            PerformUpgrade(upgradeState);
            PopulateUpgrades();

            //TODO: check which upgrade it is, and affect the correct variable by the correct amount
        } else {
            Debug.Log($"Not enough resources to afford {upgradeState.data.upgradeName}");
        }

    }

    private void PerformUpgrade(UpgradeState upgradeState) {
        string upgradeName = upgradeState.data.upgradeName;
        int level = upgradeState.level;
        if (level == upgradeState.data.maxLevel) {
            return;
        }

        upgradeState.level++;

        switch (upgradeName) {
            // Maze Upgrades
            case UpgradeNames.MAZE_FASTER_GENERATION:
                UV.GenerationTime = upgradeState.data.rewards[level];
                break;
            case UpgradeNames.MAZE_WIDTH:
                UV.MaxMapWidth = (int) upgradeState.data.rewards[level];
                CreateAndSolveUIManager.Instance.UpdateMazePrice();
                break;
            case UpgradeNames.MAZE_HEIGHT:
                UV.MaxMapHeight = (int) upgradeState.data.rewards[level];
                CreateAndSolveUIManager.Instance.UpdateMazePrice();
                break;
            case UpgradeNames.MAZE_GOLD_CHANCE:
                UV.ExtraGoldSpawnProbabilityPerCell = upgradeState.data.rewards[level];
                break;
            case UpgradeNames.MAZE_GOLD_GUARANTEED:
                UV.ExtraGoldSpawnCount = (int) upgradeState.data.rewards[level];
                break;
            case UpgradeNames.MAZE_TRAPS:
                break;
            case UpgradeNames.MAZE_WATER:
                break;

            //Idle Upgrades
            case UpgradeNames.IDLE_FACTORY_1:
                break;
            case UpgradeNames.IDLE_FACTORY_2:
                break;
            case UpgradeNames.IDLE_FACTORY_3:
                break;
            case UpgradeNames.IDLE_GOLD_VALUE:
                UV.GoldValue = (int) upgradeState.data.rewards[level];
                break;
            case UpgradeNames.IDLE_XP_VALUE:
                UV.ExperienceGainedPerTile = (int) upgradeState.data.rewards[level];
                break;



            //Mouse Upgrades
            case UpgradeNames.MOUSE_SPEED:
                UV.MouseSpeed = upgradeState.data.rewards[level];
                break;
            case UpgradeNames.MOUSE_INTELLIGENCE:
                UV.MouseIntelligence = (int)upgradeState.data.rewards[level];
                UV.MouseMaxNumberOfMoves = 50 + (10 * level);
                break;
            case UpgradeNames.MOUSE_STAMINA:
                UV.MouseStepCost = upgradeState.data.rewards[level];
                break;
            case UpgradeNames.MOUSE_DOUBLE_RUN:
                break;
            case UpgradeNames.MOUSE_SNIFF_BOOST:
                break;


            default:
                Debug.LogError($"Unknown upgrade: {upgradeName} is performing an Upgrade");
                break;

        }


        //TODO: disable button if max level is reached
    }
}