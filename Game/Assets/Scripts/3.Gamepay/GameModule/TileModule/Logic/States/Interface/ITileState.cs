using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.Gameplay.Logic
{

    public interface ITileState
    {
        string m_stateName { get; }
        Sprite m_stateSprite { get; }

        Sprite InitState(MineSweeperTile tileBase);
        void Sweep(MineSweeperTile tileBase);
        void Flag(MineSweeperTile tileBase);
    }
}
