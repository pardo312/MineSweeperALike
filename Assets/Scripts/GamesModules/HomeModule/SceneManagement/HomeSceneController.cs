using JiufenGames.MineSweeperAlike.SceneManagement;
using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public BGHomeAnimation bGHomeAnimation;
        public TitleHomeAnimation titleHomeAnimation;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            _callback?.Invoke(true);
            bGHomeAnimation.Init();
            titleHomeAnimation.Init();
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }
        #endregion ----Methods----
    }
}
