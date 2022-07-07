using System;
using System.Linq;
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
        [SerializeField] private HomeModeDragController homeModeDragController;

        private int currentPanelController = 0;
        #endregion ----Fields----

        #region ----Methods----	
        #region Init
        public void Start()
        {
            m_panelControllers.ToList().ForEach((item) => item.Init());
            homeModeDragController.onChangeMode += ShowModeOptions;
        }

        public void ShowModeSelectContainer()
        {
            HidePlayButton(ShowInitMode);
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
        public void ShowInitMode()
        {
            float initYPosition = m_modeSelectContainer.anchoredPosition.y;
            m_modeSelectContainer.position = new Vector2(m_modeSelectContainer.position.x, m_playButton.position.y);
            m_modeSelectContainer.gameObject.SetActive(true);

            m_modeSelectContainer.LeanMoveY(initYPosition, perAnimTime).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
            {
                ShowPanel();
            });
        }
        #endregion Init

        #region Show other panel
        public void ShowModeOptions(int newMode)
        {
            if (newMode == currentPanelController)
                return;

            m_panelControllers[currentPanelController].HidePanel(() =>
            {
                m_panelControllers[currentPanelController].gameObject.SetActive(false);
                currentPanelController = newMode;
                ShowPanel();
            });

        }

        public void ShowPanel()
        {
            m_panelControllers[currentPanelController].gameObject.SetActive(true);
            m_panelControllers[currentPanelController].ShowWholePanel();
        }
        #endregion Show other panel
        #endregion ----Methods----	
    }
}