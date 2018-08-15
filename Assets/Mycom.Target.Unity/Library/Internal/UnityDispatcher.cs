using System;
using System.Collections.Generic;
using System.Threading;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;
using Object = System.Object;

namespace Mycom.Target.Unity.Internal
{
    internal sealed class UnityDispatcher : MonoBehaviour, IDispatcher
    {
        private static readonly Object Sync = new Object();
        private static readonly Queue<Action> Queue = new Queue<Action>();

        private static Int32 _mainThreadId;
        private static GameObject GameObject;

        private static Int32 GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        internal static IDispatcher GetInstance()
        {
            try
            {
                if (GameObject == null)
                {
                    lock (Sync)
                    {
                        if (GameObject == null)
                        {
                            GameObject = new GameObject();
                            _mainThreadId = GetThreadId();
                        }
                    }
                }
                return GameObject.GetComponent<UnityDispatcher>() ?? GameObject.AddComponent<UnityDispatcher>();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return null;
        }

        void Update()
        {
            lock (Queue)
            {
                if (Queue.Count <= 0)
                {
                    return;
                }

                try
                {
                    Queue.Dequeue()();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }

        void OnDestroy()
        {
            lock (Sync)
            {
                GameObject = null;
            }
        }

        void IDispatcher.Perform(Action action)
        {
            if (GetThreadId() == _mainThreadId)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {
                lock (Queue)
                {
                    Queue.Enqueue(action);
                }
            }
        }
    }
}