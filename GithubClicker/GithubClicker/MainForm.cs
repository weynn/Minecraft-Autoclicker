using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GithubClicker.Sample.Other;
using GithubClicker.Sample.Combat;

namespace GithubClicker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Region = Region.FromHrgn(WinApi.CreateRoundRectRgn(0, 0, Width, Height, 15, 15)); /* create rounded borders on form */

            /* initializes tasks */
            Task.Run(() => GetSlots());
            Task.Run(() => DoLeftClick());
            Task.Run(() => DoRightClick());
            Task.Run(() => Randomisation());
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            /* moves the gui when left click is held */
            if (e.Button == MouseButtons.Left)
            {
                WinApi.ReleaseCapture();
                WinApi.SendMessage(Handle, WinApi.WM_NCLBUTTONDOWN, WinApi.HT_CAPTION, 0);
            }
        }



        #region Sliders event

        /* updates label texts according to the slider value */
        private void sldLeftCPS_ValueChanged(object sender, EventArgs e) => lbLeftCPS.Text = $"{"CPS: " + sldLeftCPS.Value}";
        private void sldRightCPS_ValueChanged(object sender, EventArgs e) => lbRightCPS.Text = $"{"CPS: " + sldRightCPS.Value}";

        #endregion



        #region Binds


        #region Set binds

        private int leftBind = 0; /* 0 = no key */

        private void btBindLeft_Click(object sender, EventArgs e) => btBindLeft.Text = "[...]";

        private void btBindLeft_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    /* set to no key, because escape has been pressed*/
                    leftBind = 0;
                    btBindLeft.Text = "[NONE]";
                    break;

                default: /* default = any key that has been pressed */
                    leftBind = (int)e.KeyCode; /* set leftBind to the KeyCode (bind) */
                    btBindLeft.Text = "[" + e.KeyCode + "]";
                    break;
            }
        }


        /* doing the exact same but for right */
        private int rightBind = 0;

        private void btBindRight_Click(object sender, EventArgs e) => btBindRight.Text = "[...]";

        private void btBindRight_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    rightBind = 0;
                    btBindRight.Text = "[NONE]";
                    break;

                default:
                    rightBind = (int)e.KeyCode;
                    btBindRight.Text = "[" + e.KeyCode + "]";
                    break;
            }
        }
        #endregion



        #region Binds timer
        private void Binding_Tick(object sender, EventArgs e)
        {
            if (WinApi.GetAsyncKeyState(leftBind) != 0) tgLeft.Checked = !tgLeft.Checked; /* if key held is leftBind, then it will unable / disable toggle */

            if (WinApi.GetAsyncKeyState(rightBind) != 0) tgRight.Checked = !tgRight.Checked; /* if key held is rightBind, then it will unable / disable toggle */
        }
        #endregion


        #endregion



        #region Slot whitelist

        private byte currentSlot = 1; /* default current slot set to 1 */
        private async void GetSlots()
        {
            for (;;)
            {
                await Task.Delay(50);

                GetKeyPressed(); /* get pressed key to get current slot position */

                IsWhitelistedLeft(); /* bool checking if the current left slot is whitelisted */
                IsWhitelistedRight(); /* same but for right click */
            }
        }


        private void GetKeyPressed()
        {
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS1) != 0) currentSlot = 1;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS2) != 0) currentSlot = 2;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS3) != 0) currentSlot = 3;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS4) != 0) currentSlot = 4;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS5) != 0) currentSlot = 5;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS6) != 0) currentSlot = 6;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS7) != 0) currentSlot = 7;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS8) != 0) currentSlot = 8;
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS9) != 0) currentSlot = 9;
        }



        private bool IsWhitelistedLeft()
        {
            switch (currentSlot)
            {
                /* switch statement, if current slot = 1, it will check for tgLeft checked and slot checked, if they are false, the slot will be considered as unwhitelisted */
                case 1: return tgLeft.Checked && slotL1.Checked;
                case 2: return tgLeft.Checked && slotL2.Checked;
                case 3: return tgLeft.Checked && slotL3.Checked;
                case 4: return tgLeft.Checked && slotL4.Checked;
                case 5: return tgLeft.Checked && slotL5.Checked;
                case 6: return tgLeft.Checked && slotL6.Checked;
                case 7: return tgLeft.Checked && slotL7.Checked;
                case 8: return tgLeft.Checked && slotL8.Checked;
                case 9: return tgLeft.Checked && slotL9.Checked;
            }
            return false;
        }

        private bool IsWhitelistedRight()
        {
            switch (currentSlot)
            {
                /* same thing but for right clicker */
                case 1: return tgRight.Checked && slotR1.Checked;
                case 2: return tgRight.Checked && slotR2.Checked;
                case 3: return tgRight.Checked && slotR3.Checked;
                case 4: return tgRight.Checked && slotR4.Checked;
                case 5: return tgRight.Checked && slotR5.Checked;
                case 6: return tgRight.Checked && slotR6.Checked;
                case 7: return tgRight.Checked && slotR7.Checked;
                case 8: return tgRight.Checked && slotR8.Checked;
                case 9: return tgRight.Checked && slotR9.Checked;
            }
            return false;
        }

        #endregion



        #region Randomisation

        private int randomisedCPSL = 10; /* setting a variable for the randomisation */
        private int randomisedCPSR = 10;
        private async void Randomisation()
        {
            /* basic randomisation system */

            for (;;)
            {
                await Task.Delay(1000);

                /* randomises the int value in a range of -3, 3 */
                randomisedCPSL = new Random(Guid.NewGuid().GetHashCode()).Next((int)sldLeftCPS.Value - 3, (int)sldLeftCPS.Value + 3);
                randomisedCPSR = new Random(Guid.NewGuid().GetHashCode()).Next((int)sldRightCPS.Value - 3, (int)sldRightCPS.Value + 3);
            }
        }

        #endregion



        #region Left clicker

        private async void DoLeftClick()
        {
            for (;;)
            {
                await Task.Delay(1000 / randomisedCPSL); /* gets the delay interval for cps, 1000 / 10 (cps) for example will get 100 delay, 100 x 10 = 1000 (ms) ; 1000ms = 1 second, so basically avergae cps per second */

                MCHelper.GetMinecraftWindow(); /* gets minecraft process */

                if ((cbMenus.Checked && !ClickerExtensionHandle.InMenu()) || !cbMenus.Checked) /* checks if "Disable in menu" checkbox is checked and if you arent in menu OR if "Disable in menu" checkbox is unchecked without checking if u are in menus */
                    LeftConds(); /* gets in the conditions */
            }
        }

        private void LeftConds()
        {
            if (IsWhitelistedLeft() && tgLeft.Checked) // checks if current slot is whitelisted AND if left clicker is toggled
            {
                if (cbShiftLeft.Checked && WinApi.GetAsyncKeyState(Keys.LShiftKey) != 0) return; // if shift disabled is checked and if its holding shift, return

                if ((!cbRMB.Checked && WinApi.GetAsyncKeyState(WinApi.VK_LBUTTON) < 0) || (cbRMB.Checked && MouseButtons == MouseButtons.Left)) /* if rmb lock checkbox isnt checked and if virtual key is pressed, OR checks if rmb lock checkbox is checked and if key is pressed */
                    if (!cbBBlocks.Checked) /* if break blocks checkbox is unchecked */
                        LeftClicker.SendMessageLeftClick(); /* send a normal click */
                    else
                        LeftClicker.SendMessageLeftClickBreakBlocks(); /* else break blocks */
            }
        }

        #endregion



        #region Right clicker

        /* same exact procedure as left clicker but for the right clicker :) */
        private async void DoRightClick()
        {
            for (;;)
            {
                await Task.Delay(1000 / randomisedCPSR);

                MCHelper.GetMinecraftWindow();

                RightConds();
            }
        }

        private void RightConds()
        {
            if (IsWhitelistedRight() && tgRight.Checked) // checks if current slot is whitelisted AND if right clicker is toggled
            {
                if (cbShiftRight.Checked && WinApi.GetAsyncKeyState(Keys.LShiftKey) != 0) return;

                if (WinApi.GetAsyncKeyState(WinApi.VK_RBUTTON) < 0)
                    RightClicker.SendMessageRightClick();
            }
        }

        #endregion




        #region Destruct

        private void pbDestruct_Click(object sender, EventArgs e)
        {
            /* super basic destruct */

            foreach (Control currentControl in Controls)
            {
                currentControl.Dispose(); /* gets each control in Controls and disposes them */
            }
            
            Task.Delay(1000).Wait();

            this.Dispose();
            Environment.Exit(0); /* exit */
        }

        #endregion
    }
}
