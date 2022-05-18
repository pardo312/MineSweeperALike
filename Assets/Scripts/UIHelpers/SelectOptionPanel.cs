using TMPro;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    [System.Serializable]
    public struct SelectOptionPanel
    {
        public TMP_Text panelTitleText;
        public GameObject panelTransform;
        public RectTransform panelMask;
        public int middlePanelSizeY;
    }

}