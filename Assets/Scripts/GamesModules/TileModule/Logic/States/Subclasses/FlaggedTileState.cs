using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public class FlaggedTileState : MonoBehaviour, ITileState
    {
        public string m_stateName => "FlaggedTileState";

        public Sprite m_stateSpriteField;
        public Sprite m_stateSprite => m_stateSpriteField;

        public Sprite InitState()
        {
            Resources.Load("Sprites/TilesStates/Flagged");
            return m_stateSprite;
        }
    }
}
