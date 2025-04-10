using UnityEngine;

public class EnergyManager : MonoBehaviour {
    // Static instance of the EnergyManager, which will be used globally  (Singleton Implementation)
    public static EnergyManager Instance { get; private set; }

    private float energy;
    public float energyPerClick = 1f;
    public float currentEnergy => energy;

    private void Awake() {
        // If an instance already exists, destroy this one to ensure only one instance exists
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            // Set the singleton instance to this object
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps the EnergyManager between scene loads
        }
    }

    // Method to add energy
    public void AddEnergy(float amount) {
        energy += amount;
        Debug.Log($"Energy added: {amount}, Total Energy: {energy}");
    }

    // Optionally, consume energy
    public void ConsumeEnergy(float amount) {
        if (energy >= amount) {
            energy -= amount;
            Debug.Log($"Energy consumed: {amount}, Remaining Energy: {energy}");
        }
        else {
            Debug.Log("Not enough energy!");
        }
    }
}
