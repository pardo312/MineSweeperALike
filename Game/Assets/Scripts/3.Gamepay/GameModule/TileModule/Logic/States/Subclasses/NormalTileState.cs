﻿using JiufenGames.MineSweeperAlike.Gameplay.Model;
using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{
    public class NormalTileState : ITileState
    {
        #region Fields
        #region Class Fields
        public string m_stateName => TileStatesConstants.NORMAL_TILE_STATE;
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
            float animTime = .5f;
            LeanTween.scale(_tileBase.gameObject, Vector2.one * .2f, animTime).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                LeanTween.scale(_tileBase.gameObject, Vector2.one, animTime).setEase(LeanTweenType.easeOutBack);
            });
            _tileBase.ChangeTileData(new MineDataPayload() { StateToChange = TileStatesConstants.SWEPT_TILE_STATE, Sweeping = true });
        }

        public void Flag(MineSweeperTile _tileBase)
        {
            _tileBase.ChangeTileData(new MineDataPayload() { StateToChange = TileStatesConstants.FLAGGED_TILE_STATE, FlaggingTile = true });
        }


        #endregion Used State Methods
        #endregion Methods
    }
}
