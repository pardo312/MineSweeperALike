using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.UIHelpers
{
    public class TMPTextHelpers : MonoBehaviour
    {
        #region ----Fields----
        [SerializeField] private Color primaryColor;
        [SerializeField] private Color secondaryColor;
        private bool isSecondary = false;

        private TMP_Text text;
        #endregion ----Fields----

        #region ----Methods----
        public void Awake()
        {
            text = GetComponent<TMP_Text>();
            if (text == null)
                Debug.LogError("Can't found text in gameobject", this);
        }
        public void ChangeColorOfText()
        {
            if (isSecondary)
                text.color = primaryColor;
            else
                text.color = secondaryColor;

            isSecondary = !isSecondary;
        }
        #endregion ----Methods----
    }
}
