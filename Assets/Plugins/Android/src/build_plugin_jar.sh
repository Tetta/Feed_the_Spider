#!/bin/sh

# RUN THIS FROM COMMAND LINE IF YOU CHANGE AppsFlyerOverrideActivity.java

echo "Handle Android jar"
 

echo "Compiling ..."

#javac UnityNotificationManager.java -classpath $CLASSPATH -d .
javac -source 1.6 -target 1.6 -cp "C:/Program Files/Unity5.5/Editor/Data/PlaybackEngines/AndroidPlayer/Variations/mono/Release/Classes/classes.jar;C:/Android/sdk/platforms/android-24/android.jar;C:/projects/Feed_the_Spider/Assets/Plugins/Android/localytics.jar; C:/projects/Feed_the_Spider/Assets/Plugins/Android/localytics-unity.jar" -d .  UnityNotificationManager.java
#javac -source 1.6 -target 1.6 -cp "C:/Program Files/Unity5.5/Editor/Data/PlaybackEngines/AndroidPlayer/Variations/mono/Release/Classes/classes.jar;C:/Android/sdk/platforms/android-24/android.jar;C:/projects/Feed_the_Spider/Assets/Plugins/Android/localytics.jar;C:/projects/Feed_the_Spider/Assets/Plugins/Android/localytics-unity.jar" -d .     FeederUnityPlayerNativeActivity.java

#javap -s com.appsflyer.AppsFlyerOverrideActivity
#javap -s com.appsflyer.AppsFlyerUnityHelper
javap -s net.agasper.unitynotification.UnityNotificationManager
#javap -s net.FeederUnityPlayerNativeActivity
echo "Manifest-Version: 1.0" > MANIFEST.MF

echo "Creating jar file..."
jar cvfM ../unitynotification.jar net/ com/

echo ""
echo "Done!"
