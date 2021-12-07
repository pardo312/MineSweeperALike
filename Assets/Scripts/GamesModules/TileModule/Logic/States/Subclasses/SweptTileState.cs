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
        #endregion BackingFields

        #region Properties
        public Sprite m_stateSprite => m_stateSpriteField;

        #endregion Properties
        #endregion Fields

        #region Methods
        #region Init
        public Sprite InitState(MineSweeperTile tileBase)
        {
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
        public void Sweep(MineSweeperTile tileBase) { }
        public void Flag(MineSweeperTile tileBase) { }
        #endregion Not Used State Methods
        #endregion Methods
    }
}
