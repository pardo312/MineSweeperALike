using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{
    public struct InputPopupMessageData
    {
        public TMP_InputField originalInputField;
        public string labelText;
        public Action onConfirmButtonCallback;
        public bool shouldEncryptText;
    }

}