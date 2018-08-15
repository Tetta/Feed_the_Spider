using System;
using Mycom.Target.Unity.Internal;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class MyTargetView : AbstractAd
    {
        public enum AdSize
        {
            Size320x50 = 0,
            Size300x250 = 1,
            Size728x90 = 2
        }

        private const Double Height300x250 = 250.0;
        private const Double Height320x50 = 50.0;
        private const Double Height728x90 = 90.0;
        private const Double Width320x50 = 320.0;
        private const Double Width300x250 = 300.0;
        private const Double Width728x90 = 728.0;

        private readonly AdSize _adSize;
        private readonly Boolean _isRefreshAd;
        private readonly UInt32 _slotId;
        private readonly Object _syncRoot = new Object();

        private Double _height;
        private volatile IMyTargetViewProxy _myTargetViewProxy;
        private Double _width;
        private Double _x;
        private Double _y;

        public Double Height
        {
            get { return _height; }
            set
            {
                _height = value;

                PlatformDispatcher.Perform(() =>
                                           {
                                               lock (_syncRoot)
                                               {
                                                   if (_myTargetViewProxy != null)
                                                   {
                                                       _myTargetViewProxy.SetHeight(value);
                                                   }
                                               }
                                           });
            }
        }

        public Double Width
        {
            get { return _width; }
            set
            {
                _width = value;

                PlatformDispatcher.Perform(() =>
                                           {
                                               lock (_syncRoot)
                                               {
                                                   if (_myTargetViewProxy != null)
                                                   {
                                                       _myTargetViewProxy.SetWidth(value);
                                                   }
                                               }
                                           });
            }
        }

        public Double X
        {
            get { return _x; }
            set
            {
                _x = value;

                PlatformDispatcher.Perform(() =>
                                           {
                                               lock (_syncRoot)
                                               {
                                                   if (_myTargetViewProxy != null)
                                                   {
                                                       _myTargetViewProxy.SetX(value);
                                                   }
                                               }
                                           });
            }
        }

        public Double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                PlatformDispatcher.Perform(() =>
                                           {
                                               lock (_syncRoot)
                                               {
                                                   if (_myTargetViewProxy != null)
                                                   {
                                                       _myTargetViewProxy.SetY(value);
                                                   }
                                               }
                                           });
            }
        }

        public MyTargetView(UInt32 slotId,
                            AdSize adSize = AdSize.Size320x50,
                            Boolean isRefreshAd = true)
        {
            _slotId = slotId;
            _adSize = adSize;
            _isRefreshAd = isRefreshAd;
            switch (adSize)
            {
                case AdSize.Size300x250:
                    _height = Height300x250;
                    _width = Width300x250;
                    break;
                case AdSize.Size728x90:
                    _height = Height728x90;
                    _width = Width728x90;
                    break;
                default:
                    _height = Height320x50;
                    _width = Width320x50;
                    break;
            }
        }

        ~MyTargetView()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy == null)
                {
                    return;
                }

                var referenceCopy = _myTargetViewProxy;
                _myTargetViewProxy = null;

                referenceCopy.AdClicked -= OnAdClicked;
                referenceCopy.AdLoadCompleted -= OnAdLoadCompleted;
                referenceCopy.AdLoadFailed -= OnAdLoadFailed;

                PlatformDispatcher.Perform(referenceCopy.Dispose);

                GC.SuppressFinalize(this);
            }
        }

        public override void Load()
        {
            PlatformDispatcher.Perform(LoadImpl);
        }

        public void Pause()
        {
            PlatformDispatcher.Perform(() =>
                                       {
                                           if (_myTargetViewProxy == null)
                                           {
                                               return;
                                           }

                                           lock (_syncRoot)
                                           {
                                               if (_myTargetViewProxy != null)
                                               {
                                                   _myTargetViewProxy.Pause();
                                               }
                                           }
                                       });
        }

        public void Resume()
        {
            PlatformDispatcher.Perform(() =>
                                       {
                                           if (_myTargetViewProxy == null)
                                           {
                                               return;
                                           }

                                           lock (_syncRoot)
                                           {
                                               if (_myTargetViewProxy != null)
                                               {
                                                   _myTargetViewProxy.Resume();
                                               }
                                           }
                                       });
        }

        public void Start()
        {
            PlatformDispatcher.Perform(() =>
                                       {
                                           if (_myTargetViewProxy == null)
                                           {
                                               return;
                                           }

                                           lock (_syncRoot)
                                           {
                                               if (_myTargetViewProxy != null)
                                               {
                                                   _myTargetViewProxy.Start();
                                               }
                                           }
                                       });
        }

        public void Stop()
        {
            PlatformDispatcher.Perform(() =>
                                       {
                                           if (_myTargetViewProxy == null)
                                           {
                                               return;
                                           }

                                           lock (_syncRoot)
                                           {
                                               if (_myTargetViewProxy != null)
                                               {
                                                   _myTargetViewProxy.Pause();
                                               }
                                           }
                                       });
        }

        protected override void SetTrackingEnvironmentEnabled(Boolean value)
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy == null)
                {
                    return;
                }

                _myTargetViewProxy.SetTrackingEnvironmentEnabled(value);
            }
        }

        protected override void SetTrackingLocationEnabled(Boolean value)
        {
            if (_myTargetViewProxy == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy == null)
                {
                    return;
                }

                _myTargetViewProxy.SetTrackingLocationEnabled(value);
            }
        }

        private void LoadImpl()
        {
            if (_myTargetViewProxy != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetViewProxy != null)
                {
                    return;
                }

                _myTargetViewProxy = PlatformFactory.CreateMyTargetControl(_slotId, _adSize, _isRefreshAd);

                _myTargetViewProxy.SetTrackingEnvironmentEnabled(TrackingEnvironmentEnabled);
                _myTargetViewProxy.SetTrackingLocationEnabled(TrackingLocationEnabled);

                CustomParams.SetCustomParamsProxy(_myTargetViewProxy.CustomParamsProxy);

                _myTargetViewProxy.SetHeight(_height);
                _myTargetViewProxy.SetWidth(_width);
                _myTargetViewProxy.SetX(_x);
                _myTargetViewProxy.SetY(_y);

                _myTargetViewProxy.AdClicked += OnAdClicked;
                _myTargetViewProxy.AdLoadCompleted += OnAdLoadCompleted;
                _myTargetViewProxy.AdLoadFailed += OnAdLoadFailed;

                _myTargetViewProxy.Load();
            }
        }
    }
}