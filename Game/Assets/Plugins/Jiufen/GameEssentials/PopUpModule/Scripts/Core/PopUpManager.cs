using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{
    public class PopUpManager : MonoBehaviour, IPopupManager
    {
        #region ----Fields----
        [Header("Prefabs")]
        [SerializeField] private List<UnityEngine.Object> popUpSubMessageControllerList;

        public List<IPopupSubMessageController> PopUpSubMessageControllerList
        {
            get
            {
                List<IPopupSubMessageController> listeners = new List<IPopupSubMessageController>();
                popUpSubMessageControllerList.ForEach((item) => listeners.Add(item as IPopupSubMessageController));
                return listeners;
            }
        }

        [Header("References")]
        private protected IPopupSubMessageController CurrentPopUpSubMessageController;

        [Header("Parents")]
        [SerializeField] private protected RectTransform RectParentInfoBG;
        public static PopUpManager Instance;
        #endregion ----Fields----

        #region ----Methods----

        #region <<< Init >>>
        public bool IsReady()
        {
            return true;
        }
        #endregion <<< Init >>>

        #region <<< Show UI Elements >>>
        #region Info Popup
        /// <summary>
        /// Show Info Popup with the data provided.
        /// </summary>
        /// <param name="baseData">Base data of the popup</param>
        /// <param name="specificData">Specific data, get the message by code or only show text</param>
        public void ShowInfoPopup(string messageOrKey = "loading", Dictionary<ButtonType, Action> onClickButtons = null, bool isKey = false, string onFailedInfoMessage = "Please try again.", Sprite image = null)
        {
            if (hasPopupOpen)
                return;
            hasPopupOpen = true;

            ShowWidget(TypeOfPopup.INFO);

            if (onClickButtons == null)
                onClickButtons = new Dictionary<ButtonType, Action>() { { ButtonType.BACK, HidePopup } };
            else if (onClickButtons.ContainsKey(ButtonType.BACK))
            {
                Action backAction = onClickButtons[ButtonType.BACK];
                onClickButtons[ButtonType.BACK] = () => HidePopup(backAction);
            }

            CurrentPopUpSubMessageController.SetPopupMessage(new InfoPopupData()
            {
                messageOrKey = messageOrKey,
                isKey = isKey,
                onClickButtons = onClickButtons,
                onFailedInfoMessage = onFailedInfoMessage,
                image = image
            });
        }
        #endregion Info Popup

        #region Input Popup
        private bool hasPopupOpen = false;
        public void ShowInputPopup(TMP_InputField originalInputField, string labelText, bool shouldEncrypt = false, Action onBackButtonCallback = null)
        {
            if (hasPopupOpen)
                return;
            hasPopupOpen = true;

            Action onConfirmButtonCallback = onBackButtonCallback;
            onBackButtonCallback = () => HidePopup(onConfirmButtonCallback);

            ShowWidget(TypeOfPopup.INPUT);
            CurrentPopUpSubMessageController.SetPopupMessage(new InputPopupMessageData()
            {
                labelText = labelText,
                originalInputField = originalInputField,
                onConfirmButtonCallback = onBackButtonCallback,
                shouldEncryptText = shouldEncrypt
            });
        }
        #endregion Input Popup

        #region Show Helper
        private void ShowWidget(TypeOfPopup typeOfPopup)
        {
            if (CurrentPopUpSubMessageController != null)
                OnHideViewCallBack();

            RectParentInfoBG.gameObject.SetActive(true);
            CurrentPopUpSubMessageController = Instantiate(PopUpSubMessageControllerList[(int)typeOfPopup].GameObjectPopup, this.transform).GetComponent<IPopupSubMessageController>();
            CurrentPopUpSubMessageController.Init();
            CurrentPopUpSubMessageController.ShowMessagePopup();
        }
        #endregion Helper
        #endregion <<< Show UI Elements >>>

        #region <<< Hide UI >>>

        public void HidePopup()
        {
            HidePopup(null);
        }

        public void HidePopup(Action onComplete)
        {
            CurrentPopUpSubMessageController.HideMessagePopup(() => OnHideViewCallBack(onComplete));
        }

        public void OnHideViewCallBack(Action onComplete = null)
        {
            if (CurrentPopUpSubMessageController != null)
            {
                hasPopupOpen = false;
                DestroyImmediate(CurrentPopUpSubMessageController.GameObjectPopup);
                CurrentPopUpSubMessageController = null;
                RectParentInfoBG.gameObject.SetActive(false);
                onComplete?.Invoke();
            }
        }
        #endregion <<< Hide UI >>>

        #endregion ----Methods----

    }
}