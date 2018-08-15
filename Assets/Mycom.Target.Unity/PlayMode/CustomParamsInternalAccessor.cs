#if PLAYMODE_TESTS_IS_ENABLED

using System;
using Mycom.Target.Unity.Ads;

namespace Mycom.Target.Unity.PlayMode
{
    public partial class CustomParamsInternalAccessor
    {
        internal CustomParamsInternalAccessor(CustomParams customParams)
        {
            Init(customParams);
        }

        internal UInt32? GetAge()
        {
            UInt32? result = null;
            GetAge(ref result);
            return result;
        }

        internal String GetEmail()
        {
            String result = null;
            GetEmail(ref result);
            return result;
        }

        internal String[] GetEmails()
        {
            String[] result = null;
            GetEmails(ref result);
            return result;
        }

        internal GenderEnum GetGender()
        {
            GenderEnum? result = null;
            GetGender(ref result);
            return result.Value;
        }

        internal UInt32? GetIcqId()
        {
            UInt32? result = null;
            GetIcqId(ref result);
            return result;
        }

        internal UInt32[] GetIcqIds()
        {
            UInt32[] result = null;
            GetIcqIds(ref result);
            return result;
        }

        internal String GetLang()
        {
            String result = null;
            GetLang(ref result);
            return result;
        }

        internal String GetMrgsAppId()
        {
            String result = null;
            GetMrgsAppId(ref result);
            return result;
        }

        internal String GetMrgsId()
        {
            String result = null;
            GetMrgsId(ref result);
            return result;
        }

        internal String GetMrgsUserId()
        {
            String result = null;
            GetMrgsUserId(ref result);
            return result;
        }

        internal String GetOkId()
        {
            String result = null;
            GetOkId(ref result);
            return result;
        }

        internal String[] GetOkIds()
        {
            String[] result = null;
            GetOkIds(ref result);
            return result;
        }

        internal String GetVkId()
        {
            String result = null;
            GetVkId(ref result);
            return result;
        }

        internal String[] GetVkIds()
        {
            String[] result = null;
            GetVkIds(ref result);
            return result;
        }

        partial void Init(CustomParams customParams);
        partial void GetAge(ref UInt32? result);
        partial void GetEmail(ref String result);
        partial void GetEmails(ref String[] result);
        partial void GetGender(ref GenderEnum? result);
        partial void GetIcqId(ref UInt32? result);
        partial void GetIcqIds(ref UInt32[] result);
        partial void GetLang(ref String result);
        partial void GetMrgsAppId(ref String result);
        partial void GetMrgsId(ref String result);
        partial void GetMrgsUserId(ref String result);
        partial void GetOkId(ref String result);
        partial void GetOkIds(ref String[] result);
        partial void GetVkId(ref String result);
        partial void GetVkIds(ref String[] result);
    }
}

#endif