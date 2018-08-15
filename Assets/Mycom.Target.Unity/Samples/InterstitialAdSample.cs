using System;
using System.Linq;
using Mycom.Target.Unity.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;

namespace Mycom.Target.Unity.Samples
{
    internal static class FullscreenSlots
    {
        public static UInt32 GetImageSlotId()
        {
#if UNITY_IOS
            return 6498;
#elif UNITY_ANDROID
            return 6481;
#else
            return 0;
#endif
        }

        public static UInt32 GetPromoSlotId()
        {
#if UNITY_IOS
            return 6899;
#elif UNITY_ANDROID
            return 6896;
#else
            return 0;
#endif
        }

        public static UInt32 GetPromoVideoSlotId()
        {
#if UNITY_IOS
            return 22091;
#elif UNITY_ANDROID
            return 10138;
#else
            return 0;
#endif
        }

        public static UInt32 GetVideoStyleSlotId()
        {
#if UNITY_IOS
            return 38838;
#elif UNITY_ANDROID
            return 38837;
#else
            return 0;
#endif
        }
    }

    public class InterstitialAdSample : MonoBehaviour
    {
        private const String AsDialogToggleName = "AsDialog";
        private const String AutocloseToggleName = "Autoclose";
        private const String ImageButtonName = "Image";
        private const String PromoButtonName = "Promo";
        private const String PromoVideoButtonName = "PromoVideo";
        private const String VideoStyleButtonName = "VideoStyle";
        private const String StandardAdButtonName = "StandardAd";

        private static void OnAdClicked(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnAdClicked");
        }

        private static void OnAdDisplayed(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnAdDisplayed");
        }

        private static void OnAdVideoCompleted(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnAdVideoCompleted");
        }

        private readonly Object _syncRoot = new Object();

        private volatile InterstitialAd _interstitialAd;

        private void Awake()
        {
            AbstractAd.IsDebugMode = true;

            var buttons = FindObjectsOfType<Button>().ToArray();

            var imageButton = buttons.FirstOrDefault(button => button.name == ImageButtonName);
            if (imageButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetImageSlotId()));
                imageButton.onClick = onClickEvent;
            }

            var promoButton = buttons.FirstOrDefault(button => button.name == PromoButtonName);
            if (promoButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetPromoSlotId()));
                promoButton.onClick = onClickEvent;
            }

            var promoVideoButton = buttons.FirstOrDefault(button => button.name == PromoVideoButtonName);
            if (promoVideoButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetPromoVideoSlotId()));
                promoVideoButton.onClick = onClickEvent;
            }

            var videoStyleButton = buttons.FirstOrDefault(button => button.name == VideoStyleButtonName);
            if (videoStyleButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetVideoStyleSlotId()));
                videoStyleButton.onClick = onClickEvent;
            }

            var standardAdButton = buttons.FirstOrDefault(button => button.name == StandardAdButtonName);
            if (standardAdButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(OpenStandardAdScene);
                standardAdButton.onClick = onClickEvent;
            }
        }

        private void OnDestroy()
        {
            if (_interstitialAd == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAd == null)
                {
                    return;
                }

                _interstitialAd.Dispose();
                _interstitialAd = null;
            }
        }

        private static void OpenStandardAdScene()
        {
            SceneManager.LoadScene("StandardAdSample");
        }

        private void OnAdDismissed(Object sender, EventArgs eventArgs)
        {
            lock (_syncRoot)
            {
                Debug.Log("OnAdDismissed");

                if (_interstitialAd != null)
                {
                    _interstitialAd.Dispose();
                }

                _interstitialAd = null;
            }
        }

        private void OnAdLoadFailed(Object sender, ErrorEventArgs e)
        {
            lock (_syncRoot)
            {
                Debug.Log("OnAdLoadFailed: " + e.Message);

                if (_interstitialAd != null)
                {
                    _interstitialAd.Dispose();
                }

                _interstitialAd = null;
            }
        }

        private void OnLoadCompleted(Object sender, EventArgs eventArgs)
        {
            var asDialog = FindObjectsOfType<Toggle>().Where(toggle => toggle.name == AsDialogToggleName)
                                                      .Select(toggle => toggle.isOn)
                                                      .FirstOrDefault();

            var isAutoClose = FindObjectsOfType<Toggle>().Where(toggle => toggle.name == AutocloseToggleName)
                                                         .Select(toggle => toggle.isOn)
                                                         .FirstOrDefault();

            System.Threading.ThreadPool.QueueUserWorkItem(state => ShowImpl(asDialog, isAutoClose));
        }

        private void ShowFullscreen(UInt32 slotId)
        {
            if (_interstitialAd != null)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_interstitialAd != null)
                {
                    return;
                }

                _interstitialAd = new InterstitialAd(slotId)
                                  {
                                      CustomParams =
                                      {
                                          Age = 23,
                                          Gender = GenderEnum.Male,
                                          Lang = "ru-RU"
                                      }
                                  };

                _interstitialAd.AdClicked += OnAdClicked;
                _interstitialAd.AdDismissed += OnAdDismissed;
                _interstitialAd.AdDisplayed += OnAdDisplayed;
                _interstitialAd.AdLoadFailed += OnAdLoadFailed;
                _interstitialAd.AdVideoCompleted += OnAdVideoCompleted;

                _interstitialAd.AdLoadCompleted += OnLoadCompleted;

                _interstitialAd.Load();
            }
        }

        private void ShowImpl(Boolean asDialog, Boolean isAutoClose)
        {
            lock (_syncRoot)
            {
                if (_interstitialAd == null)
                {
                    return;
                }

                if (asDialog)
                {
                    _interstitialAd.ShowDialog();
                }
                else
                {
                    _interstitialAd.Show();
                }
            }

            if (!isAutoClose)
            {
                return;
            }

            const Int32 timeout = 120000;

            System.Threading.Thread.Sleep(timeout);

            lock (_syncRoot)
            {
                if (_interstitialAd == null)
                {
                    return;
                }

                _interstitialAd.Dismiss();
            }
        }
    }
}