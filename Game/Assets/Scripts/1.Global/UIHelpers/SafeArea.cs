using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class SafeArea : MonoBehaviour
    {
        private RectTransform SafePanelRect;

        private Rect lastSafeArea = Rect.zero;

        private void Awake()
        {
            lastSafeArea = Screen.safeArea;
            SafePanelRect = GetComponent<RectTransform>();
            AdjustSafeArea();
        }

        private void AdjustSafeArea()
        {
            if (SafePanelRect != null)
            {
                var safeArea = Screen.safeArea;

                var anchorMin = safeArea.position;
                var anchorMax = safeArea.position + safeArea.size;

                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                SafePanelRect.anchorMin = anchorMin;
                SafePanelRect.anchorMax = anchorMax;
            }
        }

        private void LateUpdate()
        {
            if (lastSafeArea != Screen.safeArea)
            {
                lastSafeArea = Screen.safeArea;
                AdjustSafeArea();
            }
        }
    }
}