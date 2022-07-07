using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<Button> modeButtons;
    public Action<int> onChangeMode;

    private bool isChecking = false;
    private int currentPanel = 0;
    public void CheckvalueOfScroll(BaseEventData data)
    {
        if (isChecking)
            return;

        isChecking = true;
        float buttonSize = 1f / modeButtons.Count;

        for (int i = 0; i < modeButtons.Count; i++)
        {
            float buttonInitPos = i * buttonSize;
            float buttonFinalPos = buttonInitPos + buttonSize;
            if (scrollRect.horizontalNormalizedPosition.IsBetween(buttonFinalPos, buttonInitPos))
            {
                scrollRect.horizontalNormalizedPosition = i;
                if (i != currentPanel)
                {
                    modeButtons[currentPanel].interactable = false;
                    currentPanel = i;
                    modeButtons[i].interactable = true;
                    onChangeMode?.Invoke(currentPanel);
                }
                break;
            }
        }
        isChecking = false;
    }
    #endregion ----Methods----	
}
