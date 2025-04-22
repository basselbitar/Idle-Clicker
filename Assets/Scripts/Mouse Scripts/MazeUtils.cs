using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class MazeUtils {
    public static List<Vector2Int> GetNeighbors(Vector2Int pos, MazeCell[,] maze) {
        var neighbors = new List<Vector2Int>();
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);
        int x = pos.x;
        int y = pos.y;

        MazeCell current = maze[x, y];

        if (!current.HasRightWall && x + 1 < width)
            neighbors.Add(new Vector2Int(x + 1, y));
        if (!current.HasLeftWall && x - 1 >= 0)
            neighbors.Add(new Vector2Int(x - 1, y));
        if (!current.HasFrontWall && y + 1 < height)
            neighbors.Add(new Vector2Int(x, y + 1));
        if (!current.HasBackWall && y - 1 >= 0)
            neighbors.Add(new Vector2Int(x, y - 1));

        return neighbors;
    }

    public static void PrintPath(List<Vector2Int> path) {
        StringBuilder sb = new StringBuilder();
        sb.Append("The Path is: ");
        foreach (Vector2Int pos in path) {
            sb.Append($" ({pos.x},{pos.y}),");
        }
        Debug.Log(sb.ToString());
    }
}
