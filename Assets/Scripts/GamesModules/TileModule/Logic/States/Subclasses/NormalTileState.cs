using JiufenGames.MineSweeperAlike.Gameplay.Model;
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
        public bool m_canFlag = false;
        public bool m_canSweep = false;
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
            if (m_stateSpriteField == null)
                m_stateSpriteField = Resources.Load<Sprite>("Sprites/TileStates/Normal");
            return m_stateSprite;
        }
        #endregion Init

        #region Used State Methods
        public void Sweep(MineSweeperTile _tileBase)
        {
            _tileBase.ChangeTileData(new MineDataPayload() { StateToChange = "SweptTileState", Sweeping = true });
        }

        public void Flag(MineSweeperTile _tileBase)
        {
            _tileBase.ChangeTileData(new MineDataPayload() { StateToChange = "FlaggedTileState", FlaggingTile = true });
        }


        #endregion Used State Methods
        #endregion Methods
    }
}
