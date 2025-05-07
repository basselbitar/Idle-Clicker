using UnityEngine;
using Utilities;

public class GoldCoin : MonoBehaviour
{
    public int value;

    public void Awake() {
        value = UpgradeableVariables.GoldValue;
    }
}
