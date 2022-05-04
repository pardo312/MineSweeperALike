using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Timba.Games.SacredTails.PopupModule
{
    public class PopupManager : MonoBehaviour, IPopupManager
    {
        #region ----Fields----
        [SerializeField] private RectTransform infoPopupPanelWithBG;
        [SerializeField] private RectTransform infoPopupContainer;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private List<Button> buttons;

        public enum ButtonType
        {
            BACK_BUTTON = 0,
            CONFIRM_BUTTON = 1,
            RESET_BUTTON = 2
        }
        #endregion ----Fields----

        #region ----Methods----
        public bool IsReady()
        {
            return true;
        }

        public void ShowInfoPopup(string textInfo, Dictionary<ButtonType, Action> buttonsActionsPair = null)
        {
            infoPopupContainer.localScale = Vector2.one * .2f;
            LeanTween.scale(infoPopupContainer, Vector2.one * 1f, .5f).setEase(LeanTweenType.easeOutBack);
            infoText.text = textInfo;

            //Set special cases
            if (buttonsActionsPair == null)
            {
                buttonsActionsPair = new Dictionary<ButtonType, Action>();
                buttonsActionsPair.Add(ButtonType.BACK_BUTTON, null);
            }
            if (buttonsActionsPair.ContainsKey(ButtonType.BACK_BUTTON))
                buttonsActionsPair[ButtonType.BACK_BUTTON] += HideInfoPopup;

            foreach (var buttonActionPair in buttonsActionsPair)
            {
                buttons[(int)buttonActionPair.Key].onClick.RemoveAllListeners();
                buttons[(int)buttonActionPair.Key].gameObject.SetActive(true);
                buttons[(int)buttonActionPair.Key].onClick.AddListener(() => buttonActionPair.Value?.Invoke());
            }

            infoPopupPanelWithBG.gameObject.SetActive(true);
        }

        public void HideInfoPopup()
        {
            LeanTween.scale(infoPopupContainer, Vector2.one * .1f, .5f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                buttons.ForEach((buttons) => buttons.gameObject.SetActive(false));
                infoPopupPanelWithBG.gameObject.SetActive(false);
            });
        }
        #endregion ----Methods----

    }
}