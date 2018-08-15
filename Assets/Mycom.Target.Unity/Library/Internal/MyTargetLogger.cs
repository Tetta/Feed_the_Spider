using System;
using UnityEngine;

namespace Mycom.Target.Unity.Internal
{
    /// <summary>
    /// Class for library logging
    /// </summary>
    public static class MyTargetLogger
    {
#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MTRGDebugLog(String message);
#endif

        private static readonly String Tag = "[mytarget.unity]: ";

        /// <summary>
        /// Write message to log
        /// </summary>
        public static void Log(String message)
        {
            if(String.IsNullOrEmpty(message))
            {
                return;
            }
            
#if UNITY_IOS
            MTRGDebugLog(Tag + message);
#else
            Debug.Log(Tag + message);
#endif
        }
    }
}