using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public abstract class AbstractAd : IDisposable
    {
        private static readonly IDebugModeProxy DebugModeProxy = PlatformFactory.CreateDebugModeProxy();

        public static Boolean IsDebugMode
        {
            get { return DebugModeProxy.IsDebugMode; }
            set { DebugModeProxy.IsDebugMode = value; }
        }

        internal readonly IDispatcher PlatformDispatcher = PlatformFactory.CreateDispatcher();

        internal readonly IDispatcher UnityDispatcher = Internal.UnityDispatcher.GetInstance();

        private readonly CustomParams _customParams = new CustomParams();

        private Boolean _trackingEnvironmentEnabled = true;
        private Boolean _trackingLocationEnabled = true;

        public Boolean TrackingEnvironmentEnabled
        {
            get { return _trackingEnvironmentEnabled; }
            set
            {
                _trackingEnvironmentEnabled = value;
                SetTrackingEnvironmentEnabled(value);
            }
        }

        public Boolean TrackingLocationEnabled
        {
            get { return _trackingLocationEnabled; }
            set
            {
                _trackingLocationEnabled = value;
                SetTrackingLocationEnabled(value);
            }
        }

        public CustomParams CustomParams
        {
            get { return _customParams; }
        }

        public abstract void Load();

        protected abstract void SetTrackingEnvironmentEnabled(Boolean value);
        protected abstract void SetTrackingLocationEnabled(Boolean value);

        protected void OnAdClicked()
        {
            UnityDispatcher.Perform(OnAdClickedImpl);
        }

        protected void OnAdLoadCompleted()
        {
            UnityDispatcher.Perform(OnAdLoadCompletedImpl);
        }

        protected void OnAdLoadFailed(String error)
        {
            UnityDispatcher.Perform(() => OnAdLoadFailedImpl(error));
        }

        private void OnAdClickedImpl()
        {
            var handler = AdClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnAdLoadCompletedImpl()
        {
            var handler = AdLoadCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnAdLoadFailedImpl(String error)
        {
            var handler = AdLoadFailed;
            if (handler != null)
            {
                handler(this, new ErrorEventArgs(error));
            }
        }

        public event EventHandler AdClicked;

        public event EventHandler AdLoadCompleted;

        public event EventHandler<ErrorEventArgs> AdLoadFailed;

        public abstract void Dispose();
    }
}