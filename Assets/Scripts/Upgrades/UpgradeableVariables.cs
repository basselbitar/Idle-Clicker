using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities {

    public static class UpgradeableVariables {
        // Maze variables
        public static float GenerationTime = 6;
        public static int MaxMapWidth = 2;
        public static int MaxMapHeight = 2;
        public static int MaxMouseTrapsPerMaze = 0;
        public static int MaxWaterPitsPerMaze = 0;

        // Gold spawnage
        public static int BaseGoldSpawnCount = 1;
        public static int ExtraGoldSpawnCount = 0;
        public static float BaseGoldSpawnProbabilityPerCell = 0.00f;  //0 to 1
        public static float ExtraGoldSpawnProbabilityPerCell = 0.01f;  //0 to 1

        // Mouse variables
        public static float MouseSpeed = 1f;
        public static int MouseIntelligence = 1;
        public static float MouseStepCost = 5f;
        public static int MouseMaxNumberOfMoves = 50;
    }
}
