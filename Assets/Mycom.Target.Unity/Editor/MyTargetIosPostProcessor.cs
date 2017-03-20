#if UNITY_IOS
using System;
using System.IO;
using UnityEditor.iOS.Xcode;

namespace Mycom.Target.Unity.Editor
{
    public static class MyTargetIosPostProcessor
    {
        private const String AdSupportFramework = "AdSupport.framework";
        private const String AvFoundationFramework = "AVFoundation.framework";
        private const String CoreLocationFramework = "CoreLocation.framework";
        private const String CoreTelephonyFramework = "CoreTelephony.framework";
        private const String FrameworkSearchPathsKey = "FRAMEWORK_SEARCH_PATHS";
        private const String FrameworkSearchPathsInheritedValue = "$(inherited)";
        private const String FrameworkSearchPathsMyTargetValue = "$(PROJECT_DIR)/Frameworks/Mycom.Target.Unity/Plugins/iOS/libs";
        private const String InfoPlistFilePath = "Info.plist";
        private const String NsAllowsArbitraryLoadsKey = "NSAllowsArbitraryLoads";
        private const String NsAppTransportSecurityKey = "NSAppTransportSecurity";
        private const String StoreKitFramework = "StoreKit.framework";
        private const String SystemConfigurationFramework = "SystemConfiguration.framework";
        private const String TargetGuidName = "Unity-iPhone";
        private const String TestsTargetGuidName = "Unity-iPhone Tests";

        public static void Process(String pathToBuiltProject)
        {
            UpdatePbxproj(pathToBuiltProject);
            UpdatePlist(pathToBuiltProject);
        }

        private static void UpdatePbxproj(String pathToBuiltProject)
        {
            try
            {
                var filePath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

                var pbxProject = new PBXProject();
                pbxProject.ReadFromFile(filePath);

                {
                    var iphoneTargetGuid = pbxProject.TargetGuidByName(TargetGuidName);

                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, AdSupportFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, AvFoundationFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, CoreLocationFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, CoreTelephonyFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, StoreKitFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTargetGuid, SystemConfigurationFramework, true);

                    pbxProject.SetBuildProperty(iphoneTargetGuid, FrameworkSearchPathsKey, FrameworkSearchPathsInheritedValue);
                    pbxProject.AddBuildProperty(iphoneTargetGuid, FrameworkSearchPathsKey, FrameworkSearchPathsMyTargetValue);
                }

                {
                    var iphoneTestTargetGuid = pbxProject.TargetGuidByName(TestsTargetGuidName);

                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, AdSupportFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, AvFoundationFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, CoreLocationFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, CoreTelephonyFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, StoreKitFramework, true);
                    pbxProject.AddFrameworkToProject(iphoneTestTargetGuid, SystemConfigurationFramework, true);

                    pbxProject.SetBuildProperty(iphoneTestTargetGuid, FrameworkSearchPathsKey, FrameworkSearchPathsInheritedValue);
                    pbxProject.AddBuildProperty(iphoneTestTargetGuid, FrameworkSearchPathsKey, FrameworkSearchPathsMyTargetValue);
                }

                pbxProject.WriteToFile(filePath);
            }
            catch { }
        }

        private static void UpdatePlist(String pathToBuiltProject)
        {
            try
            {
                var fullPath = Path.Combine(pathToBuiltProject, InfoPlistFilePath);

                var plistDocument = new PlistDocument();
                plistDocument.ReadFromFile(fullPath);

                PlistElementDict nsAppTransportSecurityDict;
                PlistElement nsAppTransportSecurityElement;
                if (plistDocument.root.values.TryGetValue(NsAppTransportSecurityKey, out nsAppTransportSecurityElement))
                {
                    nsAppTransportSecurityDict = nsAppTransportSecurityElement.AsDict();
                }
                else
                {
                    nsAppTransportSecurityDict = new PlistElementDict();
                    plistDocument.root.values.Add(NsAppTransportSecurityKey, nsAppTransportSecurityDict);
                }

                nsAppTransportSecurityDict[NsAllowsArbitraryLoadsKey] = new PlistElementBoolean(true);
                plistDocument.WriteToFile(fullPath);
            }
            catch { }
        }
    }
} 
#endif