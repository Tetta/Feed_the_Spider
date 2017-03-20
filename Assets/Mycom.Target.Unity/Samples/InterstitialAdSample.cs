using System;
using System.Linq;
using Mycom.Target.Unity.Ads;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Mycom.Target.Unity.Samples
{
    public static class FullscreenSlots
    {
        public static UInt32 GetImageSlotId()
        {
#if UNITY_WP_8_1 || UNITY_WINRT_10_0
            return 30297;
#elif UNITY_IOS
            return 6498;
#elif UNITY_ANDROID
            return 6481;
#else
            return 0;
#endif
        }

        public static UInt32 GetPromoSlotId()
        {
#if UNITY_WP_8_1 || UNITY_WINRT_10_0
            return 22095;
#elif UNITY_IOS
            return 6899;
#elif UNITY_ANDROID
            return 6896;
#else
            return 0;
#endif
        }

        public static UInt32 GetPromoVideoSlotId()
        {
#if UNITY_WP_8_1 || UNITY_WINRT_10_0
            return 22093;
#elif UNITY_IOS
            return 22091;
#elif UNITY_ANDROID
            return 10138;
#else
            return 0;
#endif
        }

        public static UInt32 GetVideoStyleSlotId()
        {
#if UNITY_WP_8_1 || UNITY_WINRT_10_0
            return 38839;
#elif UNITY_IOS
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
        private const String ImageButtonName = "Image";
        private const String PromoButtonName = "Promo";
        private const String PromoVideoButtonName = "PromoVideo";
        private const String VideoStyleButtonName = "VideoStyle";

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

        private static void OnLoadCompletedShow(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnLoadCompletedShow");

            var interstitialAd = sender as InterstitialAd;
            if (interstitialAd != null)
            {
                interstitialAd.Show();
            }
        }

        private static void OnLoadCompletedShowDialog(Object sender, EventArgs eventArgs)
        {
            Debug.Log("OnLoadCompletedShowDialog");

            var interstitialAd = sender as InterstitialAd;
            if (interstitialAd != null)
            {
                interstitialAd.ShowDialog();
            }
        }

        private readonly Object _syncRoot = new Object();

        private InterstitialAd _interstitialAd;

        private void Awake()
        {
            Tracer.IsEnabled = true;

            var asDialog = FindObjectsOfType<Toggle>().FirstOrDefault(toggle => toggle.name == AsDialogToggleName);

            var buttons = FindObjectsOfType<Button>().ToArray();

            var imageButton = buttons.FirstOrDefault(button => button.name == ImageButtonName);
            if (imageButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetImageSlotId(),
                                                              asDialog != null && asDialog.isOn));
                imageButton.onClick = onClickEvent;
            }

            var promoButton = buttons.FirstOrDefault(button => button.name == PromoButtonName);
            if (promoButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetPromoSlotId(),
                                                              asDialog != null && asDialog.isOn));
                promoButton.onClick = onClickEvent;
            }

            var promoVideoButton = buttons.FirstOrDefault(button => button.name == PromoVideoButtonName);
            if (promoVideoButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetPromoVideoSlotId(),
                                                              asDialog != null && asDialog.isOn));
                promoVideoButton.onClick = onClickEvent;
            }

            var videoStyleButton = buttons.FirstOrDefault(button => button.name == VideoStyleButtonName);
            if (videoStyleButton)
            {
                var onClickEvent = new Button.ButtonClickedEvent();
                onClickEvent.AddListener(() => ShowFullscreen(FullscreenSlots.GetVideoStyleSlotId(),
                                                              asDialog != null && asDialog.isOn));
                videoStyleButton.onClick = onClickEvent;
            }
        }

        private void OnAdDismissed(Object sender, EventArgs eventArgs)
        {
            lock (_syncRoot)
            {
                Debug.Log("OnAdDismissed");

                _interstitialAd = null;
            }
        }

        private void OnAdLoadFailed(Object sender, ErrorEventArgs e)
        {
            lock (_syncRoot)
            {
                Debug.Log("OnAdLoadFailed: " + e.Message);

                _interstitialAd = null;
            }
        }

        private void ShowFullscreen(UInt32 slotId, Boolean showAsDialog)
        {
            lock (_syncRoot)
            {
                if (_interstitialAd != null)
                {
                    return;
                }

                _interstitialAd = new InterstitialAd(slotId)
                                  {
                                      // Задаём дополнительные параметры запроса
                                      CustomParams =
                                      {
                                          // Задаем возраст
                                          Age = 23,
                                          // Задаем пол
                                          Gender = GenderEnum.Male
                                      }
                                  };
            }

            _interstitialAd.AdClicked += OnAdClicked;
            _interstitialAd.AdDismissed += OnAdDismissed;
            _interstitialAd.AdDisplayed += OnAdDisplayed;
            _interstitialAd.AdLoadFailed += OnAdLoadFailed;
            _interstitialAd.AdVideoCompleted += OnAdVideoCompleted;

            if (!showAsDialog)
            {
                _interstitialAd.AdLoadCompleted += OnLoadCompletedShow;
            }
            else
            {
                _interstitialAd.AdLoadCompleted += OnLoadCompletedShowDialog;
            }

            _interstitialAd.Load();
        }
    }
}