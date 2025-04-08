using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeHeight;

    private MazeCell[,] _mazeGrid;

    [SerializeField]
    private float _timeToGenerate;
    private float _breakWaitTime;

    //debug only
    private float _startTime;
    private float _endTime;
    private float _visitedCounter;

    IEnumerator Start() {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeHeight];

        for (int x = 0; x < _mazeWidth; x++) {
            for (int z = 0; z < _mazeHeight; z++) {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        int totalCells = _mazeWidth * _mazeHeight;
        _visitedCounter = totalCells;
        _breakWaitTime = _timeToGenerate / totalCells;
        Debug.Log("Break Wait Time = " + _breakWaitTime);
        _startTime = Time.time;
        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell) {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(_breakWaitTime);

        MazeCell nextCell;

        do {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null) {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell) {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell) {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth) {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited) {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0) {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited) {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeHeight) {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited) {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0) {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited) {
                yield return cellToBack;
            }
        }

    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell) {
        if (previousCell == null) {
            return;
        }

        //debug
        _visitedCounter--;
        if (_visitedCounter == 1) {
            _endTime = Time.time;
            Debug.Log("Started at: " + _startTime);
            Debug.Log("Ended at: " + _endTime);
            Debug.Log("Taking " + (_endTime - _startTime));
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x) {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x) {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z) {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z) {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
