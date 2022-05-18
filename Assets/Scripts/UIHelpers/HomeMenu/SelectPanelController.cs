using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class SelectPanelController : MonoBehaviour
    {
        #region ----Fields----
        private bool isAnimating;

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
            rootLayoutParent = this.GetComponent<RectTransform>();
        }

        public void OpenButton(int buttonIndex)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            if (currentOpenedButton >= 0)
                PanelAnimation(currentOpenedButton, false);

            if (currentOpenedButton == buttonIndex)
                PanelAnimation(buttonIndex, false);
            else
                PanelAnimation(buttonIndex, true);
        }

        public void PanelAnimation(int buttonIndex, bool isOpen)
        {
            rootLayoutParent.offsetMin = new Vector2(rootLayoutParent.offsetMin.x, isOpen ? -1700 : 0);
            panels[buttonIndex].panelTransform.gameObject.SetActive(isOpen);

            panels[buttonIndex].panelTitleText.color = panelTitleColors[isOpen ? 1 : 0];
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
                    currentOpenedButton = -1;
                }
                else
                    currentOpenedButton = buttonIndex;
                isAnimating = false;
            });
        }
        #endregion ----Methods----
    }

}