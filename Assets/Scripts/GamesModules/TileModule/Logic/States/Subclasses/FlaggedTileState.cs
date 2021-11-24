using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public class FlaggedTileState : ITileState
    {
        #region Fields
        #region Class Fields
        public string m_stateName => "FlaggedTileState";
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
            m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Flag");
            //if(base.flags == base.mines && base.allnotminesswept)
            //    EndGameWin
            return m_stateSprite;
        }

        public void Sweep()
        {
            return;
        }

        public void Flag()
        {
            m_tileBase.ChangeTileData(new object[1] { "NormalTileState" });
        }
    }
}
