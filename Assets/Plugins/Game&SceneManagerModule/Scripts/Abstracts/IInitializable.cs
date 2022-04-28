using System;

namespace JiufenPackages.SceneFlow.Logic
{
    /// <summary>
    /// Class that handles, sceneName and the get of his needing data
    /// </summary>
    public interface IInitializable
    {
        string m_sceneName { get; }

        void GetData(Action<object> callback);

        void GetTestData(Action<object> callback);
    }
}