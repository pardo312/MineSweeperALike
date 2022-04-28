using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{

    public class AudioJobStart : AudioJob
    {
        public AudioJobStart(string key, AudioJobOptions audioJobExtras = null) : base(key, audioJobExtras)
        {
            action = AudioAction.START;
        }

        public override IEnumerator RunAudioJob(AudioTrack track, AudioClip clip)
        {
            yield return base.RunAudioJob(track, clip);

            track.audioSource.volume = 0;
            track.audioSource.Play();
            yield return FadeAudio(track, options.fadeIn.fadeDuration, 0, 1);
        }
    }

}