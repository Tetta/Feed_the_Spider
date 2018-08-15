using System;

namespace Mycom.Target.Unity.Internal.Interfaces
{
    internal interface IInterstitialAdProxy : IAdProxy
    {
        void Dismiss();

        void Show();

        void ShowDialog();

        event Action AdDismissed;

        event Action AdDisplayed;

        event Action AdVideoCompleted;
    }
}