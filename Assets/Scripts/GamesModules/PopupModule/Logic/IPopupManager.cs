using JiufenPackages.ServiceLocator;
using System;
using System.Collections.Generic;

namespace JiufenGames.PopupModule
{
    public interface IPopupManager : IService
    {
        void ShowInfoPopup(string textInfo, Dictionary<PopupManager.ButtonType, Action> buttonsActionsPair = null);

        void HideInfoPopup();
    }
}