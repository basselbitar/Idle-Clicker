using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject {
    public float gold;
    public float energy;

    public void AddGold(float amount) {
        gold += amount;
        Debug.Log($"Gold added: {amount}, Total Gold: {gold}");
    }

    public void AddEnergy(float amount) {
        energy += amount;
        Debug.Log($"Energy added: {amount}, Total Energy: {energy}");
    }

    public void ConsumeGold(float amount) {
        if (gold >= amount) {
            gold -= amount;
            Debug.Log($"Gold consumed: {amount}, Remaining Gold: {gold}");
        }
        else {
            Debug.Log("Not enough gold!");
        }
    }

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
