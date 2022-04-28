using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public static class GameplayDataController
    {
        #region ----Methods----

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInit()
        {
            DataManager.AddListeners(DataKeys.SAVE_GAMEPLAY_DATA, SaveGameplayData);
            DataManager.AddListeners(DataKeys.LOAD_GAMEPLAY_DATA, LoadGameplayData);

        }

        #region Save Data

        private static void SaveGameplayData(object dataToSave, Action<object> callback = null)
        {
            if (dataToSave.GetType() == typeof(int))
            {
                GameplayData gameplayData = (GameplayData)dataToSave;
                PlayerPrefs.SetInt("NumberOfBombs", gameplayData.numberOfBombs);
                PlayerPrefs.SetFloat("SizeOfSquare", gameplayData.sizeOfSquare);
                callback?.Invoke(new DataResponseModel(true, "Ok", 500));
            }
            else
            {
                callback?.Invoke(new DataResponseModel(false, "Type of data incorrect", 500));
            }
        }
        #endregion Save Data

        #region Load Data

        private static void LoadGameplayData(object payload, Action<object> response)
        {
            PlayerPrefs.SetInt("NumberOfBombs", 4);
            PlayerPrefs.SetFloat("SizeOfSquare", 1.2f);
            response?.Invoke(new GameplayData()
            {
                numberOfBombs = PlayerPrefs.GetInt("NumberOfBombs"),
                sizeOfSquare = PlayerPrefs.GetFloat("SizeOfSquare")
            });
        }

        #endregion Load Data

        #endregion ----Methods----
    }
}