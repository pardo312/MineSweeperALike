using Jiufen.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButtonSound : MonoBehaviour
{
    public string songName;

    public void PlaySound()
    {
        AudioManager.PlayAudio(songName);

    }
}
