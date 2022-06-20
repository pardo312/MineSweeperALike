using JiufenGames.TetrisAlike.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.Gameplay.Model
{
    public struct MineDataPayload
    {
        public string StateToChange;

        public bool Sweeping;
        public bool FlaggingTile;
        public bool DeFlagMine;
    }
}
