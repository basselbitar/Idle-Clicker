using UnityEngine;

public class MouseCollector : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.TryGetComponent(out GoldCoin coin)) {
            GoldManager.Instance.AddGold(coin.value);
            FloatingPopupManager.Instance.ShowPopup($"+{coin.value}", other.transform.position, FloatingPopupManager.PopupType.Gold);
            Destroy(other.gameObject);
        }
    }
}