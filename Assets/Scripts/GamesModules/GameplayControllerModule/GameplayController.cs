using JiufenGames.Board.Logic;
using JiufenGames.MineSweeperAlike.Board.Logic;
using JiufenGames.MineSweeperAlike.TimerModule;
using JiufenGames.PopupModule;
using JiufenPackages.ServiceLocator;
using System;
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
        [SerializeField] private FlagsController m_flagsController;
        [SerializeField] private TimerController m_timerController;
        [SerializeField] private SweepController m_sweepController;
        [SerializeField] private CameraZoomController m_cameraZoomController;
        [SerializeField] private BoardPersistenceController m_boardPersistenceController;

        private bool isReady = false;
        #endregion Fields

        #region Methods
        #region Init
        public void Init()
        {
            LeanTween.init(2300);

            bool gameExist = PlayerPrefs.GetInt(PersistenceConstants.SAVED_BOARD_EXIST, 0) == 1;

            MinesweeperPayload payload = new MinesweeperPayload()
            {
                _columns = PlayerPrefs.GetInt("numColumns", 8),
                _rows = PlayerPrefs.GetInt("numRows", 8),
                _mines = PlayerPrefs.GetInt("numBombs", 10),
                _loadingSavedMatch = gameExist,
                _squaredTiles = true
            };

            m_boardController.a_OnBoardCreated += () =>
            {
                m_cameraZoomController.Init(m_boardController.m_tileParent.GetComponent<RectTransform>());
                m_boardPersistenceController.Init(m_boardController);
                m_flagsController.Init(m_boardController);
                m_timerController.Init(0);
                m_sweepController.Init(m_boardController, m_flagsController, m_boardController.m_numberOfTiles - m_boardController.m_numberOfBombs);

                if (gameExist)
                    LoadBoard();

                SubscribeToEvents();
                isReady = true;
            };
            m_boardController.Init(payload);
        }

        public void SubscribeToEvents()
        {
            TileInputController.m_Instance.a_PressedInputFlag += (tile) => ExecuteInput(TileStatesConstants.FLAG_ACTION, tile);
            TileInputController.m_Instance.a_PressedInputSweep += (tile) => ExecuteInput(TileStatesConstants.SWEEP_ACTION, tile);

            m_sweepController.a_endGame += EndGame;
            m_sweepController.a_executeInput += ExecuteInput;

            m_boardController.a_OnNormalTileSweep += m_sweepController.ReduceNotSweepedTiles;
            m_boardController.a_OnClearTileSweep += m_sweepController.SweepClearTile;
            m_boardController.a_OnFlag += FlagTile;
            m_boardController.a_OnDeFlag += DeflagTile;
            m_boardController.a_OnExplodeMine += () => EndGame(false);
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

            _tile.ExecuteCurrentStateAction(_stateToChange, m_flagsController.m_numberOfFlagsLeft > 0);
        }

        public void ResetGame()
        {
            if (isReady && m_boardController.m_numberOfTiles != m_sweepController.m_notClearedTiles)
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

        #region Flagging
        private void DeflagTile(bool _isMine, int _row, int _column)
        {
            m_flagsController.DeflagTile(_isMine, m_boardController.m_numberOfBombs);
        }

        private void FlagTile(bool _isMine, int _row, int _column)
        {
            m_flagsController.FlagTile(_isMine);
            if (m_sweepController.m_notClearedTiles == 0 && m_flagsController.AreAllMinesFlagged(m_boardController.m_numberOfBombs))
                EndGame(true);
        }
        #endregion Flagging

        #region Persistence

        private void SaveBoard()
        {
            BoardData boardData = m_boardPersistenceController.SaveMatch();
            PlayerPrefs.SetString(PersistenceConstants.SAVED_BOARD, JsonUtility.ToJson(boardData));
            PlayerPrefs.SetInt(PersistenceConstants.SAVED_BOARD_EXIST, 1);
        }

        public bool LoadBoard()
        {
            string boardDataJson = PlayerPrefs.GetString(PersistenceConstants.SAVED_BOARD, "");
            if (string.IsNullOrEmpty(boardDataJson))
            {
                Debug.LogError("No data to load.");
                return false;
            }
            BoardData boardData = JsonUtility.FromJson<BoardData>(boardDataJson);
            m_boardPersistenceController.LoadMatch(boardData);

            PlayerPrefs.SetInt(PersistenceConstants.SAVED_BOARD_EXIST, 0);
            return true;
        }

        #endregion Persistence

        #region Unity End Methods
        //public void OnApplicationFocus(bool focus) { if (!focus) SaveBoard(); }
        public void OnApplicationPause(bool pause) { if (pause) SaveBoard(); }

        public void OnDisable()
        {
            TileInputController.m_Instance.a_PressedInputFlag -= (tile) => ExecuteInput(TileStatesConstants.FLAG_ACTION, tile);
            TileInputController.m_Instance.a_PressedInputSweep -= (tile) => ExecuteInput(TileStatesConstants.SWEEP_ACTION, tile);

            m_sweepController.a_endGame -= EndGame;
            m_sweepController.a_executeInput -= ExecuteInput;

            m_boardController.a_OnNormalTileSweep -= m_sweepController.ReduceNotSweepedTiles;
            m_boardController.a_OnClearTileSweep -= m_sweepController.SweepClearTile;
            m_boardController.a_OnFlag -= FlagTile;
            m_boardController.a_OnDeFlag -= DeflagTile;
            m_boardController.a_OnExplodeMine -= () => EndGame(false);
        }
        #endregion Unity End Methods
        #endregion Methods
    }
    public static class PersistenceConstants
    {
        public const string SAVED_BOARD = "sB";
        public const string SAVED_BOARD_EXIST = "sBE";

    }
}
