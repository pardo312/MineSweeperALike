using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public class SweptTileState : ITileState
    {
        public string m_stateName => "SweptTileState";

        public Sprite m_stateSpriteField;
        public Sprite m_stateSprite => m_stateSpriteField;

        public Sprite InitState()
        {
            Resources.Load("Sprites/TilesStates/Swept");
            return m_stateSprite;
        }
    }
}
