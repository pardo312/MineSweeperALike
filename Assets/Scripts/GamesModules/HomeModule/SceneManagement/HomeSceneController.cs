using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using JiufenGames.MineSweeperAlike.SceneManagement;
using JiufenGames.MineSweeperAlike.UIHelpers;
using JiufenGames.PopupModule;
using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    public enum Difficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2,
        CUSTOM = 3,
    }

    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public BGHomeAnimation bGHomeAnimation;
        public TitleHomeAnimation titleHomeAnimation;
        public HomePanelsController homePanelsController;
        public CustomValuesController customValuesController;
        public BoardDifficultyModes boardDifficultyModes;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init(DataResponseModel _data, Action<bool> _callback = null)
        {
            LeanTween.init(2300);

            customValuesController.OnCustomValuesPlay += SetCustomDifficultyValues;

            _callback?.Invoke(true);

            bGHomeAnimation.Init();
            titleHomeAnimation.Init();
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }

        public void CleanBoardDataForDifficutly(int difficulty)
        {
            Dictionary<PopupManager.ButtonType, Action> buttonDictionary = new Dictionary<PopupManager.ButtonType, Action>();
            buttonDictionary.Add(PopupManager.ButtonType.BACK_BUTTON, () => { });
            buttonDictionary.Add(PopupManager.ButtonType.CONFIRM_BUTTON, () =>
            {
                DataManager.m_instance.ReadEvent(DataKeys.SAVE_BOARD_DATA, new BoardData() { difficulty = (Difficulty)difficulty });
                SetPredifineDifficulty(difficulty);
            });
            ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("Do you want to create new game and erase old one?", buttonDictionary);
        }

        public void SetPredifineDifficulty(int difficulty)
        {
            if (difficulty < boardDifficultyModes.difficulties.Length)
            {
                DataManager.m_instance.ReadEvent(DataKeys.SAVE_GAMEPLAY_DIFFICULTY, boardDifficultyModes.difficulties[difficulty]);
                GoToGameplay();
            }
            else
                Debug.LogError($"Predifine difficulty {difficulty} doesn't exist");
        }

        public void SetCustomDifficultyValues(int rows, int columns, int numberOfBombs)
        {
            DataManager.m_instance.ReadEvent(DataKeys.SAVE_GAMEPLAY_DIFFICULTY, new BoardDifficulty()
            {
                difficulty = Difficulty.CUSTOM,
                rows = rows,
                columns = columns,
                mines = numberOfBombs
            });
            GoToGameplay();
        }

        public void OnDestroy()
        {
            customValuesController.OnCustomValuesPlay -= SetCustomDifficultyValues;
        }
        #endregion ----Methods----
    }


}
