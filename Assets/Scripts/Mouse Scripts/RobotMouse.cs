using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMouse : MonoBehaviour {
    [SerializeField] private float moveSpeed = 1f;

    private Queue<Vector3> _waypoints = new Queue<Vector3>();
    private bool _isMoving = false;

    public void SetPath(List<Vector2Int> path, MazeCell[,] mazeGrid) {
        _waypoints.Clear();

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
            while (Vector3.Distance(transform.position, targetPos) > 0.01f) {
                moveSpeed = UpgradeableVariables.MouseSpeed;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        _isMoving = false;
    }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
    }
}
