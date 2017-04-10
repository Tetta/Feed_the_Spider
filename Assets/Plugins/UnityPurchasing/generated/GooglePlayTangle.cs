#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/5eYUZK6hYr8bWfr1Q5aUbuxEjGLQnGaQhy6H22ZsfHbVEsvTNae6y37zF4MCG+u5w62gFJHrMFJykM+jyaJjZdBvr2/wJXNDHgrBmY1OMltX1scYAykx/WHqznATqx0qMIA88xPQU5+zE9ETMxPT07bTrFIMMrKXyd9dn2924qccsa3eFAyZ0mcUFtrt/c6rpHOvPlOyz0DZr+bZykPy/rilxYy37w4IxCrP8u9GPCVmhPLLXBUyiIlayZx8aafpRCWe/sU2zrtbsWH9o3n1vhnHgCWSQpMY+bHRH7MT2x+Q0hHZMgGyLlDT09PS05NPLUJH25j2TqL593e5uo6V5xLXzua5OcuGCE4ftngoL36/3/981ZFj6DJ08LNTPwD00xNT05P");
        private static int[] order = new int[] { 4,10,6,8,7,10,13,13,8,9,10,13,12,13,14 };
        private static int key = 78;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
