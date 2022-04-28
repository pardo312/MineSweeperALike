using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.SceneFlow.Logic
{
    public class SceneFlowManager : MonoBehaviour
    {
        #region ----Fields----
        [SerializeField] private string loadingSceneName = "Loading";

        private Dictionary<string, IInitializable> initilizables = new Dictionary<string, IInitializable>();
        private string previousScene = "";
        #endregion ----Fields----

        #region ----Methods----
        #region Init
        /// <summary>
        /// Get all of the <see cref="IInitializable">initializables</see> on the prefab and add them to a dictionary. 
        /// Key name of scene in initializable, Value initializable class
        /// </summary>
        public void Init()
        {
            IInitializable[] initializableList = GetComponents<IInitializable>();

            for (int i = 0; i < initializableList.Length; i++)
                initilizables.Add(initializableList[i].m_sceneName, initializableList[i]);
        }
        #endregion Init

        #region Change Scene
        /// <summary>
        /// Store previous scene, show loading screen and then load the Scene
        /// </summary>
        /// <param name="nameOfScene">Name of the scene that is going to be load</param>
        public void ChangeSceneTo(string nameOfScene)
        {
            previousScene = SceneManager.GetActiveScene().name;
            ShowLoadingScene();
            LoadScene(nameOfScene);
        }

        /// <summary>
        /// Checks if the scene exist and if is found load it.
        /// Otherwise, load previous scene and unload
        /// </summary>
        /// <param name="nameOfScene">Name of the scene that is going to be load</param>
        private void LoadScene(string nameOfScene)
        {
            if (CheckIfSceneExist(nameOfScene))
            {
                SceneManager.LoadSceneAsync(nameOfScene);
                return;
            }

            if (nameOfScene.CompareTo(previousScene) != 0)
            {
                SceneManager.LoadScene(previousScene);
                Debug.Log($"Scene {nameOfScene} doesn't exist in build. Going back to previous scene: {previousScene}");
                return;
            }

            Debug.LogError($"SceneFlow Fatal: Scene {nameOfScene} doesn't exist in build. Previous Scene is equal to current scene so couldn't go back.");
        }

        /// <summary>
        /// Check if the scene exist and can be loaded;
        /// </summary>
        /// <param name="nameOfScene">Name of the scene</param>
        private bool CheckIfSceneExist(string nameOfScene)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
                if (nameOfScene == sceneName)
                    return true;
            }
            return false;
        }
        #endregion Change Scene

        #region LoadingScene
        /// <summary>
        /// Load LoadingScene
        /// </summary>
        private void ShowLoadingScene()
        {
            SceneManager.LoadSceneAsync(loadingSceneName);
        }

        /// <summary>
        /// UnloadLoadingScene
        /// </summary>
        private void HideLoadingScene(bool loadingSuccess)
        {
            if (loadingSuccess && SceneManager.GetActiveScene() == SceneManager.GetSceneByName(loadingSceneName))
                SceneManager.UnloadSceneAsync(loadingSceneName);
        }
        #endregion LoadingScene

        #region Init Scene
        /// <summary>
        /// Get the data from it's <see cref="IInitializable">initializable</see> to later init the scene controller.
        /// </summary>
        /// <param name="sceneName">sceneName</param>
        public void InitScene(string sceneName)
        {
            if (sceneName != loadingSceneName)
                initilizables[sceneName].GetData(InitializeSceneController);
        }

        /// <summary>
        /// Initialize the scene controller with the data from the initializable and then hide the loading scene.
        /// </summary>
        /// <param name="data"></param>
        private void InitializeSceneController(object data)
        {
            SceneController sceneController = SceneController.Instance;
            sceneController.Init(data, (successLoadingScene) =>
                HideLoadingScene(successLoadingScene));
        }
        #endregion Init Scene
        #endregion ----Methods----
    }
}