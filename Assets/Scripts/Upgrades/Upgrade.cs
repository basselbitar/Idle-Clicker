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
    public float baseReward;
    public int listOfRequirements; // TODO
    public UpgradeCategory category;

    public int GetCostForLevel(int level) {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(1.15f, level));
    }
}