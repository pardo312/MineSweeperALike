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

    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public BordersInwardAnimationController bGHomeAnimation;
        public TitleLettersAnimationController titleHomeAnimation;
        public HomePanelsManager homePanelsController;
        public CustomPanelController customValuesController;
        public BoardDifficultyModesData boardDifficultyModes;
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
            if (difficulty < 0)
            {
                Dictionary<ButtonType, Action> buttonDictionary = new Dictionary<ButtonType, Action>();
                buttonDictionary.Add(ButtonType.BACK, () => { });
                buttonDictionary.Add(ButtonType.CONFIRM, () =>
                {
                    //Difficulty is sent but negative so we know that we have saveGame, we just have to reverse it 
                    DataManager.m_instance.ReadEvent(DataKeys.SAVE_BOARD_DATA, new BoardSaveData() { difficulty = (BoardDifficultyEnum)(-difficulty) });
                    SetPredifineDifficulty(difficulty);
                });
                ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInfoPopup("Do you want to create new game and erase old one?", buttonDictionary);
            }
            else
            {
                DataManager.m_instance.ReadEvent(DataKeys.SAVE_BOARD_DATA, new BoardSaveData() { difficulty = (BoardDifficultyEnum)(difficulty) });
                SetPredifineDifficulty(difficulty);
            }
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
                difficulty = BoardDifficultyEnum.CUSTOM,
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
