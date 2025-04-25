using UnityEngine;

public class EnergyManager : MonoBehaviour {
    // Static instance of the EnergyManager, which will be used globally  (Singleton Implementation)
    public static EnergyManager Instance { get; private set; }

    public float energyPerClick = 1f;

    public PlayerStats playerStats;
    public Sprite energyIcon;

    private void Awake() {
        // If an instance already exists, destroy this one to ensure only one instance exists
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            // Set the singleton instance to this object
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // Keeps the EnergyManager between scene loads
        }
    }

    private void OnEnable() {
        // Subscribe to the InputManager's OnClick event to react to clicks
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null) {
            inputManager.OnClick += HandleClick;  // Subscribe to the click event
        }
    }

    private void OnDisable() {
        // Unsubscribe from the InputManager's OnClick event to prevent memory leaks
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null) {
            inputManager.OnClick -= HandleClick;  // Unsubscribe to avoid unnecessary calls
        }
    }

    // Method to handle energy increase when click occurs
    private void HandleClick(Vector2 screenPosition) {
        // Add energy on click
        AddEnergy(energyPerClick);

        // Visually show the + n Energy Symbol on screen
        // Show floating popup
        FloatingPopupManager.Instance.ShowPopup($"+{energyPerClick}", screenPosition, FloatingPopupManager.PopupType.Energy);
    }


    public void AddEnergy(float amount) {
        playerStats.AddEnergy(amount);
        SaveManager.Save(playerStats);
    }

    public void ConsumeEnergy(float amount) {
        playerStats.ConsumeEnergy(amount);
        SaveManager.Save(playerStats);
    }
}
