using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class InterstitialAd : AbstractAd
    {
        private readonly UInt32 _slotId;
        private readonly Object _syncRoot = new Object();
        
        private volatile IInterstitialAdProxy _interstitialAdProxy;

        public InterstitialAd(UInt32 slotId)
        {
            _slotId = slotId;
        }

        ~InterstitialAd()
        {
            Dispose();
        }

        public void Dismiss()
        {
            PlatformDispatcher.Perform(() =>
                                       {
                                           if (_interstitialAdProxy == null)
                                           {
                                               return;
                                           }

                                           lock (_syncRoot)
                                           {
                                               if (_interstitialAdProxy != null)
                                               {
                                                   _interstitialAdProxy.Dismiss();
                                               }
                                           }
                                       });
        }

        public override void Dispose()
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy == null)
                {
                    return;
                }

                var referenceCopy = _interstitialAdProxy;
                _interstitialAdProxy = null;

                PlatformDispatcher.Perform(referenceCopy.Dispose);

                referenceCopy.AdClicked -= OnAdClicked;
                referenceCopy.AdDismissed -= OnAdDismissed;
                referenceCopy.AdDisplayed -= OnAdDisplayed;
                referenceCopy.AdLoadCompleted -= OnAdLoadCompleted;
                referenceCopy.AdLoadFailed -= OnAdLoadFailed;
                referenceCopy.AdVideoCompleted -= OnAdVideoCompleted;

                GC.SuppressFinalize(this);
            }
        }

        public override void Load()
        {
            PlatformDispatcher.Perform(LoadImpl);
        }

        public void Show()
        {
            PlatformDispatcher.Perform(() => ShowImpl(false));
        }

        public void ShowDialog()
        {
            PlatformDispatcher.Perform(() => ShowImpl(true));
        }

        private void LoadImpl()
        {
            if (_interstitialAdProxy != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy != null)
                {
                    return;
                }

                _interstitialAdProxy = PlatformFactory.CreateInterstitial(_slotId);

                CustomParams.SetCustomParamsProxy(_interstitialAdProxy.CustomParamsProxy);

                _interstitialAdProxy.SetTrackingEnvironmentEnabled(TrackingEnvironmentEnabled);
                _interstitialAdProxy.SetTrackingLocationEnabled(TrackingLocationEnabled);

                _interstitialAdProxy.AdClicked += OnAdClicked;
                _interstitialAdProxy.AdDismissed += OnAdDismissed;
                _interstitialAdProxy.AdDisplayed += OnAdDisplayed;
                _interstitialAdProxy.AdLoadCompleted += OnAdLoadCompleted;
                _interstitialAdProxy.AdLoadFailed += OnAdLoadFailed;
                _interstitialAdProxy.AdVideoCompleted += OnAdVideoCompleted;

                _interstitialAdProxy.Load();
            }
        }

        private void OnAdDismissed()
        {
            UnityDispatcher.Perform(OnAdDismissedImpl);
        }

        private void OnAdDismissedImpl()
        {
            var handler = AdDismissed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnAdDisplayed()
        {
            UnityDispatcher.Perform(OnAdDisplayedImpl);
        }

        private void OnAdDisplayedImpl()
        {
            var handler = AdDisplayed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnAdVideoCompleted()
        {
            UnityDispatcher.Perform(OnAdVideoCompletedImpl);
        }

        private void OnAdVideoCompletedImpl()
        {
            var handler = AdVideoCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void ShowImpl(Boolean asDialog)
        {
            lock (_syncRoot)
            {
                if (_interstitialAdProxy == null)
                {
                    return;
                }

                if (asDialog)
                {
                    _interstitialAdProxy.ShowDialog();
                }
                else
                {
                    _interstitialAdProxy.Show();
                }
            }
        }

        protected override void SetTrackingEnvironmentEnabled(Boolean value)
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy == null)
                {
                    return;
                }

                _interstitialAdProxy.SetTrackingEnvironmentEnabled(value);
            }
        }

        protected override void SetTrackingLocationEnabled(Boolean value)
        {
            if (_interstitialAdProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAdProxy == null)
                {
                    return;
                }

                _interstitialAdProxy.SetTrackingLocationEnabled(value);
            }
        }

        public event EventHandler AdDismissed;

        public event EventHandler AdDisplayed;

        public event EventHandler AdVideoCompleted;
    }
}