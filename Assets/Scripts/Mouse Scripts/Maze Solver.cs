using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : IMouseMovementStrategy {

    // Calculates the Shortest Path between two points using BFS
    public List<Vector2Int> CalculatePath(Vector2Int start, Vector2Int goal, MazeCell[,] maze) {
        var visited = new HashSet<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var queue = new Queue<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (current == goal)
                break;

            foreach (var neighbor in MazeUtils.GetNeighbors(current, maze)) {
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
        return path;
    }
}
