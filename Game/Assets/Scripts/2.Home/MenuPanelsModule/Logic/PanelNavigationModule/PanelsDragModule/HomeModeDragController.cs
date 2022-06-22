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
                currentPanel = i;
                scrollRect.horizontalNormalizedPosition = panelInitPos + (panelSize / 2f);
                break;
            }
        }
        isChecking = false;

    }
    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition = new Vector2(1, 0) * (
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position));
    }
    #endregion ----Methods----	
}
