using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UV = Utilities.UpgradeableVariables;

public class CreateAndSolveUIManager : MonoBehaviour {
    public static CreateAndSolveUIManager Instance { get; private set; }

    [SerializeField]
    private Button _createMazeButton;

    [SerializeField]
    private Button _solveMazeButton;

    [SerializeField]
    private TMP_Text _createButtonText;

    [SerializeField]
    private TMP_Text _solveButtonText;

    private int _mazePrice;

    public bool autoSolvePuzzles;

    private void Awake() {
        // Enforce singleton instance
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        Instance = this;
    }

    private void Start() {
        UpdateMazePrice();
    }

    public void UpdateMazePrice() {
        _mazePrice = UV.MaxMapWidth * UV.MaxMapHeight;
        _createButtonText.text = $"Build Maze ({_mazePrice})";
    }

    public void OnClickGenerateMaze() {

        if (!EnergyManager.Instance.HasEnergy(_mazePrice)) {
            Debug.Log("Something went wrong. This button should have been disabled as the player can't afford a new maze");
            return;
        }

        UpdateMazePrice();
        EnergyManager.Instance.ConsumeEnergy(_mazePrice);


        Vector2 temp = GetComponent<RectTransform>().localPosition;
        temp.x = (-Screen.width / 4f) - (_createMazeButton.GetComponent<RectTransform>().rect.width / 2);
        temp.y = (-Screen.height / 2f) + _createMazeButton.GetComponent<RectTransform>().rect.height;
        _createMazeButton.GetComponent<RectTransform>().localPosition = temp;
        _solveMazeButton.gameObject.SetActive(true);
        _solveMazeButton.GetComponent<RectTransform>().localPosition = temp + new Vector2(_createMazeButton.GetComponent<RectTransform>().rect.width + 10f, 0);

        MazeGenerator.Instance.GenerateAMaze();
    }

    public void OnClickSolveMaze() {
        //toggle the mouse solving
        autoSolvePuzzles = !autoSolvePuzzles;
        if (autoSolvePuzzles) {
            FindObjectOfType<RobotMouse>()._isMoving = true;
            StartCoroutine(FindObjectOfType<RobotMouse>().FollowPath());
            _solveButtonText.text = "Stop";
        }
        else {
            FindObjectOfType<RobotMouse>()._isMoving = false;
            _solveButtonText.text = $"Solve ({System.Math.Round(UV.MouseStepCost, 2)})";
        }
    }

    public void DisableButton() {
        _createMazeButton.interactable = false;
        _solveMazeButton.interactable = false;
    }

    public void EnableButton() {
        _createMazeButton.interactable = true;
        _solveMazeButton.interactable = true;
    }
}
