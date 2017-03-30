#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("nOLhKB4nPnjf5qa7/Pl5+/VQQ4n5kZ5XlLyDjPprYe3TCFxXvbcUN+tow4Hwi+HQ/mEYBpBPDEpl4MFCK3ZSzCQjbSB396CZoxaQff0S3Tw6sw8ZaGXfPI3h29jg7DxRmk1ZPVkhe3B7u92MmnTAsX5WNGFPmlZda1ldGmYKosHzga0/xkiqcq7EBvX85JEQNNm6PiUWrTnNux72k5wVzY1Ed5xEGrwZa5+3991STSlK0JjteMpJanhFTkFizgDOv0VJSUlNSEsr/cpYCg5pqOEIsIZUQarHT8xFOIkgj4uRR7i7ucaTywp+LQBgMz7PbbHxPKiXyLr/SM07BWC5nWEvCc3KSUdIeMpJQkrKSUlI3Ui3TjbMzKbP1cTLSvoF1UpLSUhJ");
        private static int[] order = new int[] { 11,4,5,9,12,13,7,9,10,11,11,11,13,13,14 };
        private static int key = 72;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
