using JiufenGames.MineSweeperAlike.HomeModule;
using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class GameplayInitializable : MonoBehaviour, IInitializable
    {
        public string m_sceneName => SceneNames.GAMEPLAY;

        public void GetData(Action<DataResponseModel> callback)
        {
            DataManager.m_instance.ReadEvent(DataKeys.LOAD_GAMEPLAY_DIFFICULTY, null, (_difficulty) =>
            {
                DataManager.m_instance.ReadEvent(DataKeys.LOAD_BOARD_DATA, ((BoardDifficulty)_difficulty.data).difficulty, (_boardData) =>
                {
                    callback?.Invoke(new DataResponseModel()
                    {
                        success = true,
                        message = "Gameplay data loaded",
                        code = 200,
                        data = new
                        {
                            difficulty = _difficulty,
                            boardData = _boardData
                        }
                    });
                });
            });
        }

        public void GetTestData(Action<DataResponseModel> callback)
        {
            callback?.Invoke(new DataResponseModel()
            {
                success = true,
                message = "Test board data",
                code = 200,
                data = new BoardData()
            });
        }
    }
}