using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiufen.Audio
{
    public class AudioJobsController : MonoBehaviour
    {

        #region 1.Fields
        public Hashtable m_jobsTable;
        #endregion 1.Fields

        #region 2.Methods
        #region 2.0.Init
        public void Init()
        {
            DontDestroyOnLoad(this);
            m_jobsTable = new Hashtable();
        }
        #endregion 2.0.Init

        #region 2.1.Add & Execute Jobs
        public void AddJob(AudioJob _audioJob)
        {
            RemoveConflictingJobs(_audioJob.key);

            //Add Job
            Coroutine jobRunner = StartCoroutine(RunAudioJob(_audioJob));
            m_jobsTable.Add(_audioJob.key, jobRunner);
            AudioLogger.Log($"Starting Job {_audioJob.key} with action: {_audioJob.action}");
            AudioLogger.Log($"Job count: {m_jobsTable.Count}");
        }

        private IEnumerator RunAudioJob(AudioJob _audioJob)
        {
            AudioTrack track = (AudioTrack)AudioManager.m_audioTable[_audioJob.key];
            AudioClip clip = GetAudioClipFromAudioTrack(_audioJob.key, track);
            track.audioSource.loop = _audioJob.options.loop;
            track.audioSource.volume = _audioJob.options.volume;

            yield return _audioJob.RunAudioJob(track, clip);

            //To Ensuser that the job was added first
            yield return new WaitForFixedUpdate();
            m_jobsTable.Remove(_audioJob.key);
        }

        private AudioClip GetAudioClipFromAudioTrack(string key, AudioTrack track)
        {
            foreach (AudioObject obj in track.audioObjects)
            {
                if (obj.key == key)
                {
                    return obj.audioClip;
                }
            }
            return null;
        }
        #endregion 2.1.Add & Execute Jobs

        #region 2.2.Remove and Dispose Jobs
        private void RemoveConflictingJobs(string key)
        {
            if (m_jobsTable.Contains(key))
            {
                RemoveJob(key);
            }

            string keyConflict = "";

            foreach (DictionaryEntry job in m_jobsTable)
            {
                AudioTrack currentTrack = (AudioTrack)AudioManager.m_audioTable[job.Key];
                AudioTrack newTrack = (AudioTrack)AudioManager.m_audioTable[key];
                if (currentTrack.audioSource == newTrack.audioSource)
                {
                    AudioLogger.LogError($"You have the same audio source for different audioTypes. Please check audioType [{job.Key}] and [{key}] ");
                    keyConflict = key;
                }
            }
            if (!String.IsNullOrEmpty(keyConflict))
            {
                RemoveJob(keyConflict);
            }
        }

        private void RemoveJob(string key)
        {
            if (!m_jobsTable.ContainsKey(key))
            {
                AudioLogger.LogError($"You are trying to stop a job that doesn't exist in the jobsTable: {key}");
                return;
            }
            Coroutine runningJob = (Coroutine)m_jobsTable[key];
            StopCoroutine(runningJob);
            m_jobsTable.Remove(key);
        }
        public void Dispose()
        {
            foreach (DictionaryEntry entry in m_jobsTable)
            {
                Coroutine job = (Coroutine)entry.Value;
                StopCoroutine(job);
            }
        }

        #endregion 2.2.Remove and Dispose Jobs
        #endregion 2.Methods
    }
}
