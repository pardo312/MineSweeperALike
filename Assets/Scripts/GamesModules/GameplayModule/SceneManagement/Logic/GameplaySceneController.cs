using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections;
using Timba.Games.SacredTails.PopupModule;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class GameplaySceneController : SceneController
    {
        public GameplayController gameplaController;
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            _callback?.Invoke(true);
            gameplaController.Init();
        }

        public void GoBackHome()
        {
            GameManager.m_instance.GoTo(SceneNames.HOME);
        }
    }
}