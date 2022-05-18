using UnityEngine;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class HomePanelsController : MonoBehaviour
    {
        #region ----Fields----
        public RectTransform m_playButton;
        public RectTransform m_modeSelectContainer;
        public RectTransform m_soloPanelMask;
        #endregion ----Fields----

        #region ----Methods----	
        public void ShowModeSelectContainer()
        {
            float initYPosition = m_modeSelectContainer.anchoredPosition.y;
            m_modeSelectContainer.position = new Vector2(m_modeSelectContainer.position.x, m_playButton.position.y);

            m_playButton.gameObject.SetActive(false);
            m_modeSelectContainer.gameObject.SetActive(true);

            m_modeSelectContainer.LeanMoveY(initYPosition, 1f).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
            {
                m_soloPanelMask.gameObject.SetActive(true);

                float soloPanelInitY = m_soloPanelMask.sizeDelta.y;
                Vector2 tempSize = m_soloPanelMask.sizeDelta;

                tempSize.y = 0;
                m_soloPanelMask.sizeDelta = tempSize;

                Transform child = m_soloPanelMask.GetChild(0);
                LeanTween.value(0, soloPanelInitY, .5f).setEase(LeanTweenType.easeInCubic).setOnUpdate((float value) =>
                {
                    child.SetParent(m_soloPanelMask.parent);
                    Vector2 tempSize = m_soloPanelMask.sizeDelta;
                    tempSize.y = value;
                    m_soloPanelMask.sizeDelta = tempSize;
                    child.SetParent(m_soloPanelMask);
                }).setOnComplete(() =>
                {
                });
            });
        }
        #endregion ----Methods----	
    }
}