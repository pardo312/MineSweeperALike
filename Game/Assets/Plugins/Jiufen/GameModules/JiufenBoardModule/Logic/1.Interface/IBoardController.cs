using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.Board.Logic
{
    public interface IBoardController<T>
    {
        GameObject m_tilePrefab { get; }
        Transform m_tileParent { get; }
        T[,] m_board { get; }

        void Init(object data);
        void CreateBoard(object payload, Action<int,int> createdTile = null, Action<object> _endCreationCallback = null);

    }
}