using JiufenGames.MineSweeperAlike.Board.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private BoardController m_boardController;
        public void Start()
        {
            m_boardController.Init();

            m_boardController.OnClearTileSweep -= SetUpToSweepAllClearTilesAround;
            m_boardController.OnClearTileSweep += SetUpToSweepAllClearTilesAround;
        }

        private void SetUpToSweepAllClearTilesAround(int _row, int _column)
        {
            Dictionary<Vector2Int, bool> tilesChecked = new Dictionary<Vector2Int, bool>();
            SweepAllClearTilesAround(_row, _column, tilesChecked);
        }
        
        private void SweepAllClearTilesAround(int _row, int _column, Dictionary<Vector2Int, bool> _tilesChecked)
        {
            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {

                    if (_tilesChecked.ContainsKey(new Vector2Int(_row + k, _column + l)))
                        continue;
                    if (!CheckIfInsideBoundsOfBoard(_row + k, _column+l))
                        continue;
                    MineSweeperTile currentTile = m_boardController.m_board[_row + k, _column + l];
                    if (currentTile.m_currentState.m_stateName.CompareTo("NormalTileState") != 0)
                        continue;

                    _tilesChecked.Add(new Vector2Int(_row + k, _column + l), true);
                    if (currentTile.m_numberOfMinesAround != 0)
                    {
                        m_boardController.m_board[_row + k, _column + l].ChangeTileData(new object[2] { "SweptTileState", true });
                    }
                    else
                    {
                        m_boardController.m_board[_row + k, _column + l].ChangeTileData(new object[1] { "SweptTileState" });
                        SweepAllClearTilesAround(_row + k, _column + l, _tilesChecked);
                    }
                }
            }

        }
        private bool CheckIfInsideBoundsOfBoard(int row, int column)
        {
            if (row == 0 && column == 0)
                return false;
            if (m_boardController.m_board.GetLength(0) <= row || row <= 0)
                return false;
            if (m_boardController.m_board.GetLength(1) <= column || column <= 0)
                return false;

            return true;
        }
    }
}
