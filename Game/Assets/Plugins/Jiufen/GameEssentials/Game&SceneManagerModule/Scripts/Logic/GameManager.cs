using JiufenPackages.SceneFlow.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.GameManager.Logic
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        private SceneFlowManager m_sceneFlowManager;

        #endregion Fields

        #region Methods

        #region Singleton

        [HideInInspector] public static GameManager m_instance;

        private void Awake()
        {
            //Change This
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }

        #endregion Singleton

        #region RuntimeInit

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInit()
        {
            foreach (GameObject commonObject in Resources.LoadAll("Common", typeof(GameObject)))
            {
                GameObject gameManagerGO = Instantiate(commonObject);
                gameManagerGO.GetComponent<GameManager>().Init();
            }

            SceneManager.sceneLoaded += m_instance.OnSceneLoad;
        }

        public void Init()
        {
            m_sceneFlowManager = GetComponentInChildren<SceneFlowManager>();

            if (m_sceneFlowManager != null)
                m_sceneFlowManager.Init();
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            m_sceneFlowManager.InitScene(scene.name);
        }

        public void GoTo(string sceneName)
        {
            m_sceneFlowManager.ChangeSceneTo(sceneName);
        }

        #endregion RuntimeInit

        #endregion Methods
    }
}