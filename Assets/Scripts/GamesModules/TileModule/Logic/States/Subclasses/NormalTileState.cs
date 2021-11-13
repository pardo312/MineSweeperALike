using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class NormalTileState : MonoBehaviour,ITileState
    {
        public string m_stateName => "NormalTileState";

        public Sprite m_stateSpriteField;
        public Sprite m_stateSprite => m_stateSpriteField;

        public Sprite InitState()
        {
            Resources.Load("Sprites/TilesStates/Normal");
            return m_stateSprite;
        }
    }
}
