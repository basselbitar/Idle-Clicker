using UnityEngine;

public class FloatingPopupManager : MonoBehaviour {
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Canvas canvas;

    public static FloatingPopupManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ShowPopup(string message, Vector2 screenPosition, Sprite iconSprite = null) {

        // Instantiate the popup prefab and get the FloatingPopup component
        GameObject popupGO = Instantiate(popupPrefab, canvas.transform);
        FloatingPopup popup = popupGO.GetComponent<FloatingPopup>();

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
}
