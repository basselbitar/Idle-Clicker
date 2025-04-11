using UnityEngine;

public static class SaveManager {
    public static void Save(PlayerStats stats) {
        PlayerPrefs.SetFloat("Gold", stats.gold);
        PlayerPrefs.SetFloat("Energy", stats.energy);
        PlayerPrefs.Save();
    }

    public static void Load(PlayerStats stats) {
        stats.gold = PlayerPrefs.GetFloat("Gold", 0f);
        stats.energy = PlayerPrefs.GetFloat("Energy", 0f);
    }
}