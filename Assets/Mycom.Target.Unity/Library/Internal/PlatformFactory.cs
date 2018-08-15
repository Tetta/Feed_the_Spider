using System;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Internal
{
    internal static class PlatformFactory
    {
        internal static IDebugModeProxy CreateDebugModeProxy()
        {
#if UNITY_ANDROID
            return new Implementations.Android.DebugModeProxy();
#elif UNITY_IOS
            return new Implementations.iOS.DebugModeProxy();
#else
            return null;
#endif
        }

        internal static IDispatcher CreateDispatcher()
        {
#if UNITY_ANDROID
            return new Implementations.Android.Dispatcher();
#elif UNITY_IOS
            return new Implementations.iOS.Dispatcher();
#else
            return null;
#endif
        }

        internal static IInterstitialAdProxy CreateInterstitial(UInt32 slotId)
        {
#if UNITY_ANDROID
            return new Implementations.Android.InterstitialAdProxy(slotId);
#elif UNITY_IOS
            return new Implementations.iOS.InterstitialAdProxy(slotId);
#else
            return null;
#endif
        }

        internal static IMyTargetViewProxy CreateMyTargetControl(UInt32 slotId,
                                                                 MyTargetView.AdSize adSize,
                                                                 Boolean isRefreshAd)
        {
#if UNITY_ANDROID
            return new Implementations.Android.MyTargetViewProxy(slotId, adSize, isRefreshAd);
#elif UNITY_IOS
            return new Implementations.iOS.MyTargetViewProxy(slotId, adSize, isRefreshAd);
#else
            return null;
#endif
        }
    }
}