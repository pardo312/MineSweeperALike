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
        #endregion BackingFields

        #region Properties
        public Sprite m_stateSprite => m_stateSpriteField;
        #endregion Properties

        #region Class Fields
        public string m_stateName => TileStatesConstants.FLAGGED_TILE_STATE;
        #endregion Class Fields
        #endregion Fields

        #region Methods
        #region Init
        public Sprite InitState(MineSweeperTile tileBase)
        {
            if (m_stateSpriteField == null)
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Flag");
            //if(base.flags == base.mines && base.allnotminesswept)
            //    EndGameWin
            return m_stateSprite;
        }
        #endregion Init

        #region Used State Methods
        public void Flag(MineSweeperTile _tileBase)
        {
            _tileBase.ChangeTileData(new MineDataPayload() { StateToChange = TileStatesConstants.NORMAL_TILE_STATE, DeFlagMine = true });
        }
        #endregion Used State Methods

        #region Not Used State Methods
        public void Sweep(MineSweeperTile tileBase) { }
        #endregion Not Used State Methods
        #endregion Methods
    }
}
