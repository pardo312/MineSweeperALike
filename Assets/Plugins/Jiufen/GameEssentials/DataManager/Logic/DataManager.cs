using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{
    public static class DataManager
    {
        #region ----Fields----
        public static Dictionary<string, Action<object, Action<object>>> m_dataEvents = new Dictionary<string, Action<object, Action<object>>>();
        #endregion ----Fields----

        #region ----Methods----
        public static void ReadEvent(string nameOfEvent, object data, Action<object> callback = null)
        {
            if (m_dataEvents.ContainsKey(nameOfEvent))
                m_dataEvents[nameOfEvent]?.Invoke(data, callback);
            else
                Debug.Log($"DataManager doesn't have the event key: {nameOfEvent}");
        }

        public static void AddListeners(string nameOfEvent, Action<object, Action<object>> newEvent)
        {
            m_dataEvents.Add(nameOfEvent, newEvent);
        }

        public static void RemoveListeners(string nameOfEvent)
        {
            m_dataEvents.Remove(nameOfEvent);
        }
        #endregion ----Methods----
    }
}