using UnityEngine;

public enum UpgradeCategory { Maze, Idle, Mouse }

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject {
    public string upgradeName;
    public string description;
    public Sprite icon;
    public int baseCost;
    public float growthRate;
    public FloatingPopupManager.PopupType currency;
    public int baseLevel;
    public int maxLevel;
    public float [] rewards;
    public int listOfRequirements;
    public UpgradeCategory category;

    public int GetCostForLevel(int level) {
        //TODO: a switch case to allow for different formulas per upgrade name/category
        return Mathf.RoundToInt(baseCost * Mathf.Pow(growthRate, level));
    }

    public float GetRewardForLevel(int level) {
        return 2.0f;
    }

    public string GetDescriptionForLevel(int level) {

        if(level == maxLevel) {
            return "Max Level Achieved";
        }

        switch (upgradeName) {
            // ----------------- Map Generation Upgrades -----------------
            case UpgradeNames.MAZE_FASTER_GENERATION:
                return $"Reduces generation time from {rewards[level - 1]}s -> {rewards[level]}s";
            case UpgradeNames.MAZE_WIDTH:
                return $"Increases maze width from {Mathf.RoundToInt(rewards[level - 1])} -> {Mathf.RoundToInt(rewards[level])}";
            case UpgradeNames.MAZE_HEIGHT:
                return $"Increases maze height from {Mathf.RoundToInt(rewards[level - 1])} -> {Mathf.RoundToInt(rewards[level])}";
            case UpgradeNames.MAZE_GOLD_CHANCE:
                return $"Increases each cell's chance of gold from {Mathf.RoundToInt(rewards[level - 1] * 100)}% -> {Mathf.RoundToInt(rewards[level] * 100)}%";
            case UpgradeNames.MAZE_GOLD_GUARANTEED:
                return $"Spawns an extra {Mathf.RoundToInt(rewards[level - 1])} -> {Mathf.RoundToInt(rewards[level])} gold per level";
            case UpgradeNames.MAZE_TRAPS:
                break;
            case UpgradeNames.MAZE_WATER:
                break;


            // ----------------- Idle Upgrades -----------------
            case UpgradeNames.IDLE_FACTORY_1:
                return $"Produce energy at: {rewards[level - 1]} -> {rewards[level]}";
            case UpgradeNames.IDLE_FACTORY_2:
                return $"Produce energy at {rewards[level - 1]} -> {rewards[level]}";
            case UpgradeNames.IDLE_FACTORY_3:
                return $"Factory 3 Description";
            case UpgradeNames.IDLE_GOLD_VALUE:
                return $"Increase the gold collected from {Mathf.RoundToInt(rewards[level - 1])} -> {Mathf.RoundToInt(rewards[level])}";
            case UpgradeNames.IDLE_XP_VALUE:
                return $"Increase the XP per tile from {Mathf.RoundToInt(rewards[level - 1])} -> {Mathf.RoundToInt(rewards[level])}";


            // ----------------- Mouse Upgrades -----------------
            case UpgradeNames.MOUSE_SPEED:
                return $"Increases speed from {rewards[level-1]} -> {rewards[level]}";
            case UpgradeNames.MOUSE_INTELLIGENCE:
                return $"Increases intelligence from {rewards[level-1]} -> {rewards[level]}";
            case UpgradeNames.MOUSE_STAMINA:
                return $"Robomouse only needs {System.Math.Round(rewards[level - 1], 2)} -> {System.Math.Round(rewards[level], 2)} per move";
            case UpgradeNames.MOUSE_DOUBLE_RUN:
                return $"Solve the puzzle twice at a 1% chance of proc.";
            case UpgradeNames.MOUSE_SNIFF_BOOST:
                return $"The mouse solves the maze with the shortest path immediately";
            default:
                Debug.LogError($"Unknown upgrade: {upgradeName} is requiring a description");
                return "Placeholder Description";
        }

        return $"Description of upgrade {upgradeName}";
    }
}