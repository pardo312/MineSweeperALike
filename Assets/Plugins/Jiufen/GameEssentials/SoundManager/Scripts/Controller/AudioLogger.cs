using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiufen.Audio
{
    public static class AudioLogger
    {
        public static bool m_debug;

        public static void Log(string message)
        {
            if (!m_debug) return;
            Debug.Log($"<color=blue>Audio Controller message:</color> {message}");
        }
        public  static void LogError(string message)
        {
            if (!m_debug) return;
            Debug.Log($"<color=red>Audio Controller error message:</color> {message}");
        }
    }
}
