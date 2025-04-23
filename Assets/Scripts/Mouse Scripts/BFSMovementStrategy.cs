using System.Collections.Generic;
using UnityEngine;

public class BFSMovementStrategy : IMouseMovementStrategy {
    public List<Vector2Int> CalculatePath(MazeCell[,] maze, Vector2Int start, Vector2Int goal) {
        Queue<Vector2Int> queue = new();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new(); // For reconstructing paths
        HashSet<Vector2Int> visited = new();

        List<Vector2Int> movementPath = new(); // Final step-by-step movement path
        Vector2Int currentPos = start;

        queue.Enqueue(start);
        visited.Add(start);
        movementPath.Add(start);

        while (queue.Count > 0) {
            Vector2Int targetNode = queue.Dequeue();

            // Backtrack to this node from currentPos (if not already there)
            if (currentPos != targetNode) {
                if (!cameFrom.ContainsKey(targetNode)) {
                    Debug.LogWarning($"Cannot backtrack to {targetNode} — no path found.");
                    continue;
                }

                List<Vector2Int> backtrackPath = ReconstructPath(cameFrom, currentPos, targetNode);
                backtrackPath.RemoveAt(0); // Avoid duplicating currentPos
                movementPath.AddRange(backtrackPath);
                currentPos = targetNode;
            }

            if (currentPos == goal)
                break;

            foreach (Vector2Int neighbor in MazeUtils.GetNeighbors(currentPos, maze)) {
                if (!visited.Contains(neighbor)) {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = currentPos;
                }
            }
        }

        return movementPath;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end) {
        List<Vector2Int> path = new();
        Vector2Int current = end;

        while (current != start) {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
}