using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    [CreateAssetMenu(fileName = "BoardDifficultyModes", menuName = "MinesweeperALike/BoardDifficultyModes", order = 1)]
    public class BoardDifficultyModes : ScriptableObject
    {
        public bool calculateBombs;
        public BoardDifficulty[] difficulties;

        private void OnValidate()
        {
            if (!calculateBombs)
                return;

            foreach (var difficulty in difficulties)
                difficulty.mines = Mathf.CeilToInt((difficulty.rows * difficulty.rows) / 6.4f);
            calculateBombs = false;
        }
    }

    [Serializable]
    public class BoardDifficulty
    {
        public Difficulty difficulty;
        public int rows;
        public int columns;
        public int mines;

    }
}
