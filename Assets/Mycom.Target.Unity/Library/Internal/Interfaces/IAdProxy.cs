using System;

namespace Mycom.Target.Unity.Internal.Interfaces
{
    internal interface IAdProxy : IDisposable
    {
        ICustomParamsProxy CustomParamsProxy { get; }

        void Load();
        void SetTrackingEnvironmentEnabled(Boolean value);
        void SetTrackingLocationEnabled(Boolean value);

        event Action AdClicked;

        event Action AdLoadCompleted;

        event Action<String> AdLoadFailed;
    }
}