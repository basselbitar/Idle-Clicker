using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFSMovementStrategy : IMouseMovementStrategy {
    public List<Vector2Int> CalculatePath(Vector2Int start, Vector2Int goal, MazeCell[,] maze) {
        Queue<Vector2Int> queue = new();
        
        HashSet<Vector2Int> visited = new();
        List<Vector2Int> movementPath = new();
        Vector2Int currentPos = start;

        queue.Enqueue(start);
        movementPath.Add(start);

        MazeSolver mazeSolver = new();

        while (queue.Count > 0) {
            Vector2Int current = queue.Dequeue();
            visited.Add(current);

            if ( currentPos != current) {
                if( MazeUtils.AreNeighbors(currentPos, current, maze)) {
                    movementPath.Add(current);
                } else {
                    // if the mouse is trying to continue from another cell that isn't its neighbor, 
                    // it needs to navigate back to it
                    movementPath.AddRange(mazeSolver.CalculatePath(currentPos, current, maze));
                }

            }

            if (current == goal)
                break;

            foreach (Vector2Int neighbor in MazeUtils.GetNeighbors(current, maze)) {
                if (!visited.Contains(neighbor)) {
                    queue.Enqueue(neighbor);
                }
            }
            currentPos = current;
        }

        // Now use ConstructPathWithBacktracking and pass the maze
        //List<Vector2Int> movementSteps = ConstructPathWithBacktracking(cameFrom, start, currentPos, goal, maze);
        //movementPath.AddRange(movementSteps);

        return movementPath;
    }



    private bool IsPathClear(Vector2Int from, Vector2Int to, MazeCell[,] maze) {
        MazeCell fromCell = maze[from.x, from.y];
        MazeCell toCell = maze[to.x, to.y];
        Vector2Int diff = to - from;

        if (diff == Vector2Int.up) return !fromCell.HasFrontWall && !toCell.HasBackWall;
        if (diff == Vector2Int.down) return !fromCell.HasBackWall && !toCell.HasFrontWall;
        if (diff == Vector2Int.left) return !fromCell.HasLeftWall && !toCell.HasRightWall;
        if (diff == Vector2Int.right) return !fromCell.HasRightWall && !toCell.HasLeftWall;

        return false; // Not a valid cardinal neighbor
    }


    private List<Vector2Int> ConstructPathWithBacktracking(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int currentPos, Vector2Int target, MazeCell[,] maze) {
        List<Vector2Int> path = new();
        Vector2Int current = currentPos;

        // Ensure we start backtracking properly
        while (current != start) {
            if (!cameFrom.ContainsKey(current)) {
                Debug.LogWarning($"Key not found while reconstructing path: {current}");
                return new List<Vector2Int>(); // Return empty if no valid path is found
            }

            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);

        // Reverse path to start from the beginning
        path.Reverse();

        // Ensure the path is valid (no jumps or diagonals)
        List<Vector2Int> sanitizedPath = new List<Vector2Int>();
        sanitizedPath.Add(path[0]);

        for (int i = 1; i < path.Count; i++) {
            Vector2Int prev = sanitizedPath[sanitizedPath.Count - 1];
            Vector2Int curr = path[i];

            // Validate path, ensuring each step is a valid neighbor
            if (IsValidNeighbor(prev, curr, maze)) {
                sanitizedPath.Add(curr);
            }
            else {
                Debug.LogWarning($"Invalid move from {prev} to {curr}");
                break; // Stop adding invalid moves
            }
        }

        return sanitizedPath;
    }

    private bool IsValidNeighbor(Vector2Int from, Vector2Int to, MazeCell[,] maze) {
        Vector2Int diff = to - from;

        if (diff == Vector2Int.up) return !maze[from.x, from.y].HasFrontWall && !maze[to.x, to.y].HasBackWall;
        if (diff == Vector2Int.down) return !maze[from.x, from.y].HasBackWall && !maze[to.x, to.y].HasFrontWall;
        if (diff == Vector2Int.left) return !maze[from.x, from.y].HasLeftWall && !maze[to.x, to.y].HasRightWall;
        if (diff == Vector2Int.right) return !maze[from.x, from.y].HasRightWall && !maze[to.x, to.y].HasLeftWall;

        return false; // Not a valid cardinal neighbor
    }

    private List<Vector2Int> SanitizePath(List<Vector2Int> rawPath, MazeCell[,] maze) {
        List<Vector2Int> legalPath = new();
        if (rawPath.Count == 0) return legalPath;

        legalPath.Add(rawPath[0]); // Always include the starting point

        for (int i = 1; i < rawPath.Count; i++) {
            Vector2Int from = legalPath[^1];
            Vector2Int to = rawPath[i];
            if (IsPathClear(from, to, maze)) {
                legalPath.Add(to);
            }
            else {
                Debug.LogWarning($"Illegal move detected from {from} to {to}. Skipping.");
            }
        }

        return legalPath;
    }

    //    private List<Vector2Int> SmartBacktrackPath(
    //    Dictionary<Vector2Int, Vector2Int> cameFrom,
    //    Vector2Int currentPos,
    //    Vector2Int target,
    //    HashSet<Vector2Int> visitedSoFar
    //) {
    //        // Trace backwards from target to find first overlap with visited cells
    //        Vector2Int? meetingPoint = null;
    //        Vector2Int trace = target;
    //        List<Vector2Int> pathToTarget = new() { target };

    //        while (cameFrom.ContainsKey(trace)) {
    //            trace = cameFrom[trace];
    //            pathToTarget.Add(trace);

    //            if (visitedSoFar.Contains(trace)) {
    //                meetingPoint = trace;
    //                break;
    //            }
    //        }

    //        if (meetingPoint == null) {
    //            Debug.LogWarning($"No valid backtrack path found between {currentPos} and {target}");
    //            return new List<Vector2Int>();
    //        }

    //        // Step 1: Backtrack from currentPos to meetingPoint
    //        List<Vector2Int> pathToMeeting = new();
    //        Vector2Int temp = currentPos;
    //        while (temp != meetingPoint) {
    //            if (!cameFrom.ContainsKey(temp)) {
    //                Debug.LogWarning($"Backtrack failed: {temp} not in cameFrom while returning to meeting point.");
    //                return new List<Vector2Int>();
    //            }
    //            temp = cameFrom[temp];
    //            pathToMeeting.Add(temp);
    //        }

    //        pathToMeeting.Reverse(); // because we built it backwards

    //        // Step 2: Forward from meetingPoint to target
    //        pathToTarget.Reverse(); // now it's meetingPoint ...  target
    //        int index = pathToTarget.IndexOf(meetingPoint.Value);
    //        List<Vector2Int> forwardPath = pathToTarget.Skip(index + 1).ToList();

    //        // Step 3: Stitch full path
    //        List<Vector2Int> fullPath = new(pathToMeeting);
    //        fullPath.AddRange(forwardPath);

    //        return fullPath;
    //    }




    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end) {
        List<Vector2Int> path = new();
        Vector2Int current = end;

        while (current != start) {
            if (!cameFrom.ContainsKey(current)) {
                Debug.LogWarning($"Key not found while reconstructing path: {current}");
                return new List<Vector2Int>(); // prevent crash
            }

            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    //private List<Vector2Int> ConstructPathWithBacktracking(
    //Dictionary<Vector2Int, Vector2Int> cameFrom,
    //Vector2Int start,
    //Vector2Int currentPos,
    //Vector2Int target) {
    //    List<Vector2Int> pathToCurrent = ReconstructPath(cameFrom, start, currentPos);
    //    List<Vector2Int> pathToTarget = ReconstructPath(cameFrom, start, target);

    //    int lcaIndex = 0;
    //    while (lcaIndex < pathToCurrent.Count && lcaIndex < pathToTarget.Count &&
    //           pathToCurrent[lcaIndex] == pathToTarget[lcaIndex]) {
    //        lcaIndex++;
    //    }

    //    lcaIndex--; // step back to the last common ancestor

    //    List<Vector2Int> path = new();

    //    // Backtrack from currentPos to LCA
    //    for (int i = pathToCurrent.Count - 1; i > lcaIndex; i--) {
    //        path.Add(pathToCurrent[i]);
    //    }

    //    // Move forward from LCA to target
    //    for (int i = lcaIndex + 1; i < pathToTarget.Count; i++) {
    //        path.Add(pathToTarget[i]);
    //    }

    //    return path;
    //}
}