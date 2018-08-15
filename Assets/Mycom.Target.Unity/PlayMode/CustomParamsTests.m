#import <MyTargetSDK/MTRGCustomParams.h>

extern MTRGCustomParams *mtrg_unity_customParamsWithAdId(uint32_t adId);
extern const char *mtrg_unity_makeStringCopy(NSString *);

const char *MTRGCustomParamsGetAge(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy([customParams.age stringValue]) : NULL;
}

const char *MTRGCustomParamsGetEmail(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.email) : NULL;
}

const char *MTRGCustomParamsGetGender(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy([@(customParams.gender) stringValue]) : NULL;
}

const char *MTRGCustomParamsGetIcqId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.icqId) : NULL;
}

const char *MTRGCustomParamsGetLang(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.language) : NULL;
}

const char *MTRGCustomParamsGetMrgsAppId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.mrgsAppId) : NULL;
}

const char *MTRGCustomParamsGetMrgsId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.mrgsDeviceId) : NULL;
}

const char *MTRGCustomParamsGetMrgsUserId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.mrgsUserId) : NULL;
}

const char *MTRGCustomParamsGetOkId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.okId) : NULL;
}

const char *MTRGCustomParamsGetVkId(uint32_t adId)
{
    MTRGCustomParams *customParams = mtrg_unity_customParamsWithAdId(adId);
    return customParams ? mtrg_unity_makeStringCopy(customParams.vkId) : NULL;
}