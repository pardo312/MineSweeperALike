using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using Timba.Games.SacredTails.PopupModule;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class GameplayController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private BoardController m_boardController;
        [SerializeField] private FlagsController m_flagsLeftController;
        [SerializeField] private CameraZoomController m_cameraZoomController;
        [SerializeField] private int m_notClearedTiles = 0;

        [SerializeField, Range(.01f, .1f)] private float m_sweepClearTileAnimTime = .05f;
        private bool isReady = false;
        #endregion Fields

        #region Methods
        #region Init
        public void Init()
        {
            LeanTween.init(2300);
            m_boardController.a_OnBoardCreated += () =>
            {
                m_flagsLeftController.Init(m_boardController);
                m_notClearedTiles = m_boardController.m_numberOfTiles - m_boardController.m_numberOfBombs;

                m_boardController.a_OnNormalTileSweep -= ReduceNotSweepedTiles;
                m_boardController.a_OnNormalTileSweep += ReduceNotSweepedTiles;

                m_boardController.a_OnClearTileSweep -= SweepClearTile;
                m_boardController.a_OnClearTileSweep += SweepClearTile;

                m_boardController.a_OnFlag -= FlagTile;
                m_boardController.a_OnFlag += FlagTile;

                InputManager.m_Instance.a_PressedInputFlag -= (tile) => ExecuteInput(TileStatesConstants.FLAG_ACTION, tile);
                InputManager.m_Instance.a_PressedInputFlag += (tile) => ExecuteInput(TileStatesConstants.FLAG_ACTION, tile);

                InputManager.m_Instance.a_PressedInputSweep -= (tile) => ExecuteInput(TileStatesConstants.SWEEP_ACTION, tile);
                InputManager.m_Instance.a_PressedInputSweep += (tile) => ExecuteInput(TileStatesConstants.SWEEP_ACTION, tile);

                m_boardController.a_OnFlag -= FlagTile;
                m_boardController.a_OnFlag += FlagTile;

                m_boardController.a_OnDeFlag -= DeflagTile;
                m_boardController.a_OnDeFlag += DeflagTile;

                m_boardController.a_OnExplodeMine -= () => EndGame(false);
                m_boardController.a_OnExplodeMine += () => EndGame(false);

                m_cameraZoomController.Init(m_boardController.m_tileParent.GetComponent<RectTransform>());
                isReady = true;
            };
            m_boardController.Init();
        }
        #endregion Init

        #region GameFlow
        public void ExecuteInput(string _stateToChange, MineSweeperTile _tile)
        {
            if (m_cameraZoomController.isZooming)
                return;
            if (m_cameraZoomController.isMoving)
                return;
            if (String.IsNullOrEmpty(_stateToChange))
                return;

            _tile.ExecuteCurrentStateAction(_stateToChange, m_flagsLeftController.m_numberOfFlagsLeft > 0);
        }

        public void ResetGame()
        {
            if (isReady && m_boardController.m_numberOfTiles != m_notClearedTiles)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }

        private bool gameEnded = false;
        private void EndGame(bool won)
        {
            if (gameEnded)
                return;
            gameEnded = true;

            // Add reset button to popup
            Dictionary<PopupManager.ButtonType, Action> buttonDictionary = new Dictionary<PopupManager.ButtonType, Action>();
            buttonDictionary.Add(PopupManager.ButtonType.RESET_BUTTON, () =>
            {
                ServiceLocator.m_Instance.GetService<IPopupManager>().HideInfoPopup();
                ResetGame();
            });

            //Set result text.
            if (won)
                ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("You Win!", buttonDictionary);
            else
            {
                m_boardController.minesPositions.ForEach((mine) => ExecuteInput("Sweep", mine));
                ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("You Lose!", buttonDictionary);
            }
        }
        #endregion GameFlow

        #region Sweep
        private void ReduceNotSweepedTiles()
        {
            m_notClearedTiles--;
            //All tiles swept, no matter the flags
            if (m_notClearedTiles == 0)
            {
                m_boardController.minesPositions.ForEach((mine) =>
                {
                    if (mine.m_currentState.m_stateName.CompareTo(TileStatesConstants.FLAGGED_TILE_STATE) != 0)
                        ExecuteInput("Flag", mine);
                });
                EndGame(true);
            }
        }

        private void SweepClearTile(int _row, int _column)
        {
            ReduceNotSweepedTiles();
            StartCoroutine(SweepAllClearTilesAround(_row, _column));
        }

        private IEnumerator SweepAllClearTilesAround(int _row, int _column)
        {
            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {
                    //if tile = tileChecking(originaltile)
                    if (k == 0 && l == 0)
                        continue;
                    //if tile outsideBounds
                    if (!CheckIfInsideBoundsOfBoard(_row + k, _column + l))
                        continue;
                    MineSweeperTile currentTile = m_boardController.m_board[_row + k, _column + l];
                    //if tile hasMine
                    if (currentTile.m_isMine)
                        continue;
                    //if tile isn't in normalState
                    if (currentTile.m_currentState.m_stateName.CompareTo(TileStatesConstants.NORMAL_TILE_STATE) != 0)
                        continue;
                    if (currentTile.m_isSwiping)
                        continue;

                    currentTile.m_isSwiping = true;

                    //Sweep tile and reduce the not sweeped tiles
                    yield return new WaitForSeconds(m_sweepClearTileAnimTime);
                    currentTile.ExecuteCurrentStateAction("Sweep", m_flagsLeftController.m_numberOfFlagsLeft > 0);

                    currentTile.m_isSwiping = false;
                }
            }
            yield return null;
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
            if (m_notClearedTiles == 0 && m_flagsLeftController.AreAllMinesFlagged(m_boardController.m_numberOfBombs))
                EndGame(true);
        }
        #endregion Flagging
        #endregion Methods
    }
}
