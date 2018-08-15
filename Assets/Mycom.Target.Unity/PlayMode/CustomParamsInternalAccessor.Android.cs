#if PLAYMODE_TESTS_IS_ENABLED && UNITY_ANDROID

using System;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Mycom.Target.Unity.Ads;
using UnityEngine;

namespace Mycom.Target.Unity.PlayMode
{
    public partial class CustomParamsInternalAccessor
    {
        private AndroidJavaObject _javaCustomParams;

        partial void Init(CustomParams customParams)
        {
            var proxyImplField = typeof(CustomParams).GetField("_customParamsProxy", BindingFlags.Instance | BindingFlags.NonPublic);

            var proxy = default(System.Object);
            while (true)
            {
                proxy = proxyImplField.GetValue(customParams);
                if (proxy != null)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            var javaCustomParamsField = proxy.GetType().GetField("_javaCustomParams", BindingFlags.Instance | BindingFlags.NonPublic);

            _javaCustomParams = (AndroidJavaObject)javaCustomParamsField.GetValue(proxy);
        }

        partial void GetAge(ref UInt32? result)
        {
            var value = _javaCustomParams.Call<Int32>("getAge");
            result = value <= 0 ? default(UInt32?) : (UInt32)value;
        }

        partial void GetEmail(ref string result)
        {
            result = _javaCustomParams.Call<String>("getEmail");
        }

        partial void GetEmails(ref string[] result)
        {
            result = _javaCustomParams.Call<String[]>("getEmails");
        }

        partial void GetGender(ref GenderEnum? result)
        {
            result = (GenderEnum)_javaCustomParams.Call<Int32>("getGender");
        }

        partial void GetIcqId(ref uint? result)
        {
            var value = _javaCustomParams.Call<String>("getIcqId");
            result = String.IsNullOrEmpty(value) ? default(UInt32?) : Convert.ToUInt32(value);
        }

        partial void GetIcqIds(ref uint[] result)
        {
            var values = _javaCustomParams.Call<String[]>("getIcqIds");
            result = values == null ? null : values.Select(v => Convert.ToUInt32(v)).ToArray();
        }

        partial void GetLang(ref string result)
        {
            result = _javaCustomParams.Call<String>("getLang");
        }

        partial void GetMrgsAppId(ref string result)
        {
            result = _javaCustomParams.Call<String>("getMrgsAppId");
        }

        partial void GetMrgsId(ref string result)
        {
            result = _javaCustomParams.Call<String>("getMrgsId");
        }

        partial void GetMrgsUserId(ref string result)
        {
            result = _javaCustomParams.Call<String>("getMrgsUserId");
        }

        partial void GetOkId(ref string result)
        {
            result = _javaCustomParams.Call<String>("getOkId");
        }

        partial void GetOkIds(ref string[] result)
        {
            result = _javaCustomParams.Call<String[]>("getOkIds");
        }

        partial void GetVkId(ref string result)
        {
            result = _javaCustomParams.Call<String>("getVKId");
        }

        partial void GetVkIds(ref string[] result)
        {
            result = _javaCustomParams.Call<String[]>("getVKIds");
        }
    }
}

#endif