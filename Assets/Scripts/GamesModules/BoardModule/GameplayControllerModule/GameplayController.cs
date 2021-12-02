using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private BoardController m_boardController;
        private int m_numberOfFlagsLeft;
        private int m_flaggedMines = 0;
        #endregion Fields

        #region Methods
        #region Init
        public void Start()
        {
            m_boardController.Init();
            m_numberOfFlagsLeft = m_boardController.m_numberOfBombs;

            m_boardController.a_OnClearTileSweep -= SetUpToSweepAllClearTilesAround;
            m_boardController.a_OnClearTileSweep += SetUpToSweepAllClearTilesAround;

            m_boardController.a_OnFlag -= FlagTile;
            m_boardController.a_OnFlag += FlagTile;

            m_boardController.a_OnDeFlag -= DeflagTile;
            m_boardController.a_OnDeFlag += DeflagTile;

            m_boardController.a_OnExplodeMine -= () => EndGame(false);
            m_boardController.a_OnExplodeMine += () => EndGame(false);
        }
        #endregion Init

        #region GameFlow
        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }
        private void EndGame(bool won)
        {
            if (won)
                Debug.Log("YOU WIN");
            else
                Debug.Log("YOU LOSE");

            Debug.Break();
        }
        #endregion GameFlow

        #region SweepClearTile
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
                    //-----<Should we verify the tile?>------
                    //if tile = tileChecking(originaltile)
                    if (k == 0 && l == 0)
                        continue;
                    //if tile alreadyChecked
                    if (_tilesChecked.ContainsKey(new Vector2Int(_row + k, _column + l)))
                        continue;
                    //if tile outsideBounds
                    if (!CheckIfInsideBoundsOfBoard(_row + k, _column + l))
                        continue;
                    MineSweeperTile currentTile = m_boardController.m_board[_row + k, _column + l];
                    //if tile hasMine
                    if (currentTile.m_isMine)
                        continue;
                    //if tile isn't in normalState
                    if (currentTile.m_currentState.m_stateName.CompareTo("NormalTileState") != 0)
                        continue;
                    //-----</Should we verify the tile?>------

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
                    _tilesChecked.Add(new Vector2Int(_row + k, _column + l), true);
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
        #endregion SweepClearTile

        #region Flagging
        private void DeflagTile(bool _isMine, int _row, int _column)

        {
            if (_isMine)
                m_flaggedMines--;

            if (m_numberOfFlagsLeft < m_boardController.m_numberOfBombs)
                m_numberOfFlagsLeft++;
        }

        private void FlagTile(bool _isMine, int _row, int _column)
        {
            if (_isMine)
                m_flaggedMines++;

            if (m_numberOfFlagsLeft > 0)
                m_numberOfFlagsLeft--;

            if (m_flaggedMines == m_boardController.m_numberOfBombs && m_numberOfFlagsLeft == 0)
            {
                EndGame(true);
            }
        }
        #endregion Flagging
        #endregion Methods
    }
}
