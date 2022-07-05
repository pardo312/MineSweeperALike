using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{

    public class InfoPopupCodeView : InfoPopupViewBase
    {
        [SerializeField] private LobbyMultiplayerInfo_scrObj lobbyInfoScriptable;
        private Dictionary<string, PopUpInfoModel> popUpInfoModelDictionary;
        private PopUpInfoModel currentInfoModel;

        [HideInInspector] public int SecondsToWaitBeforeShowingBackButton = 15;
        private Coroutine enableBackButtonCorutine = null;

        public void Awake()
        {
            popUpInfoModelDictionary = new Dictionary<string, PopUpInfoModel>();

            //Init Dictionary of infoLobby
            foreach (PopUpInfoModel lobbyInfo in lobbyInfoScriptable.lobbyMultiplayerMessage)
                if (!popUpInfoModelDictionary.ContainsKey(lobbyInfo.key))
                    popUpInfoModelDictionary.Add(lobbyInfo.key, lobbyInfo);
        }
        public override void Init(object _data, TMP_Text _popupMessageText, List<Button> _buttons, RectTransform _rectParent, Image _popUpImage)
        {
            base.Init(_data, _popupMessageText, _buttons, _rectParent, _popUpImage);
            currentInfoModel = popUpInfoModelDictionary[infoPopupData.messageOrKey];
        }

        public override void SetText()
        {
            popUpMessageText.text = "(ErrorID: " + currentInfoModel.code.ToString() + ") ";
            popUpMessageText.text += currentInfoModel.message;
        }

        public override void SetButtons(Action onActivateButton = null)
        {
            //Stop Previours Coroutines
            if (enableBackButtonCorutine != null)
            {
                StopCoroutine(enableBackButtonCorutine);
                enableBackButtonCorutine = null;
            }

            base.SetButtons(() =>
            {
                if (infoPopupData.messageOrKey.Equals("loading") && buttons.Count > 0)
                    enableBackButtonCorutine = StartCoroutine(EnableBackButtonAfterWaitingTooLong(0, infoPopupData.onFailedInfoMessage, infoPopupData.onClickButtons[0]));
            });
        }

        private IEnumerator EnableBackButtonAfterWaitingTooLong(int buttonListIndex, string OnFailedInfoMessage, Action onBackButtonCallback = null)
        {
            yield return new WaitForSecondsRealtime(SecondsToWaitBeforeShowingBackButton);
            popUpMessageText.text = OnFailedInfoMessage;

            buttons[buttonListIndex].gameObject.SetActive(true);
            onBackButtonCallback?.Invoke();
        }

    }
}
