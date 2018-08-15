#if UNITY_ANDROID

using System;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal sealed class Dispatcher : MonoBehaviour, IDispatcher
    {
        private readonly IDispatcher _unityDispatcher = UnityDispatcher.GetInstance();

        public void Perform(Action action)
        {
            _unityDispatcher.Perform(() => PlatformHelper.RunInUiThread(action));
        }
    }
}

#endif