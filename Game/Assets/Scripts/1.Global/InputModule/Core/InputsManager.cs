using JiufenPackages.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.InputModule
{

    public class InputsManager : MonoBehaviour, IInputManager
    {
        #region ----Fields----
        public MineSweeperALikeInputs inputsBackingField;
        public MineSweeperALikeInputs inputs { get => inputsBackingField; }
        private bool isReady;
        #endregion ----Fields----

        #region ----Methods----
        public bool IsReady()
        {
            return isReady;
        }

        public void Awake()
        {
            inputsBackingField = new MineSweeperALikeInputs();
            inputs.Enable();
            isReady = true;
        }
        #endregion ----Methods----
    }

}