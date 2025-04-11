using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIManager : MonoBehaviour {
    public static ResourceUIManager Instance { get; private set; }

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text energyText;

    public PlayerStats playerStats;

    // Initialize the singleton instance
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Refresh the UI when stats change
    public void RefreshUI() {
        energyText.text = playerStats.energy.ToString("0");
        goldText.text = " " + playerStats.gold.ToString("0");
    }
}
