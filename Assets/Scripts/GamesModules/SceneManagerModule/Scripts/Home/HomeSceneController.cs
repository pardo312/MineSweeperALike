using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public TMP_Text titleLabel;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            _callback?.Invoke(true);
        }

        [ContextMenu("Press me")]
        public void ShowTitle()
        {
            StartCoroutine(TextAppearingAnimation());
        }

        IEnumerator TextAppearingAnimation()
        {
            titleLabel.ForceMeshUpdate();
            TMP_TextInfo textInfo = titleLabel.textInfo;

            int characterCount = textInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                int timer = 0;
                while (timer < 100)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                        continue;
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                    for (int k = 0; k < 4; k++)
                        destinationVertices[vertexIndex + k] += new Vector3(0, 1.5f);

                    textInfo.meshInfo[materialIndex].mesh.vertices = textInfo.meshInfo[materialIndex].vertices;
                    titleLabel.UpdateGeometry(textInfo.meshInfo[materialIndex].mesh, materialIndex);
                    timer++;
                    yield return null;
                }
                yield return new WaitForSeconds(0.05f);
            }
            //TODO: Color
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }
        #endregion ----Methods----
    }
}