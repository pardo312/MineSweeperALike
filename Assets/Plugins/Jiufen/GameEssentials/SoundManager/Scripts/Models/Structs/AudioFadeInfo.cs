using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{
    public class AudioFadeInfo
    {
        public static AudioFadeInfo defaultfadeInfo = new AudioFadeInfo(false, 0);

        public bool fade;
        public float fadeDuration;

        public AudioFadeInfo(bool fade, float fadeDuration)
        {
            this.fade = fade;
            this.fadeDuration = fadeDuration;
        }
    }
}