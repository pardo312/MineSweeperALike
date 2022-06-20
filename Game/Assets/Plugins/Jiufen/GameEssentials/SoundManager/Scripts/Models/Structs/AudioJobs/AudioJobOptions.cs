using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{
    public class AudioJobOptions
    {
        public AudioFadeInfo fadeIn;
        public AudioFadeInfo fadeOut;
        public bool loop;
        public float volume;
        public WaitForSeconds delay;

        public AudioJobOptions(AudioFadeInfo fadeIn = null, AudioFadeInfo fadeOut = null, bool loop = false, float volumen = 1, float delay = 0f)
        {
            this.fadeIn = fadeIn != null ? fadeIn : new AudioFadeInfo(false, 0);
            this.fadeOut = fadeOut != null ? fadeOut : new AudioFadeInfo(false, 0);
            this.loop = loop;
            this.volume = volume >= 1 ? 1 : volume <= 0 ? 0 : volumen;
            this.delay = delay > 0f ? new WaitForSeconds(delay) : null;
        }
    }
}