using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{
    public class DataManager : MonoBehaviour
    {
        #region ----Fields----
        public DataControllerBase[] m_dataControllers;
        public Dictionary<string, Action<object, Action<DataResponseModel>>> m_dataEvents = new Dictionary<string, Action<object, Action<DataResponseModel>>>();
        #endregion ----Fields----

        #region ----Singleton----
        public static DataManager m_instance;

        public void Awake()
        {
            if (m_instance != null)
            {
                Destroy(this);
                return;
            }

            m_instance = this;
            DontDestroyOnLoad(this);
            InitDataManager();
        }

        #endregion ----Singleton----

        #region ----Methods----
        public void InitDataManager()
        {
            foreach (var dataController in m_dataControllers)
                dataController.Init();
        }

        public void ReadEvent(string nameOfEvent, object data, Action<DataResponseModel> callback = null)
        {
            if (m_dataEvents.ContainsKey(nameOfEvent))
                m_dataEvents[nameOfEvent]?.Invoke(data, callback);
            else
                Debug.Log($"DataManager doesn't have the event key: {nameOfEvent}");
        }

        public void AddListeners(string nameOfEvent, Action<object, Action<DataResponseModel>> newEvent)
        {
            m_dataEvents.Add(nameOfEvent, newEvent);
        }

        public void RemoveListeners(string nameOfEvent)
        {
            m_dataEvents.Remove(nameOfEvent);
        }
        #endregion ----Methods----
    }
}