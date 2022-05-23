using System;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{
    public abstract class SceneController : MonoBehaviour
    {
        #region Singleton
        public static SceneController Instance;
        protected virtual void Awake() { Instance = this; }
        #endregion Singleton

        #region Methods
        /// <summary>
        /// Initialize the scene with the passed data.
        /// </summary>
        /// <param name="_data">Data of the scene.</param>
        /// <param name="_callback">Callback use to hide the loading scene</param>
        public abstract void Init(DataResponseModel _data, Action<bool> _callback = null);
        #endregion Methods
    }
}