using System.Collections.Generic;
using UnityEngine;

public class RandomMovementStrategy : IMouseMovementStrategy {
    private readonly int _maxMoves;

    public RandomMovementStrategy(int maxMoves = 100) {
        _maxMoves = maxMoves;
    }

    public List<Vector2Int> CalculatePath(MazeCell[,] maze, Vector2Int start, Vector2Int goal) {
        List<Vector2Int> path = new List<Vector2Int> { start };
        Vector2Int current = start;
        int moveCount = 0;

        while (moveCount < _maxMoves && current != goal) {
            List<Vector2Int> validMoves = MazeUtils.GetNeighbors(current, maze);

            // Avoid going backwards only if other options exist
            if (path.Count > 1 && validMoves.Count > 1) {
                Vector2Int previous = path[path.Count - 2];
                validMoves.Remove(previous);
            }

            if (validMoves.Count == 0)
                break;

            current = validMoves[Random.Range(0, validMoves.Count)];
            path.Add(current);
            moveCount++;
        }

        return path;
    }
}
