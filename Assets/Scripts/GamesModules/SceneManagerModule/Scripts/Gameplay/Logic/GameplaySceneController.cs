using JiufenPackages.SceneFlow.Logic;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class GameplaySceneController : SceneController
    {
        public override void Init(object _data, Action<bool> _callback = null)
        {
            if(_data.GetType() == typeof(GameplayData))
            {
                Debug.Log(JsonConvert.SerializeObject(_data));
            }
        }
    }
}