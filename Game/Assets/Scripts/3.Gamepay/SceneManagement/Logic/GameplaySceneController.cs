using Jiufen.Audio;
using JiufenGames.MineSweeperAlike.Gameplay.Logic;
using JiufenGames.MineSweeperAlike.HomeModule;
using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class GameplaySceneController : SceneController
    {
        public GameplayController gameplayController;
        public override void Init(DataResponseModel _dataResponse, Action<bool> _callback = null)
        {
            dynamic responseData = _dataResponse.data;

            DataResponseModel difficultyModel = responseData.difficulty;
            DataResponseModel boardDataModel = responseData.boardData;

            BoardSaveData boardData = boardDataModel.success ? (BoardSaveData)(boardDataModel.data) : new BoardSaveData();
            _callback?.Invoke(true);
            gameplayController.Init((BoardDifficulty)difficultyModel.data, boardDataModel.success, boardData);
            gameplayController.goBackToHome += GoBackHome;
        }
        public void Start()
        {
            AudioManager.PlayAudio("OST_GAMEPLAY", new AudioJobOptions() { loop = true, volume = 0.1f, fadeIn = new AudioFadeInfo(true, 3) });
        }

        public void GoBackHome()
        {
            GameManager.m_instance.GoTo(SceneNames.HOME);
        }
    }
}