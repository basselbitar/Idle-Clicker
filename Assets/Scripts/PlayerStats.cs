using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject {
    public float gold;
    public float energy;
    public float experience;

    public void AddGold(float amount) {
        gold += amount;
        //Debug.Log($"Gold added: {amount}, Total Gold: {gold}");
        ResourceUIManager.Instance.RefreshUI();
    }

    public void AddEnergy(float amount) {
        energy += amount;
        //Debug.Log($"Energy added: {amount}, Total Energy: {energy}");
        ResourceUIManager.Instance.RefreshUI();
    }

    public void AddExperience(float amount) {
        experience += amount;
        ResourceUIManager.Instance.RefreshUI();
    }

    public void ConsumeGold(float amount) {
        if (gold >= amount) {
            gold -= amount;
            //Debug.Log($"Gold consumed: {amount}, Remaining Gold: {gold}");
            ResourceUIManager.Instance.RefreshUI();
        }
        else {
            Debug.Log("Not enough gold!");
            //TODO: color the gold UI red for a few seconds
        }
    }

    public void ConsumeEnergy(float amount) {
        if (energy >= amount) {
            energy -= amount;
            //Debug.Log($"Energy consumed: {amount}, Remaining Energy: {energy}");
            ResourceUIManager.Instance.RefreshUI();
        }
        else {
            Debug.Log("Not enough energy!");
        }
    }

    public void ConsumeExperience(float amount) {
        if (experience >= amount) {
            experience -= amount;
            ResourceUIManager.Instance.RefreshUI();
        }
        else {
            Debug.Log("Not enough experience!");
        }
    }
}
