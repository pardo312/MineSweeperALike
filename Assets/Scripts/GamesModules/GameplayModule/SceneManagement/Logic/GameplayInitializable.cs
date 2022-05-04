using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class GameplayInitializable : MonoBehaviour, IInitializable
    {
        public string m_sceneName => SceneNames.GAMEPLAY;

        public void GetData(Action<object> callback)
        {
            DataManager.ReadEvent(DataKeys.LOAD_GAMEPLAY_DATA, null, (_data) =>
            {
                callback?.Invoke(_data);
            });
        }

        public void GetTestData(Action<object> callback)
        {
            callback?.Invoke(new GameplayData()
            {
                numberOfBombs = 4,
                sizeOfSquare = 1.2f
            });
        }
    }
}