using System;

namespace Mycom.Target.Unity.Internal.Interfaces
{
    internal interface IDispatcher
    {
        void Perform(Action action);
    }
}