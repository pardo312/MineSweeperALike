using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeModeDragController : MonoBehaviour
{
    #region ----Fields----
    #endregion ----Fields----

    #region ----Methods----	
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public List<RectTransform> panelsRectTransforms;
    public Action<int> onChangeMode;

    private bool isChecking = false;
    private int currentPanel = 0;
    public void CheckvalueOfScroll(BaseEventData data)
    {
        if (isChecking)
            return;

        isChecking = true;
        float panelSize = 1f / panelsRectTransforms.Count;
        for (int i = 0; i < panelsRectTransforms.Count; i++)
        {
            float panelInitPos = i * panelSize;
            float panelFinalPos = panelInitPos + panelSize;
            if (scrollRect.horizontalNormalizedPosition >= panelInitPos && scrollRect.horizontalNormalizedPosition <= panelFinalPos)
            {
                scrollRect.horizontalNormalizedPosition = i;
                if (i != currentPanel)
                {
                    currentPanel = i;
                    onChangeMode?.Invoke(currentPanel);
                }
                break;
            }
        }
        isChecking = false;
    }
    #endregion ----Methods----	
}
