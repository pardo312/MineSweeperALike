using JiufenGames.MineSweeperAlike.HomeModule;
using JiufenGames.MineSweeperAlike.SceneManagement;
using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

public class SoloMatchDataController : DataControllerBase
{
    #region <<<Init>>>
    public override void AddDataManasgerListeners()
    {
        DataManager.m_instance.AddListeners(DataKeys.SAVE_BOARD_DATA, SaveBoardData);
        DataManager.m_instance.AddListeners(DataKeys.LOAD_BOARD_DATA, LoadBoardData);

        DataManager.m_instance.AddListeners(DataKeys.SAVE_GAMEPLAY_DIFFICULTY, SaveGameplayDifficutly);
        DataManager.m_instance.AddListeners(DataKeys.LOAD_GAMEPLAY_DIFFICULTY, LoadGameplayDifficutly);

        DataManager.m_instance.AddListeners(DataKeys.CHANGE_SAVED_BOARD_STATE, ChangeSavedBoardExist);
        DataManager.m_instance.AddListeners(DataKeys.CHECK_SAVED_BOARD_STATE, CheckSavedBoardExist);
    }
    #endregion <<<Init>>>

    #region <<<Save>>>
    /// <summary>
    /// Save difficulty of solo match, dataType: int
    /// </summary>
    /// <param name="dataToSave"></param>
    /// <param name="callback"></param>
    private void SaveGameplayDifficutly(object dataToSave, Action<DataResponseModel> callback)
    {
        if (dataToSave.GetType() == typeof(BoardDifficulty))
        {
            PlayerPrefs.SetString(DataKeys.SELECTED_DIFFICULTY, JsonUtility.ToJson(dataToSave));
            SendSuccessfullCallback(callback, "Difficulty save successfull");
        }
        else
        {
            SendFailedCallback(callback, "Incorrect type of data, neede BoardDifficulty");
        }
    }

    /// <summary>
    /// Save boardDatas
    /// </summary>
    /// <param name="dataToSave">Type: BoardData</param>
    /// <param name="callback"></param>
    private void SaveBoardData(object dataToSave, Action<DataResponseModel> callback)
    {
        if (dataToSave.GetType() == typeof(BoardData))
        {
            BoardData boardData = (BoardData)dataToSave;

            if (boardData.minesPositions != null)
            {
                PlayerPrefs.SetString(DataKeys.SAVED_BOARD + (int)boardData.difficulty, JsonUtility.ToJson(boardData));
                PlayerPrefs.SetInt(DataKeys.SAVED_BOARD_EXIST + (int)boardData.difficulty, 1);
            }
            else
            {
                PlayerPrefs.SetString(DataKeys.SAVED_BOARD + (int)boardData.difficulty, "");
                PlayerPrefs.SetInt(DataKeys.SAVED_BOARD_EXIST + (int)boardData.difficulty, 0);
            }

            SendSuccessfullCallback(callback, $"Save {boardData.difficulty} board data  successfull");
        }
        else
        {
            SendFailedCallback(callback, "Incorrect type of data, neede BoardData");
        }

    }
    #endregion <<<Save>>>

    #region <<<Load>>>
    private void LoadGameplayDifficutly(object payload, Action<DataResponseModel> callback)
    {
        string difficultyJson = PlayerPrefs.GetString(DataKeys.SELECTED_DIFFICULTY, "");
        if (string.IsNullOrEmpty(difficultyJson))
        {
            SendFailedCallback(callback, "No data to load.");
            return;
        }

        BoardDifficulty difficulty = new BoardDifficulty();
        try
        {
            difficulty = JsonUtility.FromJson<BoardDifficulty>(difficultyJson);
        }
        catch (Exception e)
        {
            SendFailedCallback(callback, $"Failed to load  : {e.Message}");
            return;
        }
        SendSuccessfullCallback(callback, "Difficulty loaded", difficulty);
    }

    private void LoadBoardData(object payload, Action<DataResponseModel> callback)
    {
        string boardDataJson = PlayerPrefs.GetString(DataKeys.SAVED_BOARD + (int)payload, "");
        if (string.IsNullOrEmpty(boardDataJson))
        {
            SendFailedCallback(callback, "No data to load.");
            return;
        }
        BoardData boardData = new BoardData();
        try
        {
            boardData = JsonUtility.FromJson<BoardData>(boardDataJson);
        }
        catch (Exception e)
        {
            SendFailedCallback(callback, $"Failed to load board data: {e.Message}");
            return;
        }

        SendSuccessfullCallback(callback, "DataLoaded", boardData);

    }
    #endregion <<<Load>>>

    #region <<<Other>>>
    private void ChangeSavedBoardExist(object payload, Action<DataResponseModel> response)
    {
        if (payload.GetType() != typeof(bool))
        {
            SendFailedCallback(response, "Incorrect type of payload, must be bool");
            return;
        }

        PlayerPrefs.SetInt(DataKeys.SAVED_BOARD_EXIST, ((bool)payload) ? 1 : 0);
        SendSuccessfullCallback(response, "Saved Board changed successfully");
    }

    private void CheckSavedBoardExist(object payload, Action<DataResponseModel> response)
    {
        if (payload.GetType() != typeof(int))
        {
            SendFailedCallback(response, "Incorrect type of payload, must be int");
            return;
        }

        int boardExist = PlayerPrefs.GetInt(DataKeys.SAVED_BOARD_EXIST + (int)payload, 0);
        SendSuccessfullCallback(response, "Saved Board verifyed successfully", boardExist == 1 ? true : false);
    }
    #endregion <<<Other>>>

    #region <<<Helpers>>>
    public void SendSuccessfullCallback(Action<DataResponseModel> _callback, string _message, object _data = null)
    {
        _callback?.Invoke(new DataResponseModel()
        {
            success = true,
            message = _message,
            code = 200,
            data = _data
        });
    }
    public void SendFailedCallback(Action<DataResponseModel> _callback, string _message)
    {
        _callback?.Invoke(new DataResponseModel()
        {
            success = false,
            message = _message,
            code = 500
        });
    }
    #endregion <<<Helpers>>>
}
