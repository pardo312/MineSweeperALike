using System.Collections.Generic;
using UnityEngine;
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
    public void CheckvalueOfScroll(Vector2 scroll)
    {
        if (isChecking)
            return;
        if (scroll.x == 0)
            return;

        isChecking = true;
        if (scroll.x < 0 && currentPanel > 0)
            currentPanel--;
        else if (scroll.x > 0 && currentPanel + 1 < panelsRectTransforms.Count)
            currentPanel++;

        SnapTo(panelsRectTransforms[currentPanel]);

    }
    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
    #endregion ----Methods----	
}
