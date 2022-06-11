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
        readonly Random rndSeed = new Random(Guid.NewGuid().GetHashCode());

        public MainForm()
        {
            InitializeComponent();

            Region = Region.FromHrgn(WinApi.CreateRoundRectRgn(0, 0, Width, Height, 15, 15)); /* create rounded borders on form */

            Task.Run(() => GetSlots());         /* runs a new task after MainForm has finished intializing components, which will basically run in a loop and always be on */
            Task.Run(() => DoLeftClick());      /* ..... */
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

        private void sldLeftCPS_ValueChanged(object sender, EventArgs e) => lbLeftCPS.Text = $"{"CPS: " + sldLeftCPS.Value}";       /* lbLeftCPS is a label, the Text will change once sldLeftCPS has its value changed, the text will be updated to "CPS: " + the sldLeftCPS Value */
        private void sldRightCPS_ValueChanged(object sender, EventArgs e) => lbRightCPS.Text = $"{"CPS: " + sldRightCPS.Value}";    /* same thing but for the right clicker label */

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
                    leftBind = 0; /* set leftBind to 0, because escape has been pressed*/
                    btBindLeft.Text = "[NONE]"; /* set to no key too */
                    break; /* breaks out */

                default: /* default = any key that has been pressed */
                    leftBind = (int)e.KeyCode; /* set leftBind to the KeyCode (bind) */
                    btBindLeft.Text = "[" + e.KeyCode + "]";
                    break; /* breaks out */
            }
        }


        /*                                          */
        /* doing the exact same thing but for right */
        /*                                          */
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
            for (;;) /* will loop and never stop, until it breaks */
            {
                await Task.Delay(50);

                GetKeyPressed();        /* get pressed key to get current slot position */

                IsWhitelistedLeft();    /* bool checking if the current left slot is whitelisted */
                IsWhitelistedRight();   /* same but for right click */
            }
        }


        private void GetKeyPressed()
        {
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS1) != 0) currentSlot = 1; /* if get pressed is D1, current slot = 1 */
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS2) != 0) currentSlot = 2; /* if get pressed is D2, current slot = 2 */
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS3) != 0) currentSlot = 3; /* if get pressed is D3, current slot = 3 */
            if (WinApi.GetAsyncKeyState(DefaultKeys.keyS4) != 0) currentSlot = 4; /* ........ */
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
                case 1: return tgLeft.Checked && slotL1.Checked; /* if current slot = 1, it'll check if tgLeft AND slotL1 is checked, if not, slot is considered unwhitelisted */
                case 2: return tgLeft.Checked && slotL2.Checked; /* if current slot = 2, it'll check if tgLeft AND slotL2 is checked, if not, slot is considered unwhitelisted */
                case 3: return tgLeft.Checked && slotL3.Checked; /* if current slot = 3, it'll check if tgLeft AND slotL3 is checked, if not, slot is considered unwhitelisted */
                case 4: return tgLeft.Checked && slotL4.Checked; /* if current slot = 4, it'll check if tgLeft AND slotL4 is checked, if not, slot is considered unwhitelisted */
                case 5: return tgLeft.Checked && slotL5.Checked; /* ........ */
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
                case 1: return tgRight.Checked && slotR1.Checked; /* if current slot = 1, it'll check if tgRight AND slotR1 is checked, if not, slot is considered unwhitelisted */
                case 2: return tgRight.Checked && slotR2.Checked; /* if current slot = 2, it'll check if tgRight AND slotR2 is checked, if not, slot is considered unwhitelisted */
                case 3: return tgRight.Checked && slotR3.Checked; /* if current slot = 3, it'll check if tgRight AND slotR3 is checked, if not, slot is considered unwhitelisted */
                case 4: return tgRight.Checked && slotR4.Checked; /* if current slot = 4, it'll check if tgRight AND slotR4 is checked, if not, slot is considered unwhitelisted */
                case 5: return tgRight.Checked && slotR5.Checked; /* ........ */
                case 6: return tgRight.Checked && slotR6.Checked;
                case 7: return tgRight.Checked && slotR7.Checked;
                case 8: return tgRight.Checked && slotR8.Checked;
                case 9: return tgRight.Checked && slotR9.Checked;
            }
            return false;
        }

        #endregion



        #region Randomisation

        private int randomisedCPSL = 10; /* setting a variable for the randomisation, that will be used for the average cps */
        private int randomisedCPSR = 10;
        private async void Randomisation()
        {
            /* basic randomisation system */

            for (;;) /* will loop and never stop, until it breaks */
            {
                await Task.Delay(1000);

                /* randomises the int value in a range of -3, 3 */
                randomisedCPSL = rndSeed.Next((int)sldLeftCPS.Value - 3, 
                                              (int)sldLeftCPS.Value + 3);

                randomisedCPSR = rndSeed.Next((int)sldRightCPS.Value - 3, 
                                              (int)sldRightCPS.Value + 3);
            }
        }

        #endregion



        #region Left clicker

        private async void DoLeftClick()
        {
            for (;;) /* will loop and never stop, until it breaks */
            {
                await Task.Delay(1000 / randomisedCPSL); /* gets the delay interval for cps, 1000 / 10 (cps) for example will get 100 delay, 100 x 10 = 1000 (ms) ; 1000ms = 1 second. (basically average cps per second) */

                MCHelper.GetMinecraftWindow(); /* gets minecraft process */

                if ((cbMenus.Checked && !ClickerExtensionHandle.InMenu()) || !cbMenus.Checked) /* checks if "Disable in menu" checkbox is checked AND if you arent in menu ; OR if "Disable in menu" checkbox is unchecked without checking if u are in menus (that'll skip the checking if in menu part) */
                    LeftConds(); /* gets in the conditions */
            }
        }

        private void LeftConds()
        {
            if (IsWhitelistedLeft() && tgLeft.Checked) /* checks if current slot is whitelisted AND if left clicker is toggled */
            {
                if (cbShiftLeft.Checked && WinApi.GetAsyncKeyState(Keys.LShiftKey) != 0) return; /* if shift disabled is checked and if its holding shift, return */

                if ((!cbRMB.Checked && WinApi.GetAsyncKeyState(WinApi.VK_LBUTTON) < 0) || (cbRMB.Checked && MouseButtons == MouseButtons.Left)) /* if rmb lock checkbox isnt checked AND if virtual key is pressed ; OR checks if rmb lock checkbox is checked AND if key is pressed */
                    if (!cbBBlocks.Checked)                             /* if break blocks isnt checked */
                        LeftClicker.SendMessageLeftClick();             /* send a normal click */
                    else
                        LeftClicker.SendMessageLeftClickBreakBlocks();  /* else break blocks */
            }
        }

        #endregion



        #region Right clicker

        private async void DoRightClick()
        {
            for (;; ) /* will loop and never stops, until it breaks */
            {
                /* same exact procedure as left clicker but for the right clicker :) */

                await Task.Delay(1000 / randomisedCPSR);

                MCHelper.GetMinecraftWindow();

                RightConds();
            }
        }

        private void RightConds()
        {
            if (IsWhitelistedRight() && tgRight.Checked) /* checks if current slot is whitelisted AND if right clicker is toggled */
            {
                if (cbShiftRight.Checked && WinApi.GetAsyncKeyState(Keys.LShiftKey) != 0) return;  /* if shift disabled is checked and if its holding shift, return */

                if (WinApi.GetAsyncKeyState(WinApi.VK_RBUTTON) < 0)     /* checks if virtual key is pressed (right mouse button) */
                    RightClicker.SendMessageRightClick();               /* send a click */
            }
        }

        #endregion




        #region Destruct

        private void pbDestruct_Click(object sender, EventArgs e)
        {
            /* super basic destruct */

            foreach (Control currentControl in Controls) /* for each Control that we get in Controls */
            {
                currentControl.Dispose(); /* dispose all Control that are in Conrols */
            }

            Task.Delay(1000).Wait();

            Dispose();
            Environment.Exit(0); /* exit */
        }

        #endregion
    }
}
