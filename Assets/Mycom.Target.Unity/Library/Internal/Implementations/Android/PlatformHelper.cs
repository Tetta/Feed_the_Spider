#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal static class PlatformHelper
    {
        private const String FieldCurrentActivity = "currentActivity";
        private const String FieldDensity = "density";
        private const String MethodGetApplicationContext = "getApplicationContext";
        private const String MethodGetDisplaymetrics = "getDisplayMetrics";
        private const String MethodGetResources = "getResources";
        private const String MethodRunOnUiThread = "runOnUiThread";
        private const String UnityPlayerClassName = "com.unity3d.player.UnityPlayer";

        private static readonly HashSet<AndroidJavaRunnable> AndroidJavaRunnables = new HashSet<AndroidJavaRunnable>();

        private static Single? _density;

        internal static AndroidJavaObject GetApplicationContext()
        {
            return GetCurrentActivity().Call<AndroidJavaObject>(MethodGetApplicationContext);
        }

        internal static AndroidJavaObject GetCurrentActivity()
        {
            return new AndroidJavaClass(UnityPlayerClassName).GetStatic<AndroidJavaObject>(FieldCurrentActivity);
        }

        internal static AndroidJavaObject CreateJavaString(String value)
        {
            return value == null ? null : new AndroidJavaObject("java.lang.String", value);
        }

        internal static AndroidJavaObject CreateJavaStringArray(String[] values)
        {
            if (values == null)
            {
                return null;
            }

            using (var arrayClass = new AndroidJavaClass("java.lang.reflect.Array"))
            {
                using (var stringClass = new AndroidJavaClass("java.lang.String"))
                {
                    var arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance", stringClass, values.Length);
                    for (var i = 0; i < values.Length; ++i)
                    {
                        using (var stringValue = new AndroidJavaObject("java.lang.String", values[i]))
                        {
                            arrayClass.CallStatic("set", arrayObject, i, stringValue);
                        }
                    }
                    return arrayObject;
                }
            }
        }

        internal static Single GetDensity()
        {
            if (!_density.HasValue)
            {
                var context = GetApplicationContext();
                var resources = context.Call<AndroidJavaObject>(MethodGetResources);
                var displayMetrics = resources.Call<AndroidJavaObject>(MethodGetDisplaymetrics);
                _density = displayMetrics.Get<Single>(FieldDensity);
            }

            return _density.Value;
        }

        internal static Int32 GetInDp(Single value)
        {
            return (Int32) (GetDensity() * value);
        }

        internal static Int32 GetInDp(Double value)
        {
            return (Int32) (GetDensity() * value);
        }

        internal static void RunInUiThread(Action action)
        {
            AndroidJavaRunnable javaRunnable = null;
            javaRunnable = () =>
                           {
                               action();
                               AndroidJavaRunnables.Remove(javaRunnable);
                           };

            if (AndroidJavaRunnables.Add(javaRunnable))
            {
                GetCurrentActivity().Call(MethodRunOnUiThread, javaRunnable);
            }
        }
    }
}

#endif