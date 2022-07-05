using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.PopupModule
{
    [CreateAssetMenu(fileName = "NewLobbyMultiplayerInfo", menuName = "PuzzleAssets/LobbyMultiplayer Info", order = 1)]
    public class LobbyMultiplayerInfo_scrObj : ScriptableObject
    {
        [Header("InfoMessages")]
        [SerializeField] public List<PopUpInfoModel> lobbyMultiplayerMessage;
    }
}