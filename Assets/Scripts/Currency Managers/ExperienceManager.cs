using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {
    public static ExperienceManager Instance { get; private set; }

    public int experiencePerTile = 10;

    public PlayerStats playerStats;
    public Sprite experienceIcon;

    private Vector2Int _startCell;
    private Vector2Int _endCell;

    private readonly float _animationDuration = 0.5f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public IEnumerator GiveEndOfRoundReward(Vector2Int startPos, Vector2Int endPos, MazeCell[,] maze) {

        MazeSolver mazeSolver = new();
        List<Vector2Int> path = mazeSolver.CalculatePath(startPos, endPos, maze);
        float waitTime = _animationDuration / path.Count;

        foreach (Vector2Int cellPos in path) {
            Vector3 worldPos = maze[cellPos.x, cellPos.y].transform.position;
            // worldPos.y = transform.position.y; // keep mouse on same Y level
            AddExperience(experiencePerTile);

            // Visually show the + n Experience Symbol on screen
            // Show floating popup
            FloatingPopupManager.Instance.ShowPopup($"+{experiencePerTile}", worldPos, FloatingPopupManager.PopupType.XP);
            yield return new WaitForSeconds(waitTime);
        }

    }

    public void AddExperience(float amount) {
        playerStats.AddExperience(amount);
        SaveManager.Save(playerStats);
    }

    public void ConsumeExperience(float amount) {
        playerStats.ConsumeEnergy(amount);
        SaveManager.Save(playerStats);
    }

    public bool HasEnergy(float amount) {
        return playerStats.energy >= amount;
    }
}
