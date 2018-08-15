#if UNITY_ANDROID
using System;
using Mycom.Target.Unity.Ads;
using Mycom.Target.Unity.Internal.Interfaces;
using UnityEngine;

namespace Mycom.Target.Unity.Internal.Implementations.Android
{
    internal class CustomParamsProxy : ICustomParamsProxy
    {
        private const String SetCustomParamMethodName = "setCustomParam";

        private readonly AndroidJavaObject _javaCustomParams;
        private Boolean _isDisposed;

        public CustomParamsProxy(AndroidJavaObject javaCustomParams)
        {
            _javaCustomParams = javaCustomParams;
            ((ICustomParamsProxy)this).SetCustomParam("framework", "1");
        }

        ~CustomParamsProxy()
        {
            ((IDisposable)this).Dispose();
        }

        void ICustomParamsProxy.SetAge(UInt32? value)
        {
            const String methodName = "setAge";
            if (value == null)
            {
                _javaCustomParams.Call(methodName, -1);
            }
            else
            {
                _javaCustomParams.Call(methodName, (Int32)value);
            }
        }

        void ICustomParamsProxy.SetEmails(String[] value)
        {
            using (var javaArray = PlatformHelper.CreateJavaStringArray(value))
            {
                _javaCustomParams.Call("setEmails", javaArray);
            }
        }

        void ICustomParamsProxy.SetGender(GenderEnum value)
        {
            _javaCustomParams.Call("setGender", (Int32)value);
        }

        void ICustomParamsProxy.SetLang(String value)
        {
            using (var javaValue = PlatformHelper.CreateJavaString(value))
            {
                _javaCustomParams.Call("setLang", javaValue);
            }
        }

        void ICustomParamsProxy.SetIcqIds(UInt32[] value)
        {
            const String methodName = "setIcqIds";
            if (value == null)
            {
                _javaCustomParams.Call(methodName, null);
            }
            else
            {
                String[] array = new String[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    array[i] = Convert.ToString(value[i]);
                }
                
                using (var javaArray = PlatformHelper.CreateJavaStringArray(array))
                {
                    _javaCustomParams.Call(methodName, javaArray);
                }
            }
        }

        void ICustomParamsProxy.SetMrgsAppId(String value)
        {
            using (var javaValue = PlatformHelper.CreateJavaString(value))
            {
                _javaCustomParams.Call("setMrgsAppId", javaValue);
            }
        }

        void ICustomParamsProxy.SetMrgsId(String value)
        {
            using (var javaValue = PlatformHelper.CreateJavaString(value))
            {
                _javaCustomParams.Call("setMrgsId", javaValue);
            }
        }

        void ICustomParamsProxy.SetMrgsUserId(String value)
        {
            using (var javaValue = PlatformHelper.CreateJavaString(value))
            {
                _javaCustomParams.Call("setMrgsUserId", javaValue);
            }
        }

        void ICustomParamsProxy.SetOkIds(String[] value)
        {
            using (var javaArray = PlatformHelper.CreateJavaStringArray(value))
            {
                _javaCustomParams.Call("setOkIds", javaArray);
            }
        }

        void ICustomParamsProxy.SetVkIds(String[] value)
        {
            using (var javaArray = PlatformHelper.CreateJavaStringArray(value))
            {
                _javaCustomParams.Call("setVKIds", javaArray);
            }
        }

        void ICustomParamsProxy.SetCustomParam(String key, String value)
        {
            using (var javaKey = PlatformHelper.CreateJavaString(key))
            {
                using (var javaValue = PlatformHelper.CreateJavaString(value))
                {
                    _javaCustomParams.Call(SetCustomParamMethodName, javaKey, javaValue);
                }
            }
        }

        void IDisposable.Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _javaCustomParams.Dispose();
        }
    }
}

#endif