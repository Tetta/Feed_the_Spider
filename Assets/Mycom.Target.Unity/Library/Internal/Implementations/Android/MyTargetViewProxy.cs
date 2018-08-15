#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal sealed class MyTargetViewProxy : IMyTargetViewProxy
    {
        private const String ClassName = "com.my.target.ads.MyTargetView";
        private const String ClassNameFrameLayouParams = "android.widget.FrameLayout$LayoutParams";
        private const String MethodGetCustomParams = "getCustomParams";
        private const String MethodInit = "init";
        private const String MethodLoad = "load";
        private const String MethodNameRequestLayout = "requestLayout";
        private const String MethodPause = "pause";
        private const String MethodResume = "resume";
        private const String MethodSetListener = "setListener";
        private const String MethodStart = "start";
        private const String MethodStop = "stop";
        private const String MethoSetLayoutParams = "setLayoutParams";
        private const String MethodSetTrackingEnvironmentEnabled = "setTrackingEnvironmentEnabled";
        private const String MethodSetTrackingLocationEnabled = "setTrackingLocationEnabled";

        private readonly AndroidJavaObject _layoutParams;
        private readonly AndroidJavaObject _myTargetViewObject;

        private Boolean _isDisposed;
        private Boolean _isShown;

        public MyTargetViewProxy(UInt32 slotId,
                                 MyTargetView.AdSize adSize,
                                 Boolean isRefreshAd)
        {
            var currentActivity = PlatformHelper.GetCurrentActivity();

            _myTargetViewObject = new AndroidJavaObject(ClassName, currentActivity);

            _layoutParams = new AndroidJavaObject(ClassNameFrameLayouParams, 0, 0);

            _myTargetViewObject.Call(MethodSetListener, new MyTargetViewListener(this));

            _myTargetViewObject.Call(MethodInit,
                                     (Int32) slotId,
                                     (Int32) adSize,
                                     isRefreshAd);

            _myTargetViewObject.Call(MethoSetLayoutParams, _layoutParams);

            CustomParamsProxy = new CustomParamsProxy(_myTargetViewObject.Call<AndroidJavaObject>(MethodGetCustomParams));
        }

        ~MyTargetViewProxy()
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

        private void OnAdLoadCompleted()
        {
            var handler = AdLoadCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnAdLoadFailed(String obj)
        {
            var handler = AdLoadFailed;
            if (handler != null)
            {
                handler(obj);
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
            _myTargetViewObject.Call(MethodLoad);
        }

        public ICustomParamsProxy CustomParamsProxy { get; private set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _myTargetViewObject.Call<AndroidJavaObject>("getParent")
                                                                .Call("removeView", _myTargetViewObject);

                                             _myTargetViewObject.Call("destroy");

                                             _myTargetViewObject.Dispose();

                                             CustomParamsProxy.Dispose();
                                         });

            GC.SuppressFinalize(this);
        }

        public void Pause()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _myTargetViewObject.Call(MethodPause));
        }

        public void Resume()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _myTargetViewObject.Call(MethodResume));
        }

        public void SetHeight(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("height", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetWidth(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("width", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetX(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("leftMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void SetY(Double value)
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             _layoutParams.Set("topMargin", PlatformHelper.GetInDp(value));
                                             _myTargetViewObject.Call(MethodNameRequestLayout);
                                         });
        }

        public void Start()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_isShown)
            {
                return;
            }

            _isShown = true;

            PlatformHelper.RunInUiThread(() =>
                                         {
                                             var activity = PlatformHelper.GetCurrentActivity();

                                             var r = new AndroidJavaClass("android.R$id");

                                             var contentId = r.GetStatic<Int32>("content");

                                             activity.Call<AndroidJavaObject>("findViewById", contentId)
                                                     .Call("addView", _myTargetViewObject);

                                             _myTargetViewObject.Call(MethodStart);
                                         });
        }

        public void Stop()
        {
            if (_isDisposed)
            {
                return;
            }

            PlatformHelper.RunInUiThread(() => _myTargetViewObject.Call(MethodStop));
        }

        public void SetTrackingEnvironmentEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _myTargetViewObject.Call(MethodSetTrackingEnvironmentEnabled, value);
        }

        public void SetTrackingLocationEnabled(Boolean value)
        {
            if (_isDisposed)
            {
                return;
            }

            _myTargetViewObject.Call(MethodSetTrackingLocationEnabled, value);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private sealed class MyTargetViewListener : AndroidJavaProxy
        {
            private const String ListenerClassName = "com.my.target.ads.MyTargetView$MyTargetViewListener";

            private readonly MyTargetViewProxy _myTargetViewProxy;

            public MyTargetViewListener(MyTargetViewProxy myTargetViewProxy) : base(ListenerClassName)
            {
                _myTargetViewProxy = myTargetViewProxy;
            }

            public void onClick(AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdClicked();
            }

            public void onLoad(AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdLoadCompleted();
            }

            public void onNoAd(String error, AndroidJavaObject o)
            {
                _myTargetViewProxy.OnAdLoadFailed(error);
            }
        }
    }
}

#endif