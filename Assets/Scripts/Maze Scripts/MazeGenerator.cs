using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UV = Utilities.UpgradeableVariables;


public class MazeGenerator : MonoBehaviour {

    public static MazeGenerator Instance { get; private set; }

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeHeight;

    private MazeCell[,] _mazeGrid;

    [SerializeField]
    private float _timeToGenerate;
    private float _breakWaitTime;

    [SerializeField]
    private GameObject _mousePrefab;

    public bool IsGenerating { get; private set; }

    [SerializeField]
    private List<Color> _wallColours;
    [SerializeField]
    private Material _mazeMaterial;

    private int _colorIndex;

    //debug only
    private float _startTime;
    private float _endTime;
    private float _visitedCounter;
    public bool ShowDebugText;

    private void Awake() {
        // Enforce singleton instance
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        Instance = this;
    }

    public void GenerateAMaze() {
        _timeToGenerate = UV.GenerationTime;
        _mazeWidth = UV.MaxMapWidth;
        _mazeHeight = UV.MaxMapHeight;


        _mazeGrid = new MazeCell[_mazeWidth, _mazeHeight];
        //try to find Maze GO, destroy it and instantiate a new one
        GameObject MazeGO = GameObject.Find("Maze");
        if (MazeGO != null) {
            Destroy(MazeGO);
        }
        MazeGO = new GameObject("Maze");

        for (int x = 0; x < _mazeWidth; x++) {
            for (int z = 0; z < _mazeHeight; z++) {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                _mazeGrid[x, z].UpdateDebugText(new(x, z));
                _mazeGrid[x, z].transform.SetParent(MazeGO.transform);
                if(!ShowDebugText) {
                    _mazeGrid[x, z].HideDebugText();
                }
            }
        }
        CameraManager.Instance.SetCameraPos(_mazeWidth, _mazeHeight);
        int totalCells = _mazeWidth * _mazeHeight;
        _visitedCounter = totalCells;
        _breakWaitTime = _timeToGenerate / totalCells;
        //Debug.Log("Break Wait Time = " + _breakWaitTime);
        _startTime = Time.time;
        IsGenerating = true;
        CreateAndSolveUIManager.Instance.DisableButton();
        GameObject MouseGO = GameObject.Find("Mouse(Clone)");
        if (MouseGO != null) {
            Destroy(MouseGO);
        }
        RandomizeColour();
        StartCoroutine(GenerateMaze(null, _mazeGrid[0, 0]));
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell) {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(_breakWaitTime);

        MazeCell nextCell;

        do {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null) {
                yield return StartCoroutine(GenerateMaze(currentCell, nextCell));
            }
        } while (nextCell != null);

        if (previousCell == null) {
            IsGenerating = false;
            CreateAndSolveUIManager.Instance.EnableButton();
            SpawnMouse();
        }
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell) {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.value).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell) {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth) {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited) {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0) {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited) {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeHeight) {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited) {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0) {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited) {
                yield return cellToBack;
            }
        }

    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell) {
        if (previousCell == null) {
            return;
        }

        //debug
        _visitedCounter--;

        if (previousCell.transform.position.x < currentCell.transform.position.x) {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x) {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z) {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z) {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void SpawnMouse() {
        GameObject MouseGO = GameObject.Find("Mouse(Clone)");
        if (MouseGO != null) {
            Destroy(MouseGO);
        }
        MouseGO = Instantiate(_mousePrefab, new Vector3(0, 0.5f, 0f), new Quaternion(-90f, 0, 0, 0));
        MouseGO.transform.eulerAngles = new Vector3(90f, 0f, 0);
        MouseGO.transform.localScale = new Vector3(0.08f, 0.08f, 0.6f);

        SolveMaze(MouseGO);
    }

    private void SolveMaze(GameObject mouse) {

        //TODO: move all of these to a mouse manager that responds to a button being clicked before purchasing the Mouse Manager

        RobotMouse robotMouse = mouse.GetComponent<RobotMouse>();

        Vector2Int start = new(0, 0);
        Vector2Int goal = new(_mazeWidth - 1, _mazeHeight - 1);
        robotMouse.Initialize(_mazeGrid, start, goal);
        robotMouse._isMoving = CreateAndSolveUIManager.Instance.autoSolvePuzzles;
        robotMouse.DetermineAndSetMovementStrategy();
        //placeholder target location is the last cell in the maze
        //List<Vector2Int> path = ms.FindPath(, );
        //robotMouse.SetPath(path, _mazeGrid);
    }

    private void RandomizeColour() {
        _mazeMaterial.color = _wallColours[_colorIndex++ % _wallColours.Count];
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            GenerateAMaze();
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            SpawnMouse();
        }

        if(Input.GetKeyDown(KeyCode.C)) {
            RandomizeColour();
        }
    }
}
