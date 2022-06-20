using TMPro;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    [System.Serializable]
    public struct SelectOptionPanelDto
    {
        public TMP_Text panelTitleText;
        public GameObject panelTransform;
        public RectTransform panelMask;
        public int middlePanelSizeY;
        public int containerMinY;
        public Transform optionsContainer;
    }

}