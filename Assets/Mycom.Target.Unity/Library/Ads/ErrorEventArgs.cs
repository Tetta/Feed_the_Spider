using System;

namespace Mycom.Target.Unity.Ads
{
    public sealed class ErrorEventArgs : EventArgs
    {
        public String Message { get; private set; }

        internal ErrorEventArgs(String message)
        {
            Message = message;
        }
    }
}