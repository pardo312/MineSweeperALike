using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public enum TypeOfOptionPanelAnimation
    {
        OPEN,
        CLOSE
    }
    public class SelectPanelController : MonoBehaviour
    {
        #region ----Fields----
        private bool isAnimating;

        [SerializeField] private ScrollRect scrollRect;
        private RectTransform rootLayoutParent;
        private int currentOpenedButton;
        [Header("Parameters")]
        [SerializeField] private Color[] panelTitleColors;
        [SerializeField] private float animationTime = .25f;

        [Header("References")]
        [SerializeField] private SelectOptionPanelDto[] panels;

        #endregion ----Fields----

        #region ----Methods----
        #region Init class
        public void Awake()
        {
            currentOpenedButton = -1;
            rootLayoutParent = this.GetComponent<RectTransform>();
        }
        #endregion Init class

        #region Init
        public void ClickButtonAnimation(int buttonIndex)
        {
            if (isAnimating)
                return;
            isAnimating = true;

            if (currentOpenedButton >= 0 && currentOpenedButton != buttonIndex)
                ClosePreviousPanelAnimation(buttonIndex);
            else
                CheckTypeOfAnimation(currentOpenedButton, buttonIndex);
        }

        public void ClosePreviousPanelAnimation(int buttonIndex)
        {
            int currentOpenButtonNow = currentOpenedButton;
            PanelAnimation(currentOpenedButton,
                           TypeOfOptionPanelAnimation.CLOSE,
                           () => CheckTypeOfAnimation(currentOpenButtonNow, buttonIndex));
        }

        public void CheckTypeOfAnimation(int currentOpenButtonNow, int buttonIndex)
        {
            if (currentOpenButtonNow == buttonIndex)
                PanelAnimation(buttonIndex, TypeOfOptionPanelAnimation.CLOSE);
            else
                PanelAnimation(buttonIndex, TypeOfOptionPanelAnimation.OPEN);
        }
        #endregion <<<Set anim>>>

        #region <<<Execute anim>>>
        public void PanelAnimation(int buttonIndex, TypeOfOptionPanelAnimation typeOfAnim, Action onComplete = null)
        {
            bool isOpen = typeOfAnim == TypeOfOptionPanelAnimation.OPEN;

            SetInitOfAnimation(isOpen, buttonIndex, () =>
            {
                int targetPosition = isOpen ? panels[buttonIndex].middlePanelSizeY : 0;
                LeanTween.value(panels[buttonIndex].middlePanelSizeY - targetPosition, targetPosition, animationTime)
                .setOnUpdate((float value) =>
                {
                    var temp = panels[buttonIndex].panelMask.sizeDelta;
                    temp.y = value;
                    panels[buttonIndex].panelMask.sizeDelta = temp;

                    LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayoutParent);
                })
                .setOnComplete(() =>
                {
                    if (!isOpen)
                    {
                        panels[buttonIndex].panelTransform.gameObject.SetActive(!isOpen);
                        rootLayoutParent.offsetMin = new Vector2(rootLayoutParent.offsetMin.x, 0);
                        currentOpenedButton = -1;
                        LeanTween.value(scrollRect.normalizedPosition.y, 1, (1 - scrollRect.normalizedPosition.y)*.5f)
                                 .setOnUpdate((float value) => scrollRect.normalizedPosition = new Vector2(0, value))
                                 .setOnComplete(() => { onComplete?.Invoke(); isAnimating = false; });
                    }
                    else
                    {
                        currentOpenedButton = buttonIndex;
                        isAnimating = false;
                        onComplete?.Invoke();
                    }
                });
            });
        }

        private const float PANEL_NORMAL_SIZE = -875f;
        private const float SCROLL_CHILD_FACTOR = .35f;
        /// <summary>
        /// Set the initial positions and values for the ClickButtonAnimation
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="buttonIndex"></param>
        /// <param name="onCompleteInitAnimation"></param>
        public void SetInitOfAnimation(bool isOpen, int buttonIndex, Action onCompleteInitAnimation)
        {
            panels[buttonIndex].panelTitleText.color = panelTitleColors[isOpen ? 1 : 0];
            if (isOpen)
            {
                panels[buttonIndex].panelTransform.gameObject.SetActive(true);
                rootLayoutParent.offsetMin = new Vector2(rootLayoutParent.offsetMin.x, panels[buttonIndex].containerMinY);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayoutParent);

                float sizePanelMultiplier = PANEL_NORMAL_SIZE / panels[buttonIndex].containerMinY;
                float positionScrollToChild = 1 - (buttonIndex * sizePanelMultiplier * SCROLL_CHILD_FACTOR);
                Debug.Log((1 - positionScrollToChild) * .5f);
                LeanTween.value(1, positionScrollToChild, ( positionScrollToChild)*.5f)
                         .setOnUpdate((float value) => scrollRect.normalizedPosition = new Vector2(0, value))
                         .setOnComplete(() => onCompleteInitAnimation?.Invoke());
            }
            else
            {
                onCompleteInitAnimation?.Invoke();
            }
        }

        #endregion <<<Execute anim>>>
        #endregion ----Methods----
    }
}
