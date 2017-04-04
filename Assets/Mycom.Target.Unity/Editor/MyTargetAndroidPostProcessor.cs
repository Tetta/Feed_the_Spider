#if UNITY_ANDROID
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Mycom.Target.Unity.Editor
{
    public static class MyTargetAndroidPostprocessor
    {
        private const String ActivityTag = "activity";
        private const String AndroidPermissionAccessNetworkStateValue = "android.permission.ACCESS_NETWORK_STATE";
        private const String AndroidPermissionInternetValue = "android.permission.INTERNET";
        private const String AndroidSchemaValue = "http://schemas.android.com/apk/res/android";
        private const String ApplicationTag = "application";
        private const String ConfigChangesAttribute = "configChanges";
        private const String ConfigChangesValue = "keyboard|keyboardHidden|orientation|screenLayout|uiMode|screenSize|smallestScreenSize";
        private const String HardwareAcceleratedAttribute = "hardwareAccelerated";
        private const String HardwareAcceleratedValue = "true";
        private const String ManifestTag = "manifest";
        private const String MyTargeActivityValue = "com.my.target.ads.MyTargetActivity";
        private const String NameAttribute = "name";
        private const String SourceManifestPath = "Mycom.Target.Unity/Plugins/Android/AndroidManifest.xml";
        private const String TargetManifestPath = "Plugins/Android/AndroidManifest.xml";
        private const String TargetPath = "Plugins/Android/";
        private const String UsesPermissionTag = "uses-permission";

        public static void Process()
        {
            try
            {
                var targetManifestPath = Path.Combine(Application.dataPath, TargetManifestPath);
                var mustRecompile = false;

                if (File.Exists(targetManifestPath))
                {
                    XDocument xDocument;

                    using (var streamReader = new StreamReader(targetManifestPath))
                    {
                        xDocument = XDocument.Load(streamReader);

                        var manifestElement = xDocument.Descendants()
                                                       .FirstOrDefault(element => element.Name == ManifestTag);

                        if (manifestElement == null)
                        {
                            mustRecompile = true;
                        }
                        else
                        {
                            if (!manifestElement.Elements(UsesPermissionTag)
                                                .Any(element => element.Attributes(XName.Get(NameAttribute, AndroidSchemaValue))
                                                                       .Any(attribute => attribute.Value == AndroidPermissionInternetValue)))
                            {
                                mustRecompile = true;

                                var internetPermissionAttribute = new XAttribute(XName.Get(NameAttribute, AndroidSchemaValue), AndroidPermissionInternetValue);
                                var internetPermissionElement = new XElement(UsesPermissionTag);
                                internetPermissionElement.Add(internetPermissionAttribute);
                                manifestElement.Add(internetPermissionElement);
                            }

                            if (!manifestElement.Elements(UsesPermissionTag)
                                                .Any(element => element.Attributes(XName.Get(NameAttribute, AndroidSchemaValue))
                                                                       .Any(attribute => attribute.Value == AndroidPermissionAccessNetworkStateValue)))
                            {
                                mustRecompile = true;

                                var accessNetworkStateAttribute = new XAttribute(XName.Get(NameAttribute, AndroidSchemaValue), AndroidPermissionAccessNetworkStateValue);
                                var accessNetworkStateElement = new XElement(UsesPermissionTag);
                                accessNetworkStateElement.Add(accessNetworkStateAttribute);
                                manifestElement.Add(accessNetworkStateElement);
                            }

                            var applicationElement = xDocument.Descendants()
                                                              .FirstOrDefault(element => element.Name == ApplicationTag);

                            if (applicationElement == null)
                            {
                                mustRecompile = true;
                            }
                            else
                            {
                                var currentMyTargetActivityElement = applicationElement.Elements(ActivityTag)
                                                                                       .FirstOrDefault(element => element.Attributes(XName.Get(NameAttribute, AndroidSchemaValue))
                                                                                                                         .Any(attribute => attribute.Value == MyTargeActivityValue));
                                if (currentMyTargetActivityElement == null)
                                {
                                    mustRecompile = true;

                                    var myTargetActivityElement = new XElement(ActivityTag);
                                    myTargetActivityElement.Add(new XAttribute(XName.Get(NameAttribute, AndroidSchemaValue), MyTargeActivityValue));
                                    myTargetActivityElement.Add(new XAttribute(XName.Get(ConfigChangesAttribute, AndroidSchemaValue), ConfigChangesValue));
                                    myTargetActivityElement.Add(new XAttribute(XName.Get(HardwareAcceleratedAttribute, AndroidSchemaValue), HardwareAcceleratedValue));
                                    applicationElement.Add(myTargetActivityElement);
                                }
                                else
                                {
                                    var hardwareAcceleratadAttribute = currentMyTargetActivityElement.Attributes(XName.Get(HardwareAcceleratedAttribute, AndroidSchemaValue))
                                                                                                     .FirstOrDefault();
                                    if (hardwareAcceleratadAttribute == null || hardwareAcceleratadAttribute.Value.ToLower() != HardwareAcceleratedValue)
                                    {
                                        throw new AndroidManifestException("Error in AndroidManifest.xml. Declaration of com.my.target.ads.MyTargetActivity must have attribute: android:hardwareAccelerated=\"true\".");
                                    }
                                }
                            }
                        }
                    }

                    xDocument.Save(targetManifestPath);
                }
                else
                {
                    mustRecompile = true;

                    var directoryPath = Path.Combine(Application.dataPath, TargetPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    File.Copy(Path.Combine(Application.dataPath, SourceManifestPath), targetManifestPath);
                }

                if (mustRecompile)
                {
                    throw new AndroidManifestException("AndroidManifest.xml changed. Your must rebuild project.");
                }
            }
            catch (AndroidManifestException)
            {
                throw;
            }
            catch { }
        }

        private class AndroidManifestException : Exception
        {
            public AndroidManifestException(String message)
                : base(message) { }
        }
    }
}

#endif