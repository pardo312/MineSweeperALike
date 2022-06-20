using JiufenGames.MineSweeperAlike.InputModule;
using JiufenPackages.ServiceLocator;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    public class TitleLettersAnimationController : MonoBehaviour
    {
        #region ----Fields----
        public TMP_Text titleLabel;
        public Image playButtonCover;

        [Range(5, 50)]
        public float timeAnimation = 5;
        public float positionChangeY = 150;
        bool skipAnim = false;

        public Action a_onEndAnimation;
        #endregion ----Fields----

        #region ----Methods----
        public void Init()
        {
            ServiceLocator.m_Instance.GetService<IInputManager>().inputs.UI.Click.performed += ctx => skipAnim = true;
            StartCoroutine(TextAppearingAnimationCoroutine(positionChangeY, timeAnimation));
        }

        IEnumerator TextAppearingAnimationCoroutine(float finalPositionChangeY, float timeOfAnimSeg)
        {
            //Set title alpha in 0, and set deltas
            titleLabel.alpha = 0;
            float deltaTimer = 1;
            float timeOfAnim = timeOfAnimSeg;

            float deltaPosition = (finalPositionChangeY * deltaTimer) / timeOfAnim;
            float deltaAlpha = (255 * deltaTimer) / timeOfAnim;

            //Set alpha of playButton cover in 1
            var tempColor = playButtonCover.color;
            tempColor.a = 1f;
            playButtonCover.color = tempColor;

            //Force mesh update and get text info
            titleLabel.ForceMeshUpdate();
            TMP_TextInfo textInfo = titleLabel.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                float timer = 0;
                while (timer < timeOfAnim)
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
                        destinationVertices[vertexIndex + k] += new Vector3(0, deltaPosition);
                        if (timer + deltaTimer > timeOfAnim)
                            newVertexColors[vertexIndex + k].a = 255;
                        else
                            newVertexColors[vertexIndex + k].a += (byte)deltaAlpha;
                    }

                    textInfo.meshInfo[materialIndex].mesh.vertices = textInfo.meshInfo[materialIndex].vertices;
                    titleLabel.UpdateGeometry(textInfo.meshInfo[materialIndex].mesh, materialIndex);
                    titleLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    timer += deltaTimer;
                    yield return null;
                }

                if (skipAnim)
                {
                    timeOfAnim = 5;
                    deltaPosition = (finalPositionChangeY * deltaTimer) / timeOfAnim;
                    deltaAlpha = (255 * deltaTimer) / timeOfAnim;

                    skipAnim = false;
                }
                yield return new WaitForSeconds(0.05f);
            }

            LeanTween.value(1, 0, (timeOfAnim / timeOfAnimSeg) * 2)
                .setOnUpdate((float value) =>
                  {
                      tempColor = playButtonCover.color;
                      tempColor.a = value;
                      playButtonCover.color = tempColor;
                  })
                .setOnComplete(() =>
                {
                    playButtonCover.gameObject.SetActive(false);
                    a_onEndAnimation?.Invoke();
                });
        }
        #endregion ----Methods----
    }

}