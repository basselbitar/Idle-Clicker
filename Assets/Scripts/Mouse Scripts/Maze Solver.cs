using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour {
    private MazeCell[,] _mazeGrid;
    private int _width;
    private int _height;

    public void Init(MazeCell[,] mazeGrid, int width, int height) {
        _mazeGrid = mazeGrid;
        _width = width;
        _height = height;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal) {
        return BFS(start, goal);
    }

    public List<Vector2Int> BFS(Vector2Int start, Vector2Int goal) {
        var visited = new HashSet<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var queue = new Queue<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (current == goal)
                break;

            foreach (var neighbor in GetNeighbors(current)) {
                if (!visited.Contains(neighbor)) {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        var path = new List<Vector2Int>();
        if (!cameFrom.ContainsKey(goal)) {
            Debug.Log(goal);
            Debug.Log("Did not find a Path to get to the goal!");
            return path; // No path found
        }

        var node = goal;
        while (node != start) {
            path.Add(node);
            node = cameFrom[node];
        }
        path.Add(start);
        path.Reverse();
        Debug.Log(path.ToArray());
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos) {
        var neighbors = new List<Vector2Int>();
        int x = pos.x;
        int z = pos.y;

        MazeCell current = _mazeGrid[x, z];

        // Check each direction based on wall presence
        if (!current.HasRightWall && x + 1 < _width)
            neighbors.Add(new Vector2Int(x + 1, z));
        if (!current.HasLeftWall && x - 1 >= 0)
            neighbors.Add(new Vector2Int(x - 1, z));
        if (!current.HasFrontWall && z + 1 < _height)
            neighbors.Add(new Vector2Int(x, z + 1));
        if (!current.HasBackWall && z - 1 >= 0)
            neighbors.Add(new Vector2Int(x, z - 1));

        return neighbors;
    }
}
