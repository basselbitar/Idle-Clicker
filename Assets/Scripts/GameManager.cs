using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerStats playerStats;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Load(playerStats);
        ResourceUIManager.Instance.RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoundEnd(bool IsMazeSolved, Vector2Int _startPosition, Vector2Int _goalPosition, MazeCell[,] _mazeGrid) {
        if (IsMazeSolved) {
            StartCoroutine(ExperienceManager.Instance.GiveEndOfRoundReward(_startPosition, _goalPosition, _mazeGrid));
        }
    }
}
