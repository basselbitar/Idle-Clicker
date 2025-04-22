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
    public float [] rewards;
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
            // ----------------- Map Generation Upgrades -----------------
            case UpgradeNames.MAZE_FASTER_GENERATION:
                return $"Reduces generation time from {rewards[level - 1]}s -> {rewards[level]}s";
            case UpgradeNames.MAZE_WIDTH:
                return $"Increases maze width from {(int) rewards[level - 1]} -> {(int)rewards[level]}";
            case UpgradeNames.MAZE_HEIGHT:
                return $"Increases maze height from {(int)rewards[level - 1]} -> {(int)rewards[level]}";
            case UpgradeNames.MAZE_TRAPS:
                break;
            case UpgradeNames.MAZE_WATER:
                break;


            // ----------------- Idle Upgrades -----------------

            // ----------------- Mouse Upgrades -----------------

            case UpgradeNames.MOUSE_SPEED:
                return $"Increases speed from {rewards[level-1]} -> {rewards[level]}";
            case UpgradeNames.MOUSE_AGILITY:
                return $"Increases agility of the mouse";
            case UpgradeNames.MOUSE_STAMINA:
                return $"Increases stamina so you can solve more puzzles";
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