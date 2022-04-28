using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        #region ----Fields----
        public ServiceLocatorConfig config;
        private Dictionary<Type, IService> services = new Dictionary<Type, IService>();
        #endregion ----Fields----

        #region ----Singleton----
        public static ServiceLocator m_Instance;
        public void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(this);
            }
            else
                DestroyImmediate(this);

        }
        #endregion ----Singleton----

        #region ----Methods----
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            GameObject serviceLocatorGameObject = new GameObject();
            serviceLocatorGameObject.name = "ServiceLocator";
            ServiceLocator serviceLocator = serviceLocatorGameObject.AddComponent<ServiceLocator>();
            serviceLocator.config = Resources.Load<ServiceLocatorConfig>("ServiceLocator/ServiceLocatorConfig");
            if (serviceLocator.config == null)
                Debug.LogError("Service locator not found. Please create it in Resources/ServiceLocator/ServiceLocatorConfig.asset");
        }

        public void Start()
        {

        }

        public T GetService<T>() where T : IService
        {
            T foundService = default(T);
            if (services.ContainsKey(typeof(T)))
                return (T)services[typeof(T)];
            else
            {
                config.listOfPrefabs.ForEach(item =>
                {
                    if (item.GetComponentInChildren<T>() != null)
                    {
                        var go = Instantiate(item, this.transform);
                        foundService = go.GetComponentInChildren<T>();
                        services.Add(typeof(T), foundService);
                    }
                    else
                        Debug.LogError($"Service {typeof(T).Name} not found in ServiceLocator");
                });
            }
            return foundService;
        }
        #endregion ----Methods----
    }
}