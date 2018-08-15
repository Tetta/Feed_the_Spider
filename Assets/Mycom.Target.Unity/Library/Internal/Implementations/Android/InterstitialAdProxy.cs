#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal sealed class InterstitialAdProxy : IInterstitialAdProxy
    {
        private const String ClassName = "com.my.target.ads.InterstitialAd";
        private const String MethodDestroy = "destroy";
        private const String MethodDismissDialog = "dismissDialog";
        private const String MethodGetCustomParams = "getCustomParams";
        private const String MethodLoad = "load";
        private const String MethodSetListener = "setListener";
        private const String MethodShow = "show";
        private const String MethodShowDialog = "showDialog";
        private const String MethodSetTrackingEnvironmentEnabled = "setTrackingEnvironmentEnabled";
        private const String MethodSetTrackingLocationEnabled = "setTrackingLocationEnabled";

        private readonly AndroidJavaObject _interstitialAdObject;
        private Boolean _isDisposed;

        public InterstitialAdProxy(UInt32 slotId)
        {
            var currentActivity = PlatformHelper.GetCurrentActivity();

            _interstitialAdObject = new AndroidJavaObject(ClassName,
                                                          (Int32)slotId,
                                                          currentActivity);

            _interstitialAdObject.Call(MethodSetListener, new InterstitialAdListener(this));

            CustomParamsProxy = new CustomParamsProxy(_interstitialAdObject.Call<AndroidJavaObject>(MethodGetCustomParams));
        }

        ~InterstitialAdProxy()
        {
            Dispose();
        }

        private void OnAdClicked()
        {
            var handler = AdClicked;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdDismissed()
        {
            var handler = AdDismissed;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdDisplayed()
        {
            var handler = AdDisplayed;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdLoadCompleted()
        {
            var handler = AdLoadCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdLoadFailed(String error)
        {
            var handler = AdLoadFailed;
            if (handler != null)
            {
                handler(error);
            }
        }

        private void OnAdVideoCompleted()
        {
            var handler = AdVideoCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        public event Action AdClicked;

        public event Action AdLoadCompleted;

        public event Action<String> AdLoadFailed;

        public void Load()
        {
            if (_isDisposed)
            {
                return;
            }
            _interstitialAdObject.Call(MethodLoad);
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _interstitialAdObject.Call(MethodDestroy);

            _interstitialAdObject.Dispose();

            CustomParamsProxy.Dispose();

            GC.SuppressFinalize(this);
        }

        public event Action AdDismissed;

        public event Action AdDisplayed;

        public event Action AdVideoCompleted;

        public void Dismiss()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _interstitialAdObject.Call(MethodDismissDialog));
        }

        public void Show()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _interstitialAdObject.Call(MethodShow));
        }

        public void ShowDialog()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _interstitialAdObject.Call(MethodShowDialog));
        }

        public void SetTrackingEnvironmentEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _interstitialAdObject.Call(MethodSetTrackingEnvironmentEnabled, value);
        }

        public void SetTrackingLocationEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _interstitialAdObject.Call(MethodSetTrackingLocationEnabled, value);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private sealed class InterstitialAdListener : AndroidJavaProxy
        {
            private const String ListenerClassName = "com.my.target.ads.InterstitialAd$InterstitialAdListener";

            private readonly InterstitialAdProxy _interstitialAdProxy;

            public InterstitialAdListener(InterstitialAdProxy interstitialAdProxy) : base(ListenerClassName)
            {
                _interstitialAdProxy = interstitialAdProxy;
            }

            public void onClick(AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdClicked();
            }

            public void onDismiss(AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdDismissed();
            }

            public void onDisplay(AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdDisplayed();
            }

            public void onLoad(AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdLoadCompleted();
            }

            public void onNoAd(String error, AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdLoadFailed(error);
            }

            public void onVideoCompleted(AndroidJavaObject o)
            {
                _interstitialAdProxy.OnAdVideoCompleted();
            }
        }
    }
}

#endif