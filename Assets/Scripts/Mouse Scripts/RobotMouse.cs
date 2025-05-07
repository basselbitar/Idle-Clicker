using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UV = Utilities.UpgradeableVariables;

public class RobotMouse : MonoBehaviour {
    [SerializeField] private float moveSpeed = 1f;

    private Queue<Vector3> _waypoints = new();
    public bool _isMoving;

    private List<Vector2Int> _lastPath;
    private MazeCell[,] _mazeGrid;
    private Vector2Int _startPosition;
    private Vector2Int _currentPosition;
    private Vector2Int _goalPosition;

    private IMouseMovementStrategy _movementStrategy;
    private bool _goalReached;

    public void Initialize(MazeCell[,] mazeGrid, Vector2Int start, Vector2Int goal) {
        _mazeGrid = mazeGrid;
        _startPosition = start;
        _currentPosition = start;
        _goalPosition = goal;
    }
    public void DetermineAndSetMovementStrategy() {
        int intel = UV.MouseIntelligence;
        IMouseMovementStrategy moveStrategy = new RandomMovementStrategy(UV.MouseMaxNumberOfMoves); //TODO: set the limit on the max number of moves
        if (intel >= 1 && intel <= 10) {
            moveStrategy = new RandomMovementStrategy(UV.MouseMaxNumberOfMoves); //TODO: set the limit on the max number of moves as an Upgradeable Variable
        }
        else if (intel >= 11 && intel <= 19) {
            moveStrategy = new BFSMovementStrategy();
        }
        else if (intel == 20) {
            moveStrategy = new MazeSolver();
        }
        else {
            Debug.LogError("UNKNOWN INTELLIGENCE LEVEL");
        }
        SetMovementStrategy(moveStrategy);
    }

    public void SetMovementStrategy(IMouseMovementStrategy strategy) {
        _movementStrategy = strategy;
        GenerateAndSetPath();
    }

    private void GenerateAndSetPath() {
        if (_movementStrategy == null || _mazeGrid == null) {
            Debug.LogWarning("Strategy or Maze not set up properly.");
            return;
        }

        // calculate the path based on the mouse movement strategy that has been set via SetMovementStrategy
        List<Vector2Int> path = _movementStrategy.CalculatePath(_currentPosition, _goalPosition, _mazeGrid);
        _goalReached = path[^1] == _goalPosition;
        SetPath(path, _mazeGrid);

    }


    public void SetPath(List<Vector2Int> path, MazeCell[,] mazeGrid) {
        _waypoints.Clear();
        _lastPath = path;
        _mazeGrid = mazeGrid;

        path.RemoveAt(0); // Delete the (0,0) to not cost the player energy for nothing
        //MazeUtils.PrintPath(path);
        //MazeUtils.PrintPathLength(path);

        foreach (Vector2Int cellPos in path) {
            Vector3 worldPos = mazeGrid[cellPos.x, cellPos.y].transform.position;
            worldPos.y = transform.position.y; // keep mouse on same Y level
            _waypoints.Enqueue(worldPos);
        }

        StartCoroutine(FollowPath());
    }

    public IEnumerator FollowPath() {
        int stepCount = 0;
        while (_waypoints.Count > 0) {
         
            while (!EnergyManager.Instance.HasEnergy(UV.MouseStepCost)) {
                yield return null;
                //TODO: visually tell the user that the mouse is out of energy
                //maybe send mouse to sleep so it can recharge energy faster
                if (!_isMoving) {
                    yield break;
                }
            }

            if (!_isMoving) {
                yield break;
            }

            Vector3 targetPos = _waypoints.Dequeue();

            // spend the energy needed to move and move the mouse
            if (UV.MouseStepCost >= 1) {
                EnergyManager.Instance.ConsumeEnergy(UV.MouseStepCost);
            }
            else {
                //only cost 1 energy every N steps
                stepCount++;
                int N = Mathf.RoundToInt(1f / UV.MouseStepCost);
                if (stepCount >= N) {
                    EnergyManager.Instance.ConsumeEnergy(1);
                    stepCount = 0;
                }
            }

            while (Vector3.Distance(transform.position, targetPos) > 0.01f) {
                moveSpeed = UV.MouseSpeed;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(90f, 0f, 0);
                yield return null;
            }
        }
        GameManager.Instance.OnRoundEnd(_goalReached, _startPosition, _goalPosition, _mazeGrid);
    }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
    }

    private void OnDrawGizmos() {
        if (_lastPath == null || _mazeGrid == null || _lastPath.Count < 2)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < _lastPath.Count - 1; i++) {
            Vector3 from = _mazeGrid[_lastPath[i].x, _lastPath[i].y].transform.position;
            Vector3 to = _mazeGrid[_lastPath[i + 1].x, _lastPath[i + 1].y].transform.position;

            from.y = transform.position.y + 0.1f; // raise a bit above ground
            to.y = from.y;

            Gizmos.DrawLine(from, to);
            Gizmos.DrawSphere(from, 0.1f);
        }

        // Draw sphere at the last point too
        Vector3 last = _mazeGrid[_lastPath[^1].x, _lastPath[^1].y].transform.position;
        last.y = transform.position.y + 0.1f;
        Gizmos.DrawSphere(last, 0.1f);
    }
}
