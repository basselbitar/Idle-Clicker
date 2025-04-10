using UnityEngine;
using static FloatingPopupManager;

public class FloatingPopupManager : MonoBehaviour {
    public enum PopupType {
        Energy,
        Gold,
        XP
    }
    [Header("Popup Prefab")]
    [SerializeField] private GameObject popupPrefab;

    [SerializeField] private Canvas canvas;

    public static FloatingPopupManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ShowPopup(string message, Vector2 screenPosition, PopupType popupType) {

        // Instantiate the popup prefab and get the FloatingPopup component
        GameObject popupGO = Instantiate(popupPrefab, canvas.transform);
        FloatingPopup popup = popupGO.GetComponent<FloatingPopup>();

        Sprite iconSprite = GetIconForPopupType(popupType);

        // Convert screen position to local position relative to the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, // Use world camera only if Screen Space - Camera
            out Vector2 localPos
        );

        // Set the position of the popup in local space
        popup.transform.localPosition = localPos + new Vector2(-20f, 20f);

        popup.Setup(message, iconSprite);

        // Setup the popup (e.g., show the message and icon)
    }

    private Sprite GetIconForPopupType(PopupType popupType) {
        // Select the appropriate icon based on the type
        switch (popupType) {
            case PopupType.Energy:
                return EnergyManager.Instance.energyIcon;  // You can use the energy icon from EnergyManager
            case PopupType.Gold:
                return GoldManager.Instance.goldIcon;     // Example for a GoldManager
            case PopupType.XP:
                return null;         // Example for an XPManager
            default:
                return null;
        }
    }
}
