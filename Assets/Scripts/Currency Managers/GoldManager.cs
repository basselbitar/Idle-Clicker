using UnityEngine;
using static FloatingPopupManager;

public class GoldManager : MonoBehaviour {
    public static GoldManager Instance { get; private set; }

    public PlayerStats playerStats;
    public Sprite goldIcon;  // Gold icon for popups

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void AddGold(float amount) {
        playerStats.AddGold(amount);
        SaveManager.Save(playerStats);
    }

    public void ConsumeGold(float amount) {
        playerStats.ConsumeGold(amount);
        SaveManager.Save(playerStats);
    }
}