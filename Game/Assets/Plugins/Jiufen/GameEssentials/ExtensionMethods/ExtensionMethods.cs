using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    #region ----Fields----
    #endregion ----Fields----

    #region ----Methods----	
    public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable, IComparable<T>
    {
        return Comparer<T>.Default.Compare(item, start) >= 0
            && Comparer<T>.Default.Compare(item, end) <= 0;
    }
    #endregion ----Methods----	
}
