using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JiufenGames.PopupModule
{

    public class PopupInputMessageController : MonoBehaviour, IPopupSubMessageController
    {
        #region Fields
        public GameObject GameObjectPopup => this.gameObject;
        [Header("Parents & Positions")]
        [SerializeField] private protected RectTransform RectParentInput;
        [SerializeField] private protected RectTransform TargetAnimationShow;
        private protected Vector2 TargetAnimationHide;

        [Header("Fields")]
        [SerializeField] private TMP_Text InputPlaceHolder;
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private Toggle ViewEncryptedTextToggle;

        private InputPopupMessageData InputPopupMessageData;
        private string InputValue;
        private bool IsHiding = false;
        #endregion Fields

        #region Methods
        public void Init()
        {
            TargetAnimationHide = transform.localPosition;
            InputField.onValueChanged.AddListener((value) =>
            {
                if (String.IsNullOrEmpty(value))
                {
                    InputPlaceHolder.gameObject.SetActive(true);
                }
                else
                {
                    InputPlaceHolder.gameObject.SetActive(false);
                }

                InputValue = value;
            });
            InputField.onDeselect.AddListener((idk) =>
            {
                StartCoroutine(WaitForEndFrame(() =>
                {
                    if (!IsHiding)
                    {
                        if (EventSystem.current.currentSelectedGameObject != ViewEncryptedTextToggle.gameObject)
                        {
                            IsHiding = true;
                            PopUpManager.Instance.HidePopup();
                            InputPopupMessageData.onConfirmButtonCallback?.Invoke();
                        }
                        else
                        {
                            InputField.Select();
                            InputField.ActivateInputField();
                        }
                    }
                }));
            });
        }
        IEnumerator WaitForEndFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }

        public void HideMessagePopup(Action onFinishHiding = null)
        {
            float animationTime = 0.5f;
            LeanTween.scale(RectParentInput, Vector2.one * 0.6f, animationTime).setEase(LeanTweenType.easeInBack).setFrom(Vector2.one);
            LeanTween.moveLocal(RectParentInput.gameObject, TargetAnimationHide, animationTime).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
              {
                  onFinishHiding?.Invoke();
              }
            );

            InputPopupMessageData.originalInputField.text = InputValue;
        }

        public void ShowMessagePopup(Action onFinishShowing = null)
        {
            InputField.Select();
            InputField.ActivateInputField();

            float animationTime = 0.5f;
            LeanTween.scale(RectParentInput, Vector2.one, animationTime).setEase(LeanTweenType.easeOutBack).setFrom(Vector2.one * 0.6f);
            LeanTween.moveLocal(RectParentInput.gameObject, TargetAnimationShow.localPosition, animationTime).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
              {
                  onFinishShowing?.Invoke();
              }
            );
        }

        public void SetPopupMessage(object data)
        {
            SetPopupMessageData(data);
            InputField.text = InputPopupMessageData.originalInputField.text;
            InputPlaceHolder.text = InputPopupMessageData.labelText;
            InputField.contentType = InputPopupMessageData.originalInputField.contentType;
                ViewEncryptedTextToggle.gameObject.SetActive(false);
            if (InputPopupMessageData.shouldEncryptText)
            {
                //ViewEncryptedTextToggle.gameObject.SetActive(true);
                //ViewEncryptedTextToggle.onValueChanged.AddListener((isOn) =>
                //{
                //    IsHiding = true;
                //    if (!isOn)
                //    {
                //        InputField.contentType = TMP_InputField.ContentType.Password;
                //    }
                //    else
                //    {
                //        InputField.contentType = TMP_InputField.ContentType.Standard;
                //    }
                //    InputField.ForceLabelUpdate();
                //    IsHiding = false;
                //});
            }
            else
            {
                ViewEncryptedTextToggle.gameObject.SetActive(false);
            }
        }

        private bool SetPopupMessageData(object data)
        {
            if (data.GetType() == typeof(InputPopupMessageData))
            {
                this.InputPopupMessageData = ((InputPopupMessageData)data);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion Methods
    }

}