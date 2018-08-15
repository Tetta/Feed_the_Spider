#if PLAYMODE_TESTS_IS_ENABLED && UNITY_IOS

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Mycom.Target.Unity.Ads;

namespace Mycom.Target.Unity.PlayMode
{
    public partial class CustomParamsInternalAccessor
    {
        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetAge(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetEmail(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetGender(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetIcqId(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetLang(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetMrgsAppId(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetMrgsId(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetMrgsUserId(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetOkId(UInt32 adId);

        [DllImport("__Internal")]
        private static extern String MTRGCustomParamsGetVkId(UInt32 adId);

        private UInt32 _adId;

        partial void Init(CustomParams customParams)
        {
            var proxyImplField = typeof(CustomParams).GetField("_customParamsProxy", BindingFlags.Instance | BindingFlags.NonPublic);

            var proxy = proxyImplField.GetValue(customParams);

            var adIdField = proxy.GetType().GetField("_adId", BindingFlags.Instance | BindingFlags.NonPublic);

            _adId = (UInt32)adIdField.GetValue(proxy);
        }

        partial void GetAge(ref UInt32? result)
        {
            var ageString = MTRGCustomParamsGetAge(_adId);

            result = String.IsNullOrEmpty(ageString) ? default(UInt32?) : Convert.ToUInt32(ageString);
        }

        partial void GetEmail(ref String result)
        {
            var values = MTRGCustomParamsGetEmail(_adId);

            result = values == null ? null : values.Split(',').FirstOrDefault();
        }

        partial void GetEmails(ref String[] result)
        {
            var values = MTRGCustomParamsGetEmail(_adId);

            result = values == null ? null : values.Split(',');
        }

        partial void GetGender(ref GenderEnum? result)
        {
            var value = MTRGCustomParamsGetGender(_adId);

            result = String.IsNullOrEmpty(value) ? null : (GenderEnum?)Convert.ToUInt32(value);
        }

        partial void GetIcqId(ref UInt32? result)
        {
            var value = MTRGCustomParamsGetIcqId(_adId);

            result = String.IsNullOrEmpty(value) ? default(UInt32?) : Convert.ToUInt32(value);
        }

        partial void GetIcqIds(ref UInt32[] result)
        {
            var value = MTRGCustomParamsGetIcqId(_adId);

            result = String.IsNullOrEmpty(value) ? null : value.Split(',').Select(v => Convert.ToUInt32(v)).ToArray();
        }

        partial void GetLang(ref String result)
        {
            result = MTRGCustomParamsGetLang(_adId);
        }

        partial void GetMrgsAppId(ref String result)
        {
            result = MTRGCustomParamsGetMrgsAppId(_adId);
        }

        partial void GetMrgsId(ref String result)
        {
            result = MTRGCustomParamsGetMrgsId(_adId);
        }

        partial void GetMrgsUserId(ref String result)
        {
            result = MTRGCustomParamsGetMrgsUserId(_adId);
        }

        partial void GetOkId(ref String result)
        {
            var values = MTRGCustomParamsGetOkId(_adId);
            result = values == null ? null : values.Split(',').FirstOrDefault();
        }

        partial void GetOkIds(ref String[] result)
        {
            var values = MTRGCustomParamsGetOkId(_adId);
            result = values == null ? null : values.Split(',').ToArray();
        }

        partial void GetVkId(ref String result)
        {
            var values = MTRGCustomParamsGetVkId(_adId);
            result = values == null ? null : values.Split(',').FirstOrDefault();
        }

        partial void GetVkIds(ref String[] result)
        {
            var values = MTRGCustomParamsGetVkId(_adId);
            result = values == null ? null : values.Split(',').ToArray();
        }
    }
}

#endif