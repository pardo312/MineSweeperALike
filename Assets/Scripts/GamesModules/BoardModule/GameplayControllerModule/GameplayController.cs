using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private BoardController m_boardController;
        public int m_numberOfFlags;
        public int m_flaggedMines = 0;
        public void Start()
        {
            m_boardController.Init();
            m_numberOfFlags = m_boardController.m_numberOfBombs;

            m_boardController.OnClearTileSweep -= SetUpToSweepAllClearTilesAround;
            m_boardController.OnClearTileSweep += SetUpToSweepAllClearTilesAround;

            m_boardController.OnFlag -= FlagTile;
            m_boardController.OnFlag += FlagTile;

            m_boardController.OnDeFlag -= DeflagTile;
            m_boardController.OnDeFlag  += DeflagTile;
        }

        private void DeflagTile(bool _isMine, int _row, int _column)
        {
            if (_isMine)
                m_flaggedMines--;

            if (m_numberOfFlags < m_boardController.m_numberOfBombs)
                m_numberOfFlags++;
        }

        private void FlagTile(bool _isMine, int _row, int _column)
        {
            if (_isMine)
                m_flaggedMines++;

            if (m_numberOfFlags > 0)
                m_numberOfFlags--;

            if (m_flaggedMines == m_boardController.m_numberOfBombs && m_numberOfFlags == 0)
            {
                Debug.Log("YOU WIN");
                Debug.Break();
            }
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
                    if (k == 0 && l == 0)
                        continue;
                    if (_tilesChecked.ContainsKey(new Vector2Int(_row + k, _column + l)))
                        continue;
                    if (!CheckIfInsideBoundsOfBoard(_row + k, _column + l))
                        continue;
                    MineSweeperTile currentTile = m_boardController.m_board[_row + k, _column + l];
                    if (currentTile.m_isMine)
                        continue;
                    if (currentTile.m_currentState.m_stateName.CompareTo("NormalTileState") != 0)
                        continue;

                    _tilesChecked.Add(new Vector2Int(_row + k, _column + l), true);
                    if (currentTile.m_numberOfMinesAround != 0)
                    {
                        m_boardController.m_board[_row + k, _column + l].ChangeTileData(new MineDataPayload()
                        {
                            StateToChange = "SweptTileState",
                            Sweeping = true
                        });
                    }
                    else
                    {
                        m_boardController.m_board[_row + k, _column + l].ChangeTileData(new MineDataPayload()
                        {
                            StateToChange = "SweptTileState"
                        });
                        SweepAllClearTilesAround(_row + k, _column + l, _tilesChecked);
                    }
                }
            }

        }
        private bool CheckIfInsideBoundsOfBoard(int row, int column)
        {
            if (m_boardController.m_board.GetLength(0) <= row || row < 0)
                return false;
            if (m_boardController.m_board.GetLength(1) <= column || column < 0)
                return false;

            return true;
        }
    }
}
