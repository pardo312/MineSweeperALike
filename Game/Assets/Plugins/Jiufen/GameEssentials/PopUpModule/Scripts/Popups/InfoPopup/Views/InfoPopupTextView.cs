using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{
    public class InfoPopupTextView : InfoPopupViewBase
    {
        public override void SetText()
        {
            popUpMessageText.text = infoPopupData.messageOrKey;
        }
    }
}
