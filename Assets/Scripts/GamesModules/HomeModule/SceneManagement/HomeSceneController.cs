using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using JiufenGames.MineSweeperAlike.SceneManagement;
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
    }

    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public BGHomeAnimation bGHomeAnimation;
        public TitleHomeAnimation titleHomeAnimation;
        public CustomValuesController customValuesController;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            LeanTween.init(2300);

            customValuesController.OnCustomValuesPlay += SetCustomDifficultyValues;
            titleHomeAnimation.a_onEndAnimation += CheckForSavedMatch;

            _callback?.Invoke(true);

            bGHomeAnimation.Init();
            titleHomeAnimation.Init();
        }

        public void CheckForSavedMatch()
        {
            bool gameExist = PlayerPrefs.GetInt(PersistenceConstants.SAVED_BOARD_EXIST, 0) == 1;
            if (!gameExist)
                return;

            Dictionary<PopupManager.ButtonType, Action> buttonDictionary = new Dictionary<PopupManager.ButtonType, Action>();
            buttonDictionary.Add(PopupManager.ButtonType.BACK_BUTTON, () => { PlayerPrefs.SetInt(PersistenceConstants.SAVED_BOARD_EXIST, 0); });
            buttonDictionary.Add(PopupManager.ButtonType.CONFIRM_BUTTON, () => GoToGameplay());

            ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("Do you want to load your previous game?", buttonDictionary);
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }

        public void GetDifficultyValues(int difficulty)
        {
            int rows = 1;
            int columns = 1;
            int numberOfBombs = 1;
            switch ((Difficulty)difficulty)
            {
                case Difficulty.EASY:
                    rows = 8;
                    columns = 8;
                    break;
                case Difficulty.MEDIUM:
                    rows = 16;
                    columns = 16;
                    break;
                case Difficulty.HARD:
                    rows = 30;
                    columns = 16;
                    break;
            }

            numberOfBombs = (int)((rows * columns) / 6.4f);
            SetCustomDifficultyValues(rows, columns, numberOfBombs);
        }

        public void SetCustomDifficultyValues(int rows, int columns, int numberOfBombs)
        {
            PlayerPrefs.SetInt("numRows", rows);
            PlayerPrefs.SetInt("numColumns", columns);
            PlayerPrefs.SetInt("numBombs", numberOfBombs);
        }

        public void OnDestroy()
        {
            customValuesController.OnCustomValuesPlay -= SetCustomDifficultyValues;
        }
        #endregion ----Methods----
    }

}
