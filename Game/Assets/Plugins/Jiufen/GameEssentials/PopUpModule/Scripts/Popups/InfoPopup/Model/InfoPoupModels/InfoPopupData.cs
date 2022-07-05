using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.PopupModule
{
    public class InfoPopupData
    {
        public string messageOrKey;
        public bool isKey;

        public Dictionary<ButtonType, Action> onClickButtons;
        public string onFailedInfoMessage;
        public Sprite image;
    }

}