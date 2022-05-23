using JiufenGames.MineSweeperAlike.HomeModule;
using System.Collections.Generic;
using UnityEngine;

public struct BoardData
{
    public int rows;
    public int columns;
    public int maxMines;
    public Difficulty difficulty;
    public List<Vector2Int> minesPositions;
    public List<Vector2Int> sweepedTilePositions;
    public List<Vector2Int> flaggedPositions;
}
