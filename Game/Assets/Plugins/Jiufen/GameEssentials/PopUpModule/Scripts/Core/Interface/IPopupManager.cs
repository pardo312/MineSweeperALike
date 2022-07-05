using JiufenPackages.ServiceLocator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JiufenGames.PopupModule
{
    public interface IPopupManager : IService
    {
        void ShowInfoPopup(string messageOrKey = "loading", Dictionary<ButtonType, Action> onClickButtons = null, bool isKey = false, string onFailedInfoMessageKey = "Please try again.", Sprite image = null);

        void ShowInputPopup(TMP_InputField originalInputField, string labelText, bool shouldEncrypt = false, Action onBackButtonCallback = null);

        void HidePopup(Action onComplete = null);
    }
}