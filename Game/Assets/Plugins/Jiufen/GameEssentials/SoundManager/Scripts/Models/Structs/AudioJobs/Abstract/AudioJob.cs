using System;
using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{
    public abstract class AudioJob
    {
        public string key;
        public AudioAction action;
        public AudioJobOptions options;

        public AudioJob(string key, AudioJobOptions audioJobExtras = null)
        {
            this.key = key;
            this.options = audioJobExtras != null ? audioJobExtras : new AudioJobOptions();
        }

        public virtual IEnumerator RunAudioJob(AudioTrack track, AudioClip clip)
        {
            if (options.delay != null) yield return options.delay;
            track.audioSource.clip = clip;
        }

        private protected virtual IEnumerator FadeAudio(AudioTrack track, float durationFade, float initialVolume, float targetVolume, Action onFinishFadeCallback = null)
        {
            float timerFade = 0.0f;

            while (timerFade <= durationFade)
            {
                track.audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, timerFade / durationFade);
                timerFade += Time.deltaTime;
                yield return null;
            }

            track.audioSource.volume = targetVolume;
            onFinishFadeCallback?.Invoke();
        }
    }

}