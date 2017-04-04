#if UNITY_IOS || UNITY_ANDROID
using System;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Mycom.Target.Unity.Editor
{
    public static class MyTargetfPostprocessor
    {
        [PostProcessBuild(-1)]
        public static void OnPostprocessBuild(BuildTarget target, String pathToBuiltProject)
        {
            switch (target)
            {
#if UNITY_IOS
		        case BuildTarget.iOS:
                    MyTargetIosPostProcessor.Process(pathToBuiltProject);
                    break;  
#endif
#if UNITY_ANDROID
                case BuildTarget.Android:
                    MyTargetAndroidPostprocessor.Process();
                    break;
#endif
            }
        }
    }
}

#endif