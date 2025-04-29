using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMouse : MonoBehaviour {
    [SerializeField] private float moveSpeed = 1f;

    private Queue<Vector3> _waypoints = new();
    private bool _isMoving = false;

    private List<Vector2Int> _lastPath;
    private MazeCell[,] _mazeGrid;
    private Vector2Int _currentPosition;
    private Vector2Int _goalPosition;

    private IMouseMovementStrategy _movementStrategy;

    public void Initialize(MazeCell[,] mazeGrid, Vector2Int start, Vector2Int goal) {
        _mazeGrid = mazeGrid;
        _currentPosition = start;
        _goalPosition = goal;
    }

    public void DetermineAndSetMovementStrategy() {
        int intel = UpgradeableVariables.MouseIntelligence;
        IMouseMovementStrategy moveStrategy= new RandomMovementStrategy(); //TODO: set the limit on the max number of moves
        if (intel >= 1 && intel <= 10) {
            moveStrategy = new RandomMovementStrategy(); //TODO: set the limit on the max number of moves as an Upgradeable Variable
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
        GenerateAndFollowPath();
    }

    private void GenerateAndFollowPath() {
        if (_movementStrategy == null || _mazeGrid == null) {
            Debug.LogWarning("Strategy or Maze not set up properly.");
            return;
        }

        // calculate the path based on the mouse movement strategy that has been set via SetMovementStrategy
        List<Vector2Int> path = _movementStrategy.CalculatePath(_currentPosition, _goalPosition, _mazeGrid);
        MazeUtils.PrintPath(path);
        MazeUtils.PrintPathLength(path);
        SetPath(path, _mazeGrid);
    }


    public void SetPath(List<Vector2Int> path, MazeCell[,] mazeGrid) {
        _waypoints.Clear();
        _lastPath = path;
        _mazeGrid = mazeGrid;

        path.RemoveAt(0); // Delete the (0,0) to not cost the player energy for nothing

        foreach (Vector2Int cellPos in path) {
            Vector3 worldPos = mazeGrid[cellPos.x, cellPos.y].transform.position;
            worldPos.y = transform.position.y; // keep mouse on same Y level
            _waypoints.Enqueue(worldPos);
        }

        if (_waypoints.Count > 0) {
            _isMoving = true;
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath() {
        while (_waypoints.Count > 0) {
            Vector3 targetPos = _waypoints.Dequeue();
            while(!EnergyManager.Instance.HasEnergy(UpgradeableVariables.MouseStepCost)) {
                yield return null;
                //TODO: visually tell the user that the mouse is out of energy
            }

            // spend the energy needed to move and move the mouse
            EnergyManager.Instance.ConsumeEnergy(UpgradeableVariables.MouseStepCost);
            while (Vector3.Distance(transform.position, targetPos) > 0.01f) {
                moveSpeed = UpgradeableVariables.MouseSpeed;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(90f, 0f, 0);
                yield return null;
            }
        }

        _isMoving = false;
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
