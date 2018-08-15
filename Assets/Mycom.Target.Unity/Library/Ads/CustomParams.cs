using System;
using System.Collections.Generic;
using System.Linq;
using Mycom.Target.Unity.Internal.Interfaces;

namespace Mycom.Target.Unity.Ads
{
    public sealed class CustomParams
    {
        private const String FrameworkSDKKey = "framework";
        private const String FrameworkSDKValue = "1";

        private readonly IDictionary<String, String> _customParams = new Dictionary<String, String>();

        private UInt32? _age;
        private ICustomParamsProxy _customParamsProxy;
        private String[] _emails;
        private GenderEnum _gender = GenderEnum.Unspecified;
        private UInt32[] _icqIds;
        private String _lang;
        private String _mrgsAppId;
        private String _mrgsId;
        private String _mrgsUserId;
        private String[] _okIds;
        private String[] _vkIds;

        public CustomParams()
        {
            SetCustomParam(FrameworkSDKKey, FrameworkSDKValue);
        }

        public UInt32? Age
        {
            get { return _age; }
            set
            {
                _age = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetAge(value);
                }
            }
        }

        public String Email
        {
            get { return _emails == null ? null : _emails.FirstOrDefault(); }
            set
            {
                _emails = value == null ? null : new[] { value };
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetEmails(_emails);
                }
            }
        }

        public String[] Emails
        {
            get { return _emails; }
            set
            {
                _emails = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetEmails(_emails);
                }
            }
        }

        public GenderEnum Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetGender(value);
                }
            }
        }

        public UInt32? IcqId
        {
            get { return _icqIds == null ? default(UInt32?) : _icqIds.FirstOrDefault(); }
            set
            {
                _icqIds = value == null ? null : new[] { value.Value };
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetIcqIds(_icqIds);
                }
            }
        }

        public UInt32[] IcqIds
        {
            get { return _icqIds; }
            set
            {
                _icqIds = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetIcqIds(_icqIds);
                }
            }
        }

        public String Lang
        {
            get { return _lang; }
            set
            {
                _lang = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetLang(_lang);
                }
            }
        }

        public String MrgsAppId
        {
            get { return _mrgsAppId; }
            set
            {
                _mrgsAppId = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetMrgsAppId(_mrgsAppId);
                }
            }
        }

        public String MrgsId
        {
            get { return _mrgsId; }
            set
            {
                _mrgsId = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetMrgsId(_mrgsId);
                }
            }
        }

        public String MrgsUserId
        {
            get { return _mrgsUserId; }
            set
            {
                _mrgsUserId = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetMrgsUserId(_mrgsUserId);
                }
            }
        }

        public String OkId
        {
            get { return _okIds == null ? null : _okIds.FirstOrDefault(); }
            set
            {
                _okIds = value == null ? null : new[] { value };
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetOkIds(_okIds);
                }
            }
        }

        public String[] OkIds
        {
            get { return _okIds; }
            set
            {
                _okIds = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetOkIds(_okIds);
                }
            }
        }

        public String VkId
        {
            get { return _vkIds == null ? null : _vkIds.FirstOrDefault(); }
            set
            {
                _vkIds = value == null ? null : new[] { value };
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetVkIds(_vkIds);
                }
            }
        }

        public String[] VkIds
        {
            get { return _vkIds; }
            set
            {
                _vkIds = value;
                if (_customParamsProxy != null)
                {
                    _customParamsProxy.SetVkIds(_vkIds);
                }
            }
        }

        public void SetCustomParam(String key, String value)
        {
            if (String.IsNullOrEmpty(key))
            {
                return;
            }

            _customParams[key] = value;
            if (_customParamsProxy != null)
            {
                _customParamsProxy.SetCustomParam(key, value);
            }
        }

        public String GetCustomParam(String key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }

            return _customParams[key];
        }

        internal void SetCustomParamsProxy(ICustomParamsProxy proxy)
        {
            _customParamsProxy = proxy;
            if (_customParamsProxy == null)
            {
                return;
            }

            _customParamsProxy.SetAge(_age);
            _customParamsProxy.SetEmails(_emails);
            _customParamsProxy.SetGender(_gender);
            _customParamsProxy.SetIcqIds(_icqIds);
            _customParamsProxy.SetLang(_lang);
            _customParamsProxy.SetMrgsAppId(_mrgsAppId);
            _customParamsProxy.SetMrgsId(_mrgsId);
            _customParamsProxy.SetMrgsUserId(_mrgsUserId);
            _customParamsProxy.SetOkIds(_okIds);
            _customParamsProxy.SetVkIds(_vkIds);

            foreach (var item in _customParams)
            {
                _customParamsProxy.SetCustomParam(item.Key, item.Value);
            }
        }
    }
}