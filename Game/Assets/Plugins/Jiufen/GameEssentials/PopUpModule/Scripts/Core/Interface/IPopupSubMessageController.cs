using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{
    public interface IPopupSubMessageController
    {
        GameObject GameObjectPopup { get; }
        void Init();
        void HideMessagePopup(Action onFinishHiding = null);
        void ShowMessagePopup(Action onFinishShowing = null);
        void SetPopupMessage(object data);
    }

}