#if UNITY_ANDROID
using System;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal sealed class DebugModeProxy : IDebugModeProxy
    {
        private const String ClassName = "com.my.target.ads.InterstitialAd";
        private const String MethodSetDebugMode = "setDebugMode";

        private Boolean _currentValue;

        public Boolean IsDebugMode
        {
            get
            {
                lock (this)
                {
                    return _currentValue;
                }
            }
            set
            {
                lock (this)
                {
                    if (_currentValue == value)
                    {
                        return;
                    }

                    using (var javaClass = new AndroidJavaClass(ClassName))
                    {
                        javaClass.CallStatic(MethodSetDebugMode, _currentValue = value);
                    }
                }
            }
        }
    }
}

#endif