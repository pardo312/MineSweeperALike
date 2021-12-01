using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public class SweptTileState : ITileState
    {
        #region Fields
        #region Class Fields
        public string m_stateName => "SweptTileState";
        #endregion Class Fields

        #region BackingFields
        public Sprite m_stateSpriteField;
        public MineSweeperTile m_tileBaseField;
        #endregion BackingFields

        #region Properties
        public Sprite m_stateSprite => m_stateSpriteField;

        public MineSweeperTile m_tileBase => m_tileBaseField;
        #endregion Properties
        #endregion Fields

        public Sprite InitState(MineSweeperTile tileBase)
        {
            m_tileBaseField = tileBase;
            if (tileBase.m_isMine)
            {
                //EndGame
                tileBase.EndGame();
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Mine");
            }
            else
            {
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Swept");
            }
            return m_stateSprite;
        }

        public void Sweep()
        {
            return;
        }

        public void Flag()
        {
            return;
        }
    }
}
