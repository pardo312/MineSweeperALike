using System;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public enum TypeOfOptionPanelAnimation
    {
        OPEN,
        CLOSE
    }
    public abstract class SelectPanelControllerBase : MonoBehaviour
    {
        #region ----Fields----
        private bool isAnimating;

        private int currentOpenedButton;
        [Header("Parameters")]
        [SerializeField] private Color[] panelTitleColors;
        [SerializeField] private float animationTime = .25f;

        [Header("References")]
        [SerializeField] internal SelectOptionPanelDto[] panels;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform rootLayoutParent;
        #endregion ----Fields----

        #region ----Methods----
        #region Init class
        public virtual void Init()
        {
            currentOpenedButton = -1;

            ShowWholePanel(this.GetComponent<RectTransform>());
        }

        public virtual void ShowWholePanel(RectTransform _panelRectTransform)
        {
            isAnimating = true;

            float soloPanelInitY = _panelRectTransform.sizeDelta.y;
            Vector2 tempSize = _panelRectTransform.sizeDelta;
            tempSize.y = 0;
            _panelRectTransform.sizeDelta = tempSize;

            Transform panelContainer = _panelRectTransform.GetChild(0);
            LeanTween.value(0, soloPanelInitY, animationTime * 2)
                     .setEase(LeanTweenType.easeInCubic)
                     .setOnUpdate((float value) =>
                     {
                         panelContainer.SetParent(_panelRectTransform.parent);
                         Vector2 tempSize = _panelRectTransform.sizeDelta;
                         tempSize.y = value;
                         _panelRectTransform.sizeDelta = tempSize;
                         panelContainer.SetParent(_panelRectTransform);
                     }).
                     setOnComplete(() => isAnimating = false);
        }
        public void HidePanel()
        {
            //TODO
        }
        #endregion Init class

        #region<<<Set anim>>> 
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
                        LeanTween.value(scrollRect.normalizedPosition.y, 1, buttonIndex == 0 ? 0 : .3f)
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
                LeanTween.value(1, positionScrollToChild, buttonIndex == 0 ? 0 : .3f)
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
