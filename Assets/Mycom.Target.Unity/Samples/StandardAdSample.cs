using System;
using System.Linq;
using Mycom.Target.Unity.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

namespace Mycom.Target.Unity.Samples
{
    internal static class StandardAdSlots
    {
        public static UInt32 GetNative300x250Slot()
        {
#if UNITY_IOS
            return 64529;
#elif UNITY_ANDROID
            return 64526;
#else
            return 0;
#endif
        }

        public static UInt32 GetNative320x50Slot()
        {
#if UNITY_IOS
            return 30269;
#elif UNITY_ANDROID
            return 14170;
#else
            return 0;
#endif
        }

        public static UInt32 GetNative728x90Slot()
        {
#if UNITY_IOS
            return 81621;
#elif UNITY_ANDROID
            return 81620;
#else
            return 0;
#endif
        }

        public static UInt32 GetWeb300x250Slot()
        {
#if UNITY_IOS
            return 64528;
#elif UNITY_ANDROID
            return 64525;
#else
            return 0;
#endif
        }

        public static UInt32 GetWeb320x50Slot()
        {
#if UNITY_IOS
            return 30268;
#elif UNITY_ANDROID
            return 7250;
#else
            return 0;
#endif
        }

        public static UInt32 GetWeb728x90Slot()
        {
#if UNITY_IOS
            return 81626;
#elif UNITY_ANDROID
            return 81624;
#else
            return 0;
#endif
        }
    }

    public sealed class StandardAdSample : MonoBehaviour
    {
        private const String AutocloseToggleName = "Autoclose";
        private const String InterstitialAdButtonName = "InterstitialAd";
        private const String Web320x50ButtonName = "Web320x50";
        private const String Web300x250ButtonName = "Web300x250";
        private const String Web728x90ButtonName = "Web728x90";
        private const String Native320x50ButtonName = "Native320x50";
        private const String Native300x250ButtonName = "Native300x250";
        private const String Native728x90ButtonName = "Native728x90";

        private readonly Object _syncRoot = new Object();

        private MyTargetView _myTargetView;

        private void Awake()
        {
            AbstractAd.IsDebugMode = true;

            {
                var web320x50Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Web320x50ButtonName);
                if (web320x50Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetWeb320x50Slot(), MyTargetView.AdSize.Size320x50));
                    web320x50Button.onClick = onClickEvent;
                }
            }

            {
                var web300x250Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Web300x250ButtonName);
                if (web300x250Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetWeb300x250Slot(), MyTargetView.AdSize.Size300x250));
                    web300x250Button.onClick = onClickEvent;
                }
            }

            {
                var web728x90Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Web728x90ButtonName);
                if (web728x90Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetWeb728x90Slot(), MyTargetView.AdSize.Size728x90));
                    web728x90Button.onClick = onClickEvent;
                }
            }

            {
                var native320x50Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Native320x50ButtonName);
                if (native320x50Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetNative320x50Slot(), MyTargetView.AdSize.Size320x50));
                    native320x50Button.onClick = onClickEvent;
                }
            }

            {
                var native300x250Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Native300x250ButtonName);
                if (native300x250Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetNative300x250Slot(), MyTargetView.AdSize.Size300x250));
                    native300x250Button.onClick = onClickEvent;
                }
            }

            {
                var native728x90Button = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == Native728x90ButtonName);
                if (native728x90Button)
                {
                    var onClickEvent = new Button.ButtonClickedEvent();
                    onClickEvent.AddListener(() => OnButtonClicked(StandardAdSlots.GetNative728x90Slot(), MyTargetView.AdSize.Size728x90));
                    native728x90Button.onClick = onClickEvent;
                }
            }

            var interstitialAdButton = FindObjectsOfType<Button>().FirstOrDefault(button => button.name == InterstitialAdButtonName);
            if (interstitialAdButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(OpenInterstitialAdScene);
                interstitialAdButton.onClick = onClickEvent;
            }
        }

        private void OnButtonClicked(UInt32 slotId, MyTargetView.AdSize adSize)
        {
            lock (_syncRoot)
            {
                if (_myTargetView != null)
                {
                    _myTargetView.Dispose();
                    _myTargetView.Stop();
                }

                _myTargetView = new MyTargetView(slotId, adSize)
                                {
                                    CustomParams =
                                    {
                                        Age = 23,
                                        Gender = GenderEnum.Male,
                                        Lang = "ru-RU"
                                    }
                                };

                _myTargetView.AdClicked += OnAdClicked;
                _myTargetView.AdLoadFailed += OnAdLoadFailed;
                _myTargetView.AdLoadCompleted += OnAdLoadCompleted;

                _myTargetView.Load();
            }
        }

        private void OnAdClicked(Object sender, EventArgs eventArgs) { }

        private void OnAdLoadFailed(Object sender, ErrorEventArgs errorEventArgs)
        {
            Debug.Log("OnAdLoadFailed: " + errorEventArgs.Message);
        }

        private void OnDestroy()
        {
            if (_myTargetView == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }

                _myTargetView.Dispose();
                _myTargetView = null;
            }
        }

        private void OnApplicationPause(Boolean pauseStatus)
        {
            if (_myTargetView == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }

                if (pauseStatus)
                {
                    _myTargetView.Pause();
                }
                else
                {
                    _myTargetView.Resume();
                }
            }
        }

        private static void OpenInterstitialAdScene()
        {
            SceneManager.LoadScene("InterstitialAdSample");
        }

        private void OnAdLoadCompleted(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnAdLoadCompleted");

            var isAutoClose = FindObjectsOfType<Toggle>().Where(toggle => toggle.name == AutocloseToggleName)
                                                         .Select(toggle => toggle.isOn)
                                                         .FirstOrDefault();

            System.Threading.ThreadPool.QueueUserWorkItem(state => StartImpl(isAutoClose));
        }

        private void StartImpl(Boolean isAutoClose)
        {
            const Int32 timeout = 120000;

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }
            }

            _myTargetView.Start();

            if (!isAutoClose)
            {
                return;
            }

            System.Threading.Thread.Sleep(timeout);

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }
            }

            _myTargetView.X = 50;
            _myTargetView.Y = 50;

            System.Threading.Thread.Sleep(timeout);

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }
            }

            _myTargetView.Width += 50;

            System.Threading.Thread.Sleep(timeout);

            lock (_syncRoot)
            {
                if (_myTargetView == null)
                {
                    return;
                }
            }

            _myTargetView.Dispose();
            _myTargetView = null;
        }
    }
}