using UnityEngine;

public enum UpgradeCategory { Maze, IdleClicker, Mouse }

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject {
    public string upgradeName;
    public string description;
    public Sprite icon;
    public int baseCost;
    public FloatingPopupManager.PopupType currency;
    public int baseLevel;
    public int maxLevel;
    public float baseReward;
    public int listOfRequirements; // TODO
    public UpgradeCategory category;

    public int GetCostForLevel(int level) {
        //TODO: a switch case to allow for different formulas per upgrade name/category
        return Mathf.RoundToInt(baseCost * Mathf.Pow(1.15f, level));
    }

    public float GetRewardForLevel(int level) {
        return 2.0f;
    }

    public string GetDescriptionForLevel(int level) {

        if(level == maxLevel) {
            return "Max Level Achieved";
        }

        switch (upgradeName) {
            case "Faster Generation":
                float dif;
                if (level <= 3)
                    dif = 1;
                else
                    dif = 0.5f;
                return $"Reduces generation time from {UpgradeableVariables.GenerationTime}s -> {UpgradeableVariables.GenerationTime - dif}s";
            case "Larger Maze Width":
                break;
            case "Larger Maze Height":
                break;
            case "Mouse Traps":
                break;
            case "Water Pits":
                break;
            default:
                Debug.LogError($"Unknown upgrade: {upgradeName} is requiring a description");
                break;
        }



        return $"Description of upgrade {upgradeName}";
    }
}