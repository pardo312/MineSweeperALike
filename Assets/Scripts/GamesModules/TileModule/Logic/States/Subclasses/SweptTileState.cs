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

        #region Methods
        #region Init
        public Sprite InitState(MineSweeperTile tileBase)
        {
            m_tileBaseField = tileBase;
            if (tileBase.m_isMine)
            {
                tileBase.ExplodeMine();
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Mine");
            }
            else
            {
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Swept");
            }
            return m_stateSprite;
        }
        #endregion Init

        #region Not Used State Methods
        public void Sweep() { }
        public void Flag() { }
        #endregion Not Used State Methods
        #endregion Methods
    }
}
