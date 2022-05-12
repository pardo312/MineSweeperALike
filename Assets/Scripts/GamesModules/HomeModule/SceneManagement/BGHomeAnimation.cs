using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JiufenGames.MineSweeperAlike.HomeModule
{
    public class BGHomeAnimation : MonoBehaviour
    {
        #region ----Fields----
        public Image bgPrefab;
        public Transform parentBG;
        [Range(1, 16)]
        public int numberOfLines = 4;
        [Range(.3f, 3)]
        public float distanceBetweenBars = 1;
        [Range(15, 120)]
        public float speed = 15;

        private List<Image> bgList = new List<Image>();
        private float scaleFactor;
        private float deltaA = 2.5f;
        #endregion ----Fields----

        #region ----Methods----
        #region <<<Init>>>
        public void Init()
        {
            scaleFactor = .4f / numberOfLines;
            for (int i = 0; i < numberOfLines; i++)
            {
                Image newBg = Instantiate(bgPrefab, parentBG);
                newBg.name = i.ToString();

                //Scale
                newBg.transform.localScale += new Vector3(.05f - (scaleFactor * distanceBetweenBars * i), .025f - (scaleFactor * distanceBetweenBars * .5f * i), 0);

                //Alpha
                Image imageBg = newBg;
                Color temp = imageBg.color;
                temp.a -= (scaleFactor * deltaA * i);
                imageBg.color = temp;

                //Add to list
                bgList.Add(newBg);
            }
        }
        #endregion <<<Init>>>

        #region <<<OnTick>>>
        public void FixedUpdate()
        {
            OnBgAnimationTick();
        }

        public void OnBgAnimationTick()
        {
            float scaleDelta = (scaleFactor) / (speed * 2);
            for (int i = 0; i < bgList.Count; i++)
            {

                //Scale
                bgList[i].transform.localScale -= new Vector3(scaleDelta * distanceBetweenBars, scaleDelta * (distanceBetweenBars * .5f), 0);

                //Alpha
                Image imageBg = bgList[i];
                Color temp = imageBg.color;
                temp.a -= (scaleDelta * deltaA);
                imageBg.color = temp;

                if (imageBg.color.a <= 0)
                {
                    //Scale
                    bgList[i].transform.localScale = new Vector3(1.05f, 1.025f, 1);

                    //Alpha
                    temp.a = 1;
                    imageBg.color = temp;
                }
            }
        }
        #endregion <<<OnTick>>>
        #endregion ----Methods----
    }
}