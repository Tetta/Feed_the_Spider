using System;
using Mycom.Target.Unity.Internal;

namespace Mycom.Target.Unity.Ads
{
    public static class MyTargetPrivacy
    {
        public static Boolean UserConsent
        {
            get { return MyTargetPrivacyProxy.UserConsent; }
            set { MyTargetPrivacyProxy.UserConsent = value; }
        }

        public static Boolean UserAgeRestricted
        {
            get { return MyTargetPrivacyProxy.UserAgeRestricted; }
            set { MyTargetPrivacyProxy.UserAgeRestricted = value; }
        }
    }
}