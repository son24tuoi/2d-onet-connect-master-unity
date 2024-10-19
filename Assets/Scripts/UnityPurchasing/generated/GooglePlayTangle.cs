// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VIxBwakN9hImmKy/+oEZwQYPSNoQUeAHePjzTFSSq8Rvi7BWNnrMFi+BdpHzruD7mExG/bm7WGjtnci4vHX2JSkjbuSxdF/hIGt5fUhvCUMi27HBscOf6Uo3eWmzf6B3QGa33cqPf+dMa7sYYQMO2d4iM2oGAA6A1p2+L1dNAT+zfyFsjCI6vBXzAR8DWwf/q7fJrX0EjmTyUy/JFAYCSfpIy+j6x8zD4EyCTD3Hy8vLz8rJSMvFyvpIy8DISMvLygSXTk4+pvsdf0ougBY6xQOsL+R/hVSMuWIrgTpYiuK91UiUpLmdlocNaAyok1M1guW4mCAdAwC61eRQcFaVRAUyG32awaFhm/PVfzDbSVxHTjFW17JXqgOYl4Na56tkCcjJy8rL");
        private static int[] order = new int[] { 2,8,10,12,12,9,9,7,10,10,13,11,13,13,14 };
        private static int key = 202;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
