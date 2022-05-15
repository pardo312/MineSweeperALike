using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class PausePanelController : MonoBehaviour
    {
        #region ----Fields----
        [SerializeField] private RectTransform m_circleMiddleRectTransform;
        [SerializeField] private float m_animTime;
        [SerializeField] private GameObject m_PausePanel;
        [SerializeField] private Image m_PauseCover;
        [SerializeField] private RectMask2D m_pauseMask;

        [SerializeField] private GameObject m_UnpausePanel;
        [SerializeField] private Image m_UnpauseCover;
        #endregion ----Fields----

        #region ----Methods----
        [ContextMenu("1")]
        public void ExpandPauseMenu()
        {
            AnimatePauseContainer(500, m_UnpausePanel, m_UnpauseCover, m_PausePanel, m_PauseCover);
            LeanTween.rotate(m_UnpausePanel, new Vector3(0, 0, -90), m_animTime);
            LeanTween.scale(m_UnpausePanel, Vector3.one * 0.05f, m_animTime);
        }

        [ContextMenu("2")]
        public void ContractPauseMenu()
        {
            AnimatePauseContainer(0, m_PausePanel, m_PauseCover, m_UnpausePanel, m_UnpauseCover);
            LeanTween.rotate(m_UnpausePanel, new Vector3(0, 0, 0), m_animTime);
            LeanTween.scale(m_UnpausePanel, Vector3.one, m_animTime);
        }

        public void AnimatePauseContainer(int finalValue, GameObject _panel1ToHide, Image _cover1ToShow, GameObject _panel2ToShow, Image _cover2ToHide)
        {
            Vector2 tempSize = m_circleMiddleRectTransform.sizeDelta;
            RectTransform _maskTransform = m_pauseMask.GetComponent<RectTransform>();
            Color tempColor;

            LeanTween.value(m_circleMiddleRectTransform.sizeDelta.x, finalValue, m_animTime)
            .setOnUpdate((float value) =>
            {
                tempSize.x = value;
                m_circleMiddleRectTransform.sizeDelta = tempSize;
                _maskTransform.sizeDelta = tempSize + new Vector2(50, 0);

            })
            .setOnComplete(() =>
            {
                _panel1ToHide.SetActive(false);
            });

            _panel2ToShow.SetActive(true);
            LeanTween.value(1, 0, m_animTime)
            .setOnUpdate((float value) =>
            {
                tempColor = _cover1ToShow.color;
                tempColor.a = 1 - value;
                _cover1ToShow.color = tempColor;

                tempColor = _cover2ToHide.color;
                tempColor.a = value;
                _cover2ToHide.color = tempColor;
            });
        }

        #endregion ----Methods----
    }
}