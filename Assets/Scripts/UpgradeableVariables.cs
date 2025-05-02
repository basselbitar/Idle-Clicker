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

        // Mouse variables
        public static float MouseSpeed = 1f;
        public static int MouseIntelligence = 1;
        public static float MouseStepCost = 5f;
        public static int MouseMaxNumberOfMoves = 50;
    }
}
