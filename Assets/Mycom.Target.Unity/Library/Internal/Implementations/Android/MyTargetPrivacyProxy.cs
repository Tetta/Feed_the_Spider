#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Mycom.Target.Unity.Internal
{
    internal static partial class MyTargetPrivacyProxy
    {
        private const String ClassName = "com.my.target.common.MyTargetPrivacy";

        static partial void GetUserConsent(ref Boolean isUserConsent)
        {
            const String methodName = "isUserConsent";
            using (var javaClass = new AndroidJavaClass(ClassName))
            {
                isUserConsent = javaClass.CallStatic<Boolean>(methodName);
            }
        }

        static partial void SetUserConsent(Boolean userConsent)
        {
            const String methodName = "setUserConsent";
            using (var javaClass = new AndroidJavaClass(ClassName))
            {
                javaClass.CallStatic(methodName, userConsent);
            }
        }

        static partial void GetUserAgeRestricted(ref Boolean isUserAgeRestricted)
        {
            const String methodName = "isUserAgeRestricted";
            using (var javaClass = new AndroidJavaClass(ClassName))
            {
                isUserAgeRestricted = javaClass.CallStatic<Boolean>(methodName);
            }
        }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted)
        {
            const String methodName = "setUserAgeRestricted";
            using (var javaClass = new AndroidJavaClass(ClassName))
            {
                javaClass.CallStatic(methodName, userAgeRestricted);
            }
        }
    }
}

#endif