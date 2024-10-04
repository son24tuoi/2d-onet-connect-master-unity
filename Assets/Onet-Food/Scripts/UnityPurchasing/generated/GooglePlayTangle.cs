// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("NvzW2GvlraC5KVJsVzOnQTanzYP5Vi82sgwf/CZnsv/bakYgdm00tPNJzsI0At60BtzF8FsMOddcjbJgzRjuxh73j/vA8B+nr2mvL+VJ1YBYprFlXRjc8B5IEZzL6ZclIX1Cnt34rtWuJgD1KbfrZiWkCeLgzWlv0oNZRJZG2HFv5/EqlmWK84xW5ySuPW/cnEpXk2sU5bjKiaHpTgXAgakbmLuplJ+Qsx/RH26UmJiYnJmaG5iWmakbmJObG5iYmTpJ1wMAO0sxH8IVR2eZc3V+d2wwUqSRJAI0IIMHDy2OHEnK5Lk76TfY9bs1HrZu8J71YEcIa7bTScM0uuv0/yhMhejCqoQXRJsgyNI1CvdknfiDQXmYHf+3un1Sq9IQGJuamJmY");
        private static int[] order = new int[] { 13,2,10,7,9,6,9,7,13,10,10,11,13,13,14 };
        private static int key = 153;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
