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
            m_tileBase.ChangeTileData(new MineDataPayload() { StateToChange = "SweptTileState", Sweeping = true });
        }

        public void Flag()
        {
            m_tileBase.ChangeTileData(new MineDataPayload() { StateToChange = "FlaggedTileState", FlaggingTile = true });
        }
    }
}
