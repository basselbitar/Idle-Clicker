[System.Serializable]
public class UpgradeState {
    public Upgrade data;
    public int level = 1;

    public int CurrentCost => data.GetCostForLevel(level);

    public string Description => data.GetDescriptionForLevel(level);
}