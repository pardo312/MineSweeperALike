using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
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
        public void Awake()
        {
            currentOpenedButton = -1;
            rootLayoutParent = this.GetComponent<RectTransform>();
        }

        public void OpenButton(int buttonIndex)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            int currentOpenButtonNow = currentOpenedButton;
            if (currentOpenedButton >= 0 && currentOpenedButton != buttonIndex)
                PanelAnimation(currentOpenedButton, false, () => InitPanelAnimation(currentOpenButtonNow,buttonIndex));
            else
                InitPanelAnimation(currentOpenButtonNow, buttonIndex);
        }

        public void InitPanelAnimation(int currentOpenButtonNow, int buttonIndex)
        {
            if (currentOpenButtonNow == buttonIndex)
                PanelAnimation(buttonIndex, false);
            else
                PanelAnimation(buttonIndex, true);
        }

        public void PanelAnimation(int buttonIndex, bool isOpen, Action onComplete = null)
        {
            if (isOpen)
            {
                panels[buttonIndex].panelTransform.gameObject.SetActive(true);
                rootLayoutParent.offsetMin = new Vector2(rootLayoutParent.offsetMin.x, panels[buttonIndex].containerMinY);

                float sizePanelMultiplier = 1 / (panels[buttonIndex].containerMinY / -875f);
                scrollRect.normalizedPosition = new Vector2(0, 1 - (buttonIndex * .35f * sizePanelMultiplier));

                LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayoutParent);
            }
            panels[buttonIndex].panelTitleText.color = panelTitleColors[isOpen ? 1 : 0];
            int targetPosition = isOpen ? panels[buttonIndex].middlePanelSizeY : 0;
            //rootLayoutParent.offsetMax = new Vector2 (rootLayoutParent.offsetMax.x, -330 * buttonIndex);

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
                    scrollRect.normalizedPosition = new Vector2(0, 1);
                    currentOpenedButton = -1;
                }
                else
                    currentOpenedButton = buttonIndex;

                onComplete?.Invoke();
                isAnimating = false;
            });
        }
        #endregion ----Methods----
    }

}