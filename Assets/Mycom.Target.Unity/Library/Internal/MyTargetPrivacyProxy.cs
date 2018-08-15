using System;

namespace Mycom.Target.Unity.Internal
{
    internal static partial class MyTargetPrivacyProxy
    {
        private const Boolean UserConsentDefault = true;
        private const Boolean UserAgeRestrictedDefault = false;

        internal static Boolean UserConsent
        {
            get
            {
                try
                {
                    Boolean result = false;
                    GetUserConsent(ref result);
                    return result;
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());

                    return UserConsentDefault;
                }
            }
            set
            {
                try
                {
                    SetUserConsent(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        internal static Boolean UserAgeRestricted
        {
            get
            {
                try
                {
                    Boolean result = false;
                    GetUserAgeRestricted(ref result);
                    return result;
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());

                    return UserAgeRestrictedDefault;
                }
            }
            set
            {
                try
                {
                    SetUserAgeRestricted(value);
                }
                catch (Exception ex)
                {
                    MyTargetLogger.Log(ex.ToString());
                }
            }
        }

        static partial void GetUserConsent(ref Boolean isUserConsent);

        static partial void SetUserConsent(Boolean userConsent);

        static partial void GetUserAgeRestricted(ref Boolean isUserAgeRestricted);

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted);
    }
}

#if !UNITY_IOS && !UNITY_ANDROID

namespace Mycom.Target.Unity.Internal
{
    internal static partial class MyTargetPrivacyProxy
    {
        static partial void GetUserConsent(ref Boolean isUserConsent) { }

        static partial void SetUserConsent(Boolean userConsent) { }

        static partial void GetUserAgeRestricted(ref Boolean isUserAgeRestricted) { }

        static partial void SetUserAgeRestricted(Boolean userAgeRestricted) { }
    }
}

#endif