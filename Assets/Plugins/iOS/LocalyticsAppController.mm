#import "UnityAppController.h"
#import "Localytics.h"

@interface LocalyticsAppController : UnityAppController {}
@end

@implementation LocalyticsAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    [Localytics autoIntegrate:@"fc631941d160eeb3ae14250-16d5b2ca-e200-11e6-8aad-008cc99655f0" launchOptions:launchOptions];
 
    // If you are using Localytics Messaging include the following code to register for push notifications
    if ([application respondsToSelector:@selector(registerUserNotificationSettings:)])
    {
        UIUserNotificationType types = (UIUserNotificationTypeAlert | UIUserNotificationTypeBadge | UIUserNotificationTypeSound);
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:types categories:nil];
        [application registerUserNotificationSettings:settings];
        [application registerForRemoteNotifications];
    }
    else
    {
        [application registerForRemoteNotificationTypes:(UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound)];
    }
    
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    return YES;
}

- (void)application:(UIApplication *)application handleWatchKitExtensionRequest:(NSDictionary *)userInfo reply:(void (^)(NSDictionary *))reply
{
    [Localytics handleWatchKitExtensionRequest:userInfo reply:reply];
}


@end

IMPL_APP_CONTROLLER_SUBCLASS( LocalyticsAppController )
