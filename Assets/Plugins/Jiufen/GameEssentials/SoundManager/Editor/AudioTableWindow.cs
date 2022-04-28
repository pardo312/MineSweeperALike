using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jiufen.Audio.Editor
{
    public class AudioTableWindow : EditorWindow
    {
        #region Singleton And Events
        public const string AUDIO_TRACK_PATH = "Assets/Resources/Scriptables/AudioManager/SFXAudioTrack.asset";
        private const int AUDIOTABLE_KEY_COLUM_WIDTH = 120;
        private int _currentPiece = 0;
        private int _previousPiece = 0;

        public List<string> _piecesNames = new List<string>();
        public Action _onChangePiece;
        public AudioTrack m_audioTableScriptable;

        private void Awake()
        {
            if (m_audioTableScriptable == null)
            {
                string[] scriptableList = AssetDatabase.FindAssets("t:" + typeof(AudioTrack));
                if (scriptableList.Length == 0)
                {
                    if (!AssetDatabase.LoadAssetAtPath(AUDIO_TRACK_PATH, typeof(AudioTrack)))
                    {
                        Debug.Log("Create");
                        CreateScriptable(AUDIO_TRACK_PATH);
                    }
                    m_audioTableScriptable = (AudioTrack)EditorGUIUtility.Load(AUDIO_TRACK_PATH);
                }
                else
                {
                    m_audioTableScriptable = (AudioTrack)EditorGUIUtility.Load(AssetDatabase.GUIDToAssetPath(scriptableList[0]));
                }
            }
        }

        private void CreateFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Scriptables"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Scriptables");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Scriptables/AudioManager"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Scriptables", "AudioManager");
            }
        }

        private AudioTrack CreateScriptable(string path)
        {
            CreateFolders();

            AudioTrack asset = ScriptableObject.CreateInstance<AudioTrack>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        #endregion

        [MenuItem("Jiufen/Audio/AudioTable")]
        public static void ShowWindow()
        {
            GetWindow<AudioTableWindow>(false, "AudioTable", true);
        }

        void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.fontSize = 16;

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("Audio Script Table", guiStyle);
            SelectAudioTrack();

            if (m_audioTableScriptable != null)
            {
                SetAudioTrack();
            }
        }

        private void SelectAudioTrack()
        {
            m_audioTableScriptable = (AudioTrack)EditorGUILayout.ObjectField(m_audioTableScriptable, typeof(AudioTrack));
            string path = AssetDatabase.GenerateUniqueAssetPath(AUDIO_TRACK_PATH);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                m_audioTableScriptable = CreateScriptable(path);
            }
            GUILayout.EndHorizontal();

        }

        private void SetAudioTrack()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.ExpandWidth(true), GUILayout.Height(5));

            //AUDIO TABLE
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("", GUILayout.Height(5), GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Key", GUILayout.Width(AUDIOTABLE_KEY_COLUM_WIDTH));
            EditorGUILayout.LabelField("AudioClip");
            GUILayout.EndHorizontal();
            for (int i = 0; i < m_audioTableScriptable.audioObjects.Count; i++)
            {
                if (m_audioTableScriptable.audioObjects == null)
                {
                    m_audioTableScriptable.audioObjects.RemoveAt(i);
                    continue;
                }
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                m_audioTableScriptable.audioObjects[i].key = EditorGUILayout.TextField(m_audioTableScriptable.audioObjects[i].key, GUILayout.Width(AUDIOTABLE_KEY_COLUM_WIDTH)).ToUpper();
                m_audioTableScriptable.audioObjects[i].audioClip = (AudioClip)EditorGUILayout.ObjectField(m_audioTableScriptable.audioObjects[i].audioClip, typeof(AudioClip));
                GUILayout.EndHorizontal();
            }

            //BUTTON
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                m_audioTableScriptable.audioObjects.Add(new AudioObject() { key = "", audioClip = null });
            }
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
            {
                m_audioTableScriptable.audioObjects.RemoveAt(m_audioTableScriptable.audioObjects.Count - 1);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(m_audioTableScriptable);
        }

    }
}
