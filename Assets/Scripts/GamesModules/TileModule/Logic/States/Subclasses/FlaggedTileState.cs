using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;
using JiufenGames.MineSweeperAlike.Gameplay.Model;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public class FlaggedTileState : ITileState
    {
        #region Fields
        #region BackingFields
        public Sprite m_stateSpriteField;
        public MineSweeperTile m_tileBaseField;
        #endregion BackingFields

        #region Properties
        public Sprite m_stateSprite => m_stateSpriteField;
        public MineSweeperTile m_tileBase => m_tileBaseField;
        #endregion Properties

        #region Class Fields
        public string m_stateName => "FlaggedTileState";
        #endregion Class Fields
        #endregion Fields

        #region Methods
        #region Init
        public Sprite InitState(MineSweeperTile tileBase)
        {
            m_tileBaseField = tileBase;
            if (m_stateSpriteField == null)
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Flag");
            //if(base.flags == base.mines && base.allnotminesswept)
            //    EndGameWin
            return m_stateSprite;
        }
        #endregion Init

        #region Used State Methods
        public void Flag()
        {
            m_tileBase.ChangeTileData(new MineDataPayload() { StateToChange = "NormalTileState", DeFlagMine = true });
        }
        #endregion Used State Methods

        #region Not Used State Methods
        public void Sweep() { }
        #endregion Not Used State Methods
        #endregion Methods
    }
}
