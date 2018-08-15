using System;

namespace Mycom.Target.Unity.Internal.Interfaces
{
    internal interface IMyTargetViewProxy : IAdProxy
    {
        void Pause();

        void Resume();

        void SetHeight(Double value);

        void SetWidth(Double value);

        void SetX(Double value);

        void SetY(Double value);

        void Start();

        void Stop();
    }
}