using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public bool HasLeftWall;
    public bool HasRightWall;
    public bool HasFrontWall;
    public bool HasBackWall;

    public TMP_Text DebugText;

    public bool IsVisited { get; private set; }

    public void Visit() {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall() {
        _leftWall.SetActive(false);
        HasLeftWall = false;
    }

    public void ClearRightWall() {
        _rightWall.SetActive(false);
        HasRightWall = false;
    }

    public void ClearFrontWall() {
        _frontWall.SetActive(false);
        HasFrontWall = false;
    }

    public void ClearBackWall() {
        _backWall.SetActive(false);
        HasBackWall = false;
    }

    public void UpdateDebugText(Vector2Int position) {
        DebugText.text = position.ToString();
    }

    public void HideDebugText() {
        DebugText.gameObject.SetActive(false);
    }
}