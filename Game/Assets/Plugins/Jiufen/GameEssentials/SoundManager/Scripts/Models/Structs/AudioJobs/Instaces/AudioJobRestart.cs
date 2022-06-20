using System.Collections;
using UnityEngine;

namespace Jiufen.Audio
{
    public class AudioJobRestart : AudioJob
    {
        public AudioJobRestart(string key, AudioJobOptions audioJobExtras = null) : base(key, audioJobExtras)
        {
            action = AudioAction.RESTART;
        }

        public override IEnumerator RunAudioJob(AudioTrack track, AudioClip clip)
        {
            yield return base.RunAudioJob(track, clip);

            track.audioSource.Stop();
            track.audioSource.Play();
        }
    }

}