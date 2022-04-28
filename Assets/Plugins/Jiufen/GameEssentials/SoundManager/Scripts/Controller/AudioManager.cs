using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiufen.Audio
{
    public static class AudioManager
    {

        #region 1.Fields
        public static Hashtable m_audioTable;
        private static List<AudioTrack> m_audioTracks = new List<AudioTrack>();
        [SerializeField] private static bool debug;

        private static AudioJobsController m_audioJobsController;
        #endregion 1.Fields

        #region 2.Methods
        #region 2.1.UnityEvents
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            Configure();
        }

        #endregion 2.1.UnityEvents

        #region 2.2.Audio Behaviours
        public static void PlayAudio(string key, AudioJobOptions options = null)
        {
            m_audioJobsController.AddJob(new AudioJobStart(key, options));
        }
        public static void StopAudio(string key, AudioJobOptions options = null)
        {
            m_audioJobsController.AddJob(new AudioJobStop(key, options));
        }
        public static void RestartAudio(string key, AudioJobOptions options = null)
        {
            m_audioJobsController.AddJob(new AudioJobRestart(key, options));
        }
        #endregion 2.2.Audio Behaviours

        #region 2.3.Helpers
        private static void Configure()
        {
            //Create jobsController 
            GameObject audioJobsGO = new GameObject();
            audioJobsGO.name = "AudioJobsController";
            m_audioJobsController = audioJobsGO.AddComponent<AudioJobsController>();
            m_audioJobsController.Init();

            //Set Logger
            AudioLogger.m_debug = debug;

            //Populate AudioTracks
            UnityEngine.Object[] audioTrackObjs = Resources.LoadAll("/", typeof(AudioTrack));
            foreach (UnityEngine.Object obj in audioTrackObjs)
            {
                AudioTrack track = (AudioTrack)obj;
                GameObject audioSourceGO = new GameObject();
                audioSourceGO.name = "AudioSource";
                audioSourceGO.transform.parent = audioJobsGO.transform;

                track.audioSource = audioSourceGO.AddComponent<AudioSource>();
                m_audioTracks.Add(track);
            }

            //Create and populate AudioTable
            m_audioTable = new Hashtable();
            GenerateAudioTable();

        }

        private static void GenerateAudioTable()
        {
            foreach (AudioTrack tracks in m_audioTracks)
            {
                foreach (AudioObject audioObj in tracks.audioObjects)
                {
                    if (m_audioTable.Contains(audioObj.key))
                    {
                        AudioLogger.LogError($"AudioTable is registering an already registered audio type: {audioObj.key}");
                    }
                    else
                    {
                        m_audioTable.Add(audioObj.key, tracks);
                        AudioLogger.Log($"Audio type {audioObj.key} has been registered in the audioTable");
                    }
                }
            }

        }

        #endregion 2.3.Helpers
        #endregion 2.Methods
    }
}

