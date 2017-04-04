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
    [PostProcessBuild(Int32.MaxValue-2)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
#if UNITY_IOS
 

#endif
        }
    }
}