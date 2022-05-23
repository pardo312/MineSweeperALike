using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class HomeInitializer : MonoBehaviour, IInitializable
    {
        public string m_sceneName => SceneNames.HOME;

        public void GetData(Action<DataResponseModel> callback)
        {
            callback?.Invoke(new DataResponseModel()
            {
                success = true,
                message = "Test",
                code = 200
            });
        }

        public void GetTestData(Action<DataResponseModel> callback)
        {
            callback?.Invoke(new DataResponseModel()
            {
                success = true,
                message = "Test",
                code = 200
            });
        }

    }
}
