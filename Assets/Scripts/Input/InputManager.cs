using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    private PlayerControls controls;

    // Define the target area as a percentage of the screen width (e.g., 50% means the right half of the screen)
    [SerializeField] private float targetAreaX = 0.5f;

    // Event to broadcast clicks
    public delegate void OnClickAction(Vector2 screenPosition);
    public event OnClickAction OnClick;

    private void Awake() {
        controls = new PlayerControls();
    }

    private void OnEnable() {
        controls.Enable();

        // Subscribe to the click event in the input system
        controls.Gameplay.Click.performed += OnClickPerformed;
    }

    private void OnDisable() {
        controls.Disable();

        // Unsubscribe from the click event to avoid potential memory leaks
        controls.Gameplay.Click.performed -= OnClickPerformed;
    }

    // Method called when the mouse click is performed
    private void OnClickPerformed(InputAction.CallbackContext context) {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Check if the click is in the defined target area (e.g., right half of the screen)
        if (mousePos.x > Screen.width * targetAreaX) {
            OnClick?.Invoke(mousePos);

            //Debug.Log("Click within target area at " + mousePos);
        }
    }
}
