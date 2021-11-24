using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class NormalTileState : ITileState
    {
        #region Fields
        #region Class Fields
        public string m_stateName => "NormalTileState";
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
            m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Normal");
            return m_stateSprite;
        }

        public void Sweep()
        {
            if (m_tileBase.m_isMine)
            {
                //Base.endGame
                Debug.Break();
            }
            m_tileBase.ChangeTileData(new object[1] { "SweptTileState" });
        }

        public void Flag()
        {
            if (m_tileBase.m_isMine)
            {
                //Base.minesLeftMinus1
            }
            //Base.FlagsMinus1
            m_tileBase.ChangeTileData(new object[1] { "FlaggedTileState" });
        }
    }
}
