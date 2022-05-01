using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.SceneManagement
{
    public class HomeSceneController : SceneController
    {
        #region ----Fields----
        public TMP_Text titleLabel;
        public Image playButtonCover;
        #endregion ----Fields----

        #region ----Methods----
        public override void Init<T>(T _data, Action<bool> _callback = null)
        {
            _callback?.Invoke(true);
            StartCoroutine(TextAppearingAnimation());
        }

        IEnumerator TextAppearingAnimation()
        {
            titleLabel.alpha = 0;

            var tempColor = playButtonCover.color;
            tempColor.a = 1f;
            playButtonCover.color = tempColor;

            titleLabel.ForceMeshUpdate();
            TMP_TextInfo textInfo = titleLabel.textInfo;

            int characterCount = textInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                int timer = 0;
                byte alpha = 0;
                while (timer < 50)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                        continue;

                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                    for (int k = 0; k < 4; k++)
                    {
                        destinationVertices[vertexIndex + k] += new Vector3(0, 3f);
                        newVertexColors[vertexIndex + k].a = alpha;
                    }

                    textInfo.meshInfo[materialIndex].mesh.vertices = textInfo.meshInfo[materialIndex].vertices;
                    titleLabel.UpdateGeometry(textInfo.meshInfo[materialIndex].mesh, materialIndex);
                    titleLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    Debug.Log(alpha);
                    alpha += 5;
                    timer++;
                    yield return null;
                }
                yield return new WaitForSeconds(0.05f);
            }
            LeanTween.value(1, 0, 2).setOnUpdate((float value) =>
             {
                 tempColor = playButtonCover.color;
                 tempColor.a = value;
                 playButtonCover.color = tempColor;
             });
        }

        public void GoToGameplay()
        {
            GameManager.m_instance.GoTo(SceneNames.GAMEPLAY);
        }
        #endregion ----Methods----
    }
}