using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.Gameplay.Model;
using JiufenModules.InterfaceReferenceValidator;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private BoardController m_boardController;
        [SerializeField] private FlagsController m_flagsLeftController;
        private int m_notClearedTiles = 0;
        #endregion Fields

        #region Methods
        #region Init
        public void Start()
        {
            m_boardController.Init();
            m_flagsLeftController.Init(m_boardController);
            m_notClearedTiles = m_boardController.m_numberOfTiles;

            m_boardController.a_OnNormalTileSweep -= ReduceNotClearedTiles;
            m_boardController.a_OnNormalTileSweep += ReduceNotClearedTiles;

            m_boardController.a_OnClearTileSweep -= SetUpToSweepAllClearTilesAround;
            m_boardController.a_OnClearTileSweep += SetUpToSweepAllClearTilesAround;

            m_boardController.a_OnFlag -= FlagTile;
            m_boardController.a_OnFlag += FlagTile;

            InputManager.m_Instance.a_PressedInputFlag -= (tile) => ExecuteInput("Flag", tile);
            InputManager.m_Instance.a_PressedInputFlag += (tile) => ExecuteInput("Flag", tile);

            InputManager.m_Instance.a_PressedInputSweep -= (tile) => ExecuteInput("Sweep", tile);
            InputManager.m_Instance.a_PressedInputSweep += (tile) => ExecuteInput("Sweep", tile);

            m_boardController.a_OnFlag -= FlagTile;
            m_boardController.a_OnFlag += FlagTile;

            m_boardController.a_OnDeFlag -= DeflagTile;
            m_boardController.a_OnDeFlag += DeflagTile;

            m_boardController.a_OnExplodeMine -= () => EndGame(false);
            m_boardController.a_OnExplodeMine += () => EndGame(false);
        }
        #endregion Init

        #region GameFlow
        public void ExecuteInput(string _stateToChange, MineSweeperTile _tile)
        {
            if (!String.IsNullOrEmpty(_stateToChange))
                _tile.ExecuteCurrentStateAction(_stateToChange, m_flagsLeftController.m_numberOfFlagsLeft > 0);
        }
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

        #region Sweep
        private void ReduceNotClearedTiles()
        {
            m_notClearedTiles--;
            //Also check flags
            if (m_notClearedTiles <= 0 && m_flagsLeftController.AreAllMinesFlagged(m_boardController.m_numberOfBombs))
                EndGame(true);
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
                        ReduceNotClearedTiles();
                    }
                    else
                    {
                        m_boardController.m_board[_row + k, _column + l].ChangeTileData(new MineDataPayload()
                        {
                            StateToChange = "SweptTileState"
                        });
                        ReduceNotClearedTiles();
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
            m_flagsLeftController.DeflagTile(_isMine, m_boardController.m_numberOfBombs);
        }

        private void FlagTile(bool _isMine, int _row, int _column)
        {
            m_flagsLeftController.FlagTile(_isMine);
        }
        #endregion Flagging
        #endregion Methods
    }
}
