using System;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class HomePanelsManager : MonoBehaviour
    {
        #region ----Fields----
        [SerializeField] private RectTransform m_playButton;
        [SerializeField] private RectTransform m_modeSelectContainer;
        [SerializeField] private SelectPanelControllerBase[] m_panelControllers;
        [SerializeField] private float perAnimTime = 1f;

        private int currentPanelController;
        #endregion ----Fields----

        #region ----Methods----	
        public void ShowModeSelectContainer()
        {
            HidePlayButton(ShowOptions);
        }

        public void HidePlayButton(Action onComplete = null)
        {
            m_playButton.GetChild(0).GetComponent<Button>().interactable = false;
            m_playButton.LeanMoveY(m_playButton.anchoredPosition.y - 1500, perAnimTime)
                        .setEase(LeanTweenType.easeInBack)
                        .setOnComplete(() =>
                        {
                            onComplete?.Invoke();
                            m_playButton.gameObject.SetActive(false);
                        });
        }
        public void ShowOptions()
        {
            float initYPosition = m_modeSelectContainer.anchoredPosition.y;
            m_modeSelectContainer.position = new Vector2(m_modeSelectContainer.position.x, m_playButton.position.y);
            m_modeSelectContainer.gameObject.SetActive(true);

            m_modeSelectContainer.LeanMoveY(initYPosition, perAnimTime).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
            {
                m_panelControllers[currentPanelController].gameObject.SetActive(true);
                m_panelControllers[currentPanelController].Init();
            });
        }
        #endregion ----Methods----	
    }
}