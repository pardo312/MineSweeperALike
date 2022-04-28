using System;
using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{
    public class AudioJobStop : AudioJob
    {
        public AudioJobStop(string key, AudioJobOptions audioJobExtras = null) : base(key, audioJobExtras)
        {
            action = AudioAction.STOP;
        }

        public override IEnumerator RunAudioJob(AudioTrack track, AudioClip clip)
        {
            yield return base.RunAudioJob(track, clip);

            if (!options.fadeOut.fade)
            {
                track.audioSource.Stop();
            }
            else
            {
                yield return FadeAudio(track, options.fadeOut.fadeDuration, track.audioSource.volume, 0);
            }
        }

        private protected override IEnumerator FadeAudio(AudioTrack track, float durationFade, float initialVolume, float targetVolume,Action onFinishFadeCallback = null)
        {
            yield return base.FadeAudio(track, durationFade, initialVolume, targetVolume, () =>
            {
                track.audioSource.Stop();
            });
        }
    }
}