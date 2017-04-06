#import "UnityAppController.h"
#import "Localytics.h"

#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "UI/UnityView.h"
#import "UI/UnityViewControllerBase.h"

@interface LocalyticsAppController : UnityAppController {}
@end

@implementation LocalyticsAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    [Localytics autoIntegrate:@"6ade12378baa21672fee2f2-4c5a06ba-1a01-11e7-51ff-00acd5a75879" launchOptions:launchOptions];
 
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


- (BOOL)application:(UIApplication *)application
	openURL:(NSURL *)url
	sourceApplication:(NSString *)sourceApplication
	annotation:(id)annotation {

	NSLog(@"Received callback: %@", [url absoluteString]);
	NSString* urlData = url.query ? url.query : url.fragment;
	NSLog(@"Data: %@", urlData);
 
	NSMutableDictionary *args = [[NSMutableDictionary alloc] init];
	NSArray *data = [urlData componentsSeparatedByString:@"&"];
 
	NSLog(@"Params size: %lu", (unsigned long)[data count]);
 
	for (NSString *entry in data) {
		NSArray *keyValue = [entry componentsSeparatedByString:@"="];
		if ([keyValue count] != 2) {
			NSLog(@"Bad fragment data: %@", entry);
		} else {
			NSLog(@"Parsing args: %@ -> %@", [keyValue firstObject], [keyValue lastObject]);
			[args setObject:[keyValue lastObject] forKey: [keyValue firstObject]];
		}
	}
 
	NSLog(@"Args size: %lu", (unsigned long)[args count]);
	NSString *token = [args objectForKey:@"access_token"];
	NSString *key = [args objectForKey:@"session_secret_key"];
	NSString *expiresIn = [args objectForKey:@"expires_in"];
	NSString *refreshToken = [args objectForKey:@"refresh_token"];
	NSString *code = [args objectForKey:@"code"];
	NSString *error = [args objectForKey:@"error"];
	
	if (error) {
		UnitySendMessage("Odnoklassniki", "AuthFailed", [error UTF8String]);
	} else if (key) {
		NSLog(@"Extracted token=%@ , key=%@ , expiresIn=%@", token, key, expiresIn);
		NSString* message = [NSString stringWithFormat:@"%@;%@;%@", token, key, expiresIn];
		UnitySendMessage("Odnoklassniki", "AuthSuccessIOS", [message UTF8String]);
	} else {
		NSLog(@"Extracted token=%@ , no key, refreshToken=%@ , expiresIn=%@", token, refreshToken, expiresIn);
		NSString* message = [NSString stringWithFormat:@"%@;%@;%@", token, refreshToken, expiresIn];
		UnitySendMessage("Odnoklassniki", "AuthSuccessIOS", [message UTF8String]);
	}

	return [super application:application
		openURL:url
		sourceApplication:sourceApplication
		annotation:annotation];
}

@end

IMPL_APP_CONTROLLER_SUBCLASS( LocalyticsAppController )
