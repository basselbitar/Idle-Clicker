using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField]
    private Camera _mainCamera;

    private void Awake() {
        // Enforce singleton instance
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_mainCamera == null) {
            Debug.LogError("No camera assigned");
            _mainCamera = Camera.main;
        }
    }

   public void SetCameraPos(int MazeWidth,  int MazeHeight) {
        //Debug.Log("Camera received " + MazeWidth + "x" + MazeHeight);
        float xCoord = (MazeWidth * 1.0f / 2) - 0.5f;
        float yCoord = (Mathf.Max(MazeWidth,MazeHeight) + 2f);
        float zCoord = (MazeHeight * 1.0f / 1.5f);
        _mainCamera.transform.position = new Vector3(xCoord, yCoord, zCoord);
    }
}
