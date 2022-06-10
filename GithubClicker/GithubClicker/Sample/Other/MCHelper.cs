using System;

namespace GithubClicker.Sample.Other
{
    public class MCHelper
    {
        public static IntPtr hwnd;

        public static void GetMinecraftWindow() => hwnd = WinApi.FindWindow("LWJGL", null);

    }
}
