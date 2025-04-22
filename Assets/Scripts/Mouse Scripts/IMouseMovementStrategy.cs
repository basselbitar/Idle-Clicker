using System.Collections.Generic;
using UnityEngine;

public interface IMouseMovementStrategy {
    /// <summary>
    /// Calculates a path through the maze from the start position to the goal.
    /// </summary>
    /// <param name="maze">The maze grid with cell data.</param>
    /// <param name="start">Starting grid position of the mouse.</param>
    /// <param name="goal">Target grid position to reach.</param>
    /// <returns>A list of grid positions representing the path from start to goal.</returns>
    List<Vector2Int> CalculatePath(MazeCell[,] maze, Vector2Int start, Vector2Int goal);
}