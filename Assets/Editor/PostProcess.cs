using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class PbxModifier
{
    [PostProcessBuild(Int32.MaxValue-10)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string target = proj.TargetGuidByName("Unity-iPhone"); 
            string file = proj.FindFileGuidByProjectPath("Libraries/Plugins/iOS/KiiIOSSocialNetworkConnector.mm"); 
            var flags = proj.GetCompileFlagsForFile(target, file);
            flags.Add("-fno-objc-arc");
            proj.SetCompileFlagsForFile(target, file, flags);

            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
}