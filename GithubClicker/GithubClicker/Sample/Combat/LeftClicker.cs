using System.Threading.Tasks;
using GithubClicker.Sample.Other;

namespace GithubClicker.Sample.Combat
{
    public class LeftClicker
    {
        public async static void SendMessageLeftClick()
        {
            WinApi.SendMessage(MCHelper.hwnd, 0x201, 0, 0);
            await Task.Delay(10);
            WinApi.SendMessage(MCHelper.hwnd, 0x202, 0, 0);
        }

        public static void SendMessageLeftClickBreakBlocks()
        {
            WinApi.SendMessage(MCHelper.hwnd, 0x202, 0, 0);
            WinApi.SendMessage(MCHelper.hwnd, 0x201, 0, 0);
        }
    }
}
