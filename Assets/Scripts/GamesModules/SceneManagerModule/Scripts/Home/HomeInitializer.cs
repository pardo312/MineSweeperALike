using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class HomeInitializer : MonoBehaviour, IInitializable
    {
        public string m_sceneName => SceneNames.HOME;

        public void GetData(Action<object> callback)
        {
            callback?.Invoke(null);
        }

        public void GetTestData(Action<object> callback)
        {
            callback?.Invoke(null);
        }

    }
}
