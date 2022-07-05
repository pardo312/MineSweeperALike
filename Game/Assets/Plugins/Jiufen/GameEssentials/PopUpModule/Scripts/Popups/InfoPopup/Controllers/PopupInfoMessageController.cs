using JiufenGames.PopupModule;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{
    public class PopupInfoMessageController : MonoBehaviour, IPopupSubMessageController
    {
        #region Fields
        [Header("Animations")]
        [SerializeField, Range(.1f, 5)] private protected float timeOfAnimation = .5f;

        [Header("Text Fields")]
        [SerializeField] private protected TMP_Text PopUpMessageText;

        [Header("Buttons")]
        [SerializeField] private protected List<Button> Buttons;

        [Header("Parents")]
        [SerializeField] private protected RectTransform RectParentInfo;

        [Header("Other")]
        [SerializeField] private protected Image PopUpImage;

        public GameObject GameObjectPopup { get => this.gameObject; }
        public List<InfoPopupViewBase> infoPoupViews;
        private InfoPopupViewBase currentInfoPopupView;
        #endregion Fields

        #region Methods
        #region Init
        public void Init()
        {
            //Init all subViews
        }
        #endregion Init

        #region Set Message
        public void SetPopupMessage(object data)
        {
            if (SetSubController(data))
            {
                currentInfoPopupView.SetImage();
                currentInfoPopupView.SetText();
                currentInfoPopupView.SetButtons();
            }
            else
                Debug.Log("Popup Info data hasn't been set correctly, please check the data that you are sending in PopupManager.");
        }
        #endregion Set Message

        #region Show Popup
        public void ShowMessagePopup(Action onFinishShowing = null)
        {
            LeanTween.scale(RectParentInfo, Vector2.one, timeOfAnimation).setEase(LeanTweenType.easeOutBack).setFrom(Vector2.one * 0.2f);
        }
        #endregion Show Popup

        #region Hide Popup
        public void HideMessagePopup(Action onFinishHiding = null)
        {
            LeanTween.scale(RectParentInfo, Vector2.one * 0.2f, timeOfAnimation).setEase(LeanTweenType.easeInBack).setFrom(Vector2.one).setOnComplete(() =>
              {
                  RectParentInfo.gameObject.SetActive(false);
                  onFinishHiding?.Invoke();
              });
        }
        #endregion Hide Popup

        #region Helpers
        private bool SetSubController(object data)
        {
            if (data.GetType() != typeof(InfoPopupData))
                return false;
            InfoPopupData parsedData = (InfoPopupData)data;

            if (parsedData.isKey)
                currentInfoPopupView = infoPoupViews[0];
            else
                currentInfoPopupView = infoPoupViews[1];

            currentInfoPopupView.Init(data, PopUpMessageText, Buttons, RectParentInfo, PopUpImage);
            return true;
        }
        #endregion Helpers
        #endregion Methods
    }
}
