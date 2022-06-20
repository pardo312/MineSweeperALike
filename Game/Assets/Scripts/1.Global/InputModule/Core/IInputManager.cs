using JiufenPackages.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.InputModule
{
    public interface IInputManager : IService
    {
        public MineSweeperALikeInputs inputs { get; }
    }

}