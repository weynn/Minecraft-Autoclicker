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

            Task.Run(() => GetSlots());
            Task.Run(() => DoLeftClick());
            Task.Run(() => DoRightClick());
            Task.Run(() => Randomisation());
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WinApi.ReleaseCapture();
                WinApi.SendMessage(Handle, WinApi.WM_NCLBUTTONDOWN, WinApi.HT_CAPTION, 0);
            }
        }



        #region Sliders event

        private void sldLeftCPS_ValueChanged(object sender, EventArgs e) => lbLeftCPS.Text = $"{"CPS: " + sldLeftCPS.Value}";
        private void sldRightCPS_ValueChanged(object sender, EventArgs e) => lbRightCPS.Text = $"{"CPS: " + sldRightCPS.Value}";

        #endregion



        #region Binds


        #region Set binds
        private int leftBind = 0;
        private void btBindLeft_Click(object sender, EventArgs e) => btBindLeft.Text = "[...]";

        private void btBindLeft_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    leftBind = 0;
                    btBindLeft.Text = "[NONE]";
                    break;

                default:
                    leftBind = (int)e.KeyCode;
                    btBindLeft.Text = "[" + e.KeyCode + "]";
                    break;
            }
        }


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
            if (WinApi.GetAsyncKeyState(leftBind) != 0) tgLeft.Checked = !tgLeft.Checked;

            if (WinApi.GetAsyncKeyState(rightBind) != 0) tgRight.Checked = !tgRight.Checked;
        }
        #endregion


        #endregion



        #region Slot whitelist

        private byte currentSlot = 1;
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

        private int randomisedCPSL = 10;
        private int randomisedCPSR = 10;
        private async void Randomisation()
        {
            /* basic randomisation system */

            for (;;)
            {
                await Task.Delay(1000);

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
                await Task.Delay(1000 / randomisedCPSL);

                MCHelper.GetMinecraftWindow();

                if ((cbMenus.Checked && !ClickerExtensionHandle.InMenu()) || !cbMenus.Checked)
                    LeftConds();
            }
        }

        private void LeftConds()
        {
            if (IsWhitelistedLeft() && tgLeft.Checked) // checks if current slot is whitelisted AND if left clicker is toggled
            {
                if (cbShiftLeft.Checked && WinApi.GetAsyncKeyState(Keys.LShiftKey) != 0) return; // if shift disabled is checked and if its holding shift, return

                if ((!cbRMB.Checked && WinApi.GetAsyncKeyState(WinApi.VK_LBUTTON) < 0) || (cbRMB.Checked && MouseButtons == MouseButtons.Left))
                    if (!cbBBlocks.Checked)
                        LeftClicker.SendMessageLeftClick();
                    else
                        LeftClicker.SendMessageLeftClickBreakBlocks();
            }
        }

        #endregion



        #region Right clicker

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

            label1.Dispose();
            label2.Dispose();
            label5.Dispose();
            label6.Dispose();
            label7.Dispose();
            label8.Dispose();
            label9.Dispose();
            label10.Dispose();
            label11.Dispose();

            pnLeft.Dispose();
            pnRight.Dispose();
            pnCategory.Dispose();
            pictureBox1.Dispose();

            pbDestruct.Dispose();
            pbMain.Dispose();

            tgLeft.Dispose();
            tgRight.Dispose();

            cbBBlocks.Dispose();
            cbMenus.Dispose();
            cbRMB.Dispose();
            cbShiftLeft.Dispose();

            cbShiftRight.Dispose();

            slotL1.Dispose();
            slotL2.Dispose();
            slotL3.Dispose();
            slotL4.Dispose();
            slotL5.Dispose();
            slotL6.Dispose();
            slotL7.Dispose();
            slotL8.Dispose();
            slotL9.Dispose();

            slotR1.Dispose();
            slotR2.Dispose();
            slotR3.Dispose();
            slotR4.Dispose();
            slotR5.Dispose();
            slotR6.Dispose();
            slotR7.Dispose();
            slotR8.Dispose();
            slotR9.Dispose();

            btBindLeft.Dispose();
            btBindRight.Dispose();

            Task.Delay(1000).Wait();

            this.Dispose();
            Environment.Exit(0);
        }

        #endregion
    }
}
