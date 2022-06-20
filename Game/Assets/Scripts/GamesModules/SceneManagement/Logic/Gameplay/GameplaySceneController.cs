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

            BoardData boardData = boardDataModel.success ? (BoardData)(boardDataModel.data) : new BoardData();
            _callback?.Invoke(true);
            gameplayController.Init((BoardDifficulty)difficultyModel.data, boardDataModel.success, boardData);
        }

        public void GoBackHome()
        {
            GameManager.m_instance.GoTo(SceneNames.HOME);
        }
    }
}