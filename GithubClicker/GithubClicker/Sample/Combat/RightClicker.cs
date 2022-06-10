using System.Threading.Tasks;
using GithubClicker.Sample.Other;

namespace GithubClicker.Sample.Combat
{
    public class RightClicker
    {
        public async static void SendMessageRightClick()
        {
            WinApi.SendMessage(MCHelper.hwnd, WinApi.WM_RBUTTONDOWN, 0, 0);
            await Task.Delay(10);
            WinApi.SendMessage(MCHelper.hwnd, WinApi.WM_RBUTTONUP, 0, 0);
        }
    }
}
