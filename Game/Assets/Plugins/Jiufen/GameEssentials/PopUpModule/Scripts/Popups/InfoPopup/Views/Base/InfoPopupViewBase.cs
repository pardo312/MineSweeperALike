using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{

    public abstract class InfoPopupViewBase : MonoBehaviour
    {
        private protected TMP_Text popUpMessageText;
        private protected List<Button> buttons;
        private protected RectTransform rectParentInfo;
        private protected Image popUpImage;

        private protected InfoPopupData infoPopupData;

        public virtual void Init(object _data, TMP_Text _popupMessageText, List<Button> _buttons, RectTransform _rectParent, Image _popUpImage)
        {
            popUpMessageText = _popupMessageText;
            buttons = _buttons;
            rectParentInfo = _rectParent;
            popUpImage = _popUpImage;
            infoPopupData = (InfoPopupData)_data;
        }

        public abstract void SetText();
        public virtual void SetButtons(Action onActivateButton = null)
        {
            foreach (Button button in buttons)
                button.gameObject.SetActive(false);

            if (infoPopupData.onClickButtons != null)
            {
                foreach (KeyValuePair<ButtonType, Action> buttonPair in infoPopupData.onClickButtons)
                {
                    int buttonIndex = (int)buttonPair.Key;
                    if (buttons.Count < buttonIndex)
                    {
                        Debug.Log("<color=red>PopupManagerError: Button not found</color>");
                        continue;
                    }

                    buttons[buttonIndex].onClick.RemoveAllListeners();
                    buttons[buttonIndex].onClick.AddListener(() => buttonPair.Value?.Invoke());
                    buttons[buttonIndex].gameObject.SetActive(true);
                    onActivateButton?.Invoke();
                }
            }
        }

        public virtual void SetImage()
        {
            if (infoPopupData.image == null)
            {
                popUpImage.gameObject.SetActive(false);
                return;
            }

            popUpImage.gameObject.SetActive(true);
            popUpImage.sprite = infoPopupData.image;
        }
    }
}
