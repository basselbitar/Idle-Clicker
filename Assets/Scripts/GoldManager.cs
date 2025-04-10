using UnityEngine;
using static FloatingPopupManager;

public class GoldManager : MonoBehaviour {
    public static GoldManager Instance { get; private set; }

    public Sprite goldIcon;  // Gold icon for popups

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddGold(int amount) {
        // Code to add gold and show the popup
        Vector2 screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        FloatingPopupManager.Instance.ShowPopup($"+{amount}", screenPosition, PopupType.Gold);
    }
}