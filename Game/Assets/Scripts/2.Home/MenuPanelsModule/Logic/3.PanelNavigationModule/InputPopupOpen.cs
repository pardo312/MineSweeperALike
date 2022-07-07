using JiufenGames.PopupModule;
using JiufenPackages.ServiceLocator;
using TMPro;
using UnityEngine;

public class InputPopupOpen : MonoBehaviour
{
    #region ----Fields----
    #endregion ----Fields----

    #region ----Methods----	
    public void OpenPopupInput(string currentText)
    {
        ServiceLocator.m_Instance.GetService<IPopupManager>().ShowInputPopup(this.GetComponent<TMP_InputField>(), currentText);
    }
    #endregion ----Methods----	
}
