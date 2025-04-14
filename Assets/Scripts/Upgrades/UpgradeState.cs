[System.Serializable]
public class UpgradeState {
    public Upgrade upgradeData;
    public int level = 0;

    public int CurrentCost => upgradeData.GetCostForLevel(level);
}