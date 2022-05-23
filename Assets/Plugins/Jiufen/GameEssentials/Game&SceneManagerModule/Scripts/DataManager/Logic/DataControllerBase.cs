using UnityEngine;

public abstract class DataControllerBase : MonoBehaviour
{
    public void Init()
    {
        AddDataManasgerListeners();
    }

    /// <summary>
    /// Add listeners to the data manager 
    /// </summary>
    public abstract void AddDataManasgerListeners();
}