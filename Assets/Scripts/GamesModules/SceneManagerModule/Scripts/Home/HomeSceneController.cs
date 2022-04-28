using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class HomeSceneController : SceneController
    {
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            _callback?.Invoke(true);
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }
    }
}