using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class PbxModifier
{
    [PostProcessBuild(Int32.MaxValue-10)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
#if UNITY_IOS
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string target = proj.TargetGuidByName("Unity-iPhone"); 

			proj.SetBuildProperty (target, "ENABLE_BITCODE", "false");
			proj.SetBuildProperty (target, "CLANG_ALLOW_NON_MODULAR_INCLUDES_IN_FRAMEWORK_MODULES", "yes");
			proj.AddFrameworkToProject (target, "CoreData.framework", false);
            proj.SetCompileFlagsForFile(target, file, flags);

            File.WriteAllText(projPath, proj.WriteToString());


            string plistPath = path + "/Info.plist";
			var txt = File.ReadAllText(plistPath);

			txt = txt.Replace("<key>LSApplicationQueriesSchemes</key>",@"
<key>NSPhotoLibraryUsageDescription</key>
<string>This app requires access to the photo library.</string>
<key>NSCameraUsageDescription</key>
<string>This app requires access to the camera</string>			
<key>LSApplicationQueriesSchemes</key>");
            // Write to file
            File.WriteAllText(plistPath, txt);

#endif
        }
    }
}