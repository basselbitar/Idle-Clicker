using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UV = Utilities.UpgradeableVariables;

public class GoldSpawner : MonoBehaviour
{
    public GameObject goldCoinPrefab;
    public int baseSpawnCount;
    public float spawnChancePerCell = 0.1f; // Base Chance for each cell to spawn a coin

    [Header("Upgrade Values")]
    public int spawnBonus = 0; // Bonus number of coins from upgrades
    public float chanceBonus = 0f; // Additional chance from upgrades

    private MazeCell[,] maze;

    public void SpawnCoins(MazeCell[,] mazeGrid, MazeCell start, MazeCell end) {
        maze = mazeGrid;
        List<MazeCell> allCells = new();
        baseSpawnCount = 1; //TODO: calculate based on width and height somehow

        foreach (var cell in maze) {
            if (!cell.HasCoin && cell != start && cell != end)
                allCells.Add(cell);
        }
        Debug.Log("all cells count is " +  allCells.Count);
        // Shuffle the list to get random positions
        allCells = allCells.OrderBy(x => Random.value).ToList();


        GameObject CoinsGO = GameObject.Find("CoinsGO");
        if (CoinsGO != null) {
            Destroy(CoinsGO);
        }
        CoinsGO = new GameObject("CoinsGO");


        // 1. Spawn guaranteed coins
        int guaranteedCount = baseSpawnCount + spawnBonus;
        for (int i = 0; i < Mathf.Min(guaranteedCount, allCells.Count); i++) {
            SpawnCoinInCell(allCells[i], CoinsGO);
        }

        // 2. Spawn chance-based bonus coins
        for (int i = guaranteedCount; i < allCells.Count; i++) {
            float chance = spawnChancePerCell + chanceBonus;
            if (Random.value < chance) {
                SpawnCoinInCell(allCells[i], CoinsGO);
            }
        }




        //Debug purposes
        int coinsCount = 0;
        foreach (var cell in maze) {
            if(cell.HasCoin) {
                coinsCount++;
            }
        }
        Debug.Log("Total coins spawned: " +  coinsCount);
        if(CoinSpawnerTester.Instance) {
        CoinSpawnerTester.Instance.AddALine($"{UV.MaxMapWidth},{UV.MaxMapHeight},{allCells.Count},{baseSpawnCount},{spawnBonus},{spawnChancePerCell}," +
            $"{chanceBonus},{coinsCount}");
        }
    }

    private void SpawnCoinInCell(MazeCell cell, GameObject parent) {
        Vector3 spawnPosition = cell.transform.position + Vector3.up * 0.5f;
        GameObject coin = Instantiate(goldCoinPrefab, spawnPosition, Quaternion.identity);
        coin.transform.eulerAngles = new Vector3(90f, 0f, 0);
        coin.transform.SetParent(parent.transform);
        cell.HasCoin = true;
    }
}
