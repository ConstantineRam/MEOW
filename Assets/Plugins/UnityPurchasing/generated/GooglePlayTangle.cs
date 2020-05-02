#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iEi/5U8wWNE7sGT3ZMLftd2A3ywITodW3eHhzSyFAm4JVtndOsZb1bX+hC2q59Xemz7CtMDQmLurE6IrC3eRPehWDCP2B2qCMmpsfuf8xvscC9v3u0LQTBliwNEZGGVbJQrnUsNATkFxw0BLQ8NAQEH/GJcEqqyQU2z4+Da41rBsl95HymBeOUbHycLgVOTJ/uD8qnMygWhsl0sb8L3DgnHDQGNxTEdIa8cJx7ZMQEBAREFCulyLI+/4oy1fYcPJok48MujqRqP8dAXssSt0AJIGjlQSbAF7kVz7Imab6Zpd19p0e+VceYxJSZ/Et+zyLmZeAao+E5fa/jkG0u0nEod3c2M5wNbp+2NQT6qyINmcNaf24qO+TIUUBVF87ByK7kNCQEFA");
        private static int[] order = new int[] { 2,13,7,11,9,13,12,10,10,12,10,13,13,13,14 };
        private static int key = 65;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
