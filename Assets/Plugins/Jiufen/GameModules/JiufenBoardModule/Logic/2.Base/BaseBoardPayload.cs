using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenGames.Board.Logic
{
    public struct BaseBoardPayload
    {
        public int _rows, _columns;
        public bool _squaredTiles;
    }
    public struct BaseBoardDto
    {
        public Vector2 _sizeOfTiles;
    }
}