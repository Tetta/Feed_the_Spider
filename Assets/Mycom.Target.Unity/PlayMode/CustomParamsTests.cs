#if PLAYMODE_TESTS_IS_ENABLED

using System;
using System.Collections;
using Mycom.Target.Unity.Ads;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mycom.Target.Unity.PlayMode
{
    public class CustomParamsTests
    {
        private const Int32 SlotId = 0;
        private static InterstitialAd _interstitialAd;

        private static readonly System.Object _sync = new System.Object();
        private static MyTargetView _view;

        private static void Age(CustomParams customParams)
        {
            UInt32? defaultValue = null;
            UInt32? firtstValue = 10;
            UInt32? secondValue = 20;

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firtstValue,
                        secondValue,
                        accessor.GetAge,
                        v => customParams.Age = v);
        }

        private static void Email(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "email0@mail.ru";
            const String secondValue = "email1@mail.ru";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetEmail,
                        v => customParams.Email = v);
        }

        private static void Emails(CustomParams customParams)
        {
            String[] defaultValue = null;
            String[] firstValue = { "email0@mail.ru", "email1@mail.ru" };
            String[] secondValue = { "email2@mail.ru", "email3@mail.ru" };

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetEmails,
                        v => customParams.Emails = v);
        }

        private static void Gender(CustomParams customParams)
        {
            const GenderEnum defaultValue = GenderEnum.Unspecified;
            const GenderEnum firstValue = GenderEnum.Male;
            const GenderEnum secondValue = GenderEnum.Female;

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetGender,
                        v => customParams.Gender = v);
        }

        private static void GenericTest<T>(T defaultValue,
                                           T firstValue,
                                           T secondValue,
                                           Func<T> getFunc,
                                           Action<T> setFunc)
        {
            Assert.AreEqual(getFunc(), defaultValue);

            setFunc(firstValue);

            Assert.AreEqual(getFunc(), firstValue);

            setFunc(secondValue);

            Assert.AreEqual(getFunc(), secondValue);

            setFunc(defaultValue);

            Assert.AreEqual(getFunc(), defaultValue);
        }

        private static void GenericTest<T>(T[] defaultValue,
                                           T[] firstValue,
                                           T[] secondValue,
                                           Func<T[]> getFunc,
                                           Action<T[]> setFunc)
        {
            Assert.That(getFunc(), Is.EqualTo(defaultValue));

            setFunc(firstValue);

            Assert.That(getFunc(), Is.EqualTo(firstValue));

            setFunc(secondValue);

            Assert.That(getFunc(), Is.EqualTo(secondValue));

            setFunc(defaultValue);

            Assert.That(getFunc(), Is.EqualTo(defaultValue));
        }

        private static void IcqId(CustomParams customParams)
        {
            UInt32? defaultValue = null;
            UInt32? firstValue = 123456;
            UInt32? secondValue = 234567;

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetIcqId,
                        v => customParams.IcqId = v);
        }

        private static void IcqIds(CustomParams customParams)
        {
            UInt32[] defaultValue = null;
            UInt32[] firstValue = { 123456, 234567 };
            UInt32[] secondValue = { 345678, 456789 };

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetIcqIds,
                        v => customParams.IcqIds = v);
        }

        private static void Lang(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "ru-RU";
            const String secondValue = "en-US";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetLang,
                        v => customParams.Lang = v);
        }

        private static void MrgsAppId(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "mrgsappid00";
            const String secondValue = "mrgsappid01";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetMrgsAppId,
                        v => customParams.MrgsAppId = v);
        }

        private static void MrgsId(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "mrgsid00";
            const String secondValue = "mrgsid01";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetMrgsId,
                        v => customParams.MrgsId = v);
        }

        private static void MrgsUserId(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "mrgsuserid00";
            const String secondValue = "mrgsuserid01";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetMrgsUserId,
                        v => customParams.MrgsUserId = v);
        }

        private static void OkId(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "okid0000";
            const String secondValue = "okid0001";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetOkId,
                        v => customParams.OkId = v);
        }

        private static void OkIds(CustomParams customParams)
        {
            String[] defaultValue = null;
            String[] firstValue = { "okid0000", "okid0001" };
            String[] secondValue = { "okid0002", "okid0003" };

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetOkIds,
                        v => customParams.OkIds = v);
        }

        private static void VkId(CustomParams customParams)
        {
            const String defaultValue = null;
            const String firstValue = "vkid0000";
            const String secondValue = "vkid0001";

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetVkId,
                        v => customParams.VkId = v);
        }

        private static void VkIds(CustomParams customParams)
        {
            String[] defaultValue = null;
            String[] firstValue = { "vkid0000", "vkid0001" };
            String[] secondValue = { "vkid0002", "vkid0003" };

            var accessor = new CustomParamsInternalAccessor(customParams);

            GenericTest(defaultValue,
                        firstValue,
                        secondValue,
                        accessor.GetVkIds,
                        v => customParams.VkIds = v);
        }

        [UnityTest]
        public IEnumerator Age()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            Age(_view.CustomParams);

            Age(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator Email()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            Email(_view.CustomParams);

            Email(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator Emails()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            Emails(_view.CustomParams);

            Emails(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator IcqId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            IcqId(_view.CustomParams);

            IcqId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator IcqIds()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            IcqIds(_view.CustomParams);

            IcqIds(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator Lang()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            Lang(_view.CustomParams);

            Lang(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator MrgsAppId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            MrgsAppId(_view.CustomParams);

            MrgsAppId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator MrgsId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            MrgsId(_view.CustomParams);

            MrgsId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator MrgsUserId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            MrgsUserId(_view.CustomParams);

            MrgsUserId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator OkId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            OkId(_view.CustomParams);

            OkId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator OkIds()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            OkIds(_view.CustomParams);

            OkIds(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator VkId()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            VkId(_view.CustomParams);

            VkId(_interstitialAd.CustomParams);
        }

        [UnityTest]
        public IEnumerator VkIds()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

            VkIds(_view.CustomParams);

            VkIds(_interstitialAd.CustomParams);
        }

        public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
        {
            private void Update()
            {
                if (_view == null)
                {
                    lock (_sync)
                    {
                        if (_view == null)
                        {
                            _view = new MyTargetView(SlotId);
                            _view.Load();

                            _interstitialAd = new InterstitialAd(SlotId);
                            _interstitialAd.Load();
                        }
                    }
                }

                IsTestFinished = true;
            }

            public Boolean IsTestFinished { get; private set; }
        }
    }
}

#endif