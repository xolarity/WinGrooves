using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using mshtml;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Taskbar;
using WinGrooves.Utils;
using WinGrooves.Properties;

namespace WinGrooves
{
    /// <summary>
    /// Summary description for FrmMain.
    /// </summary>
    public class FrmMain : System.Windows.Forms.Form
    {
        private WebBrowser webBrowser1;
        private System.ComponentModel.IContainer components;
        private bool injectedSongInfoFunctions = false;
        private bool closingFromTray = false;
        const int WM_HOTKEY = 0x0312;
        const int VK_MEDIA_NEXT_TRACK = 0xB0;
        const int VK_MEDIA_PREV_TRACK = 0xB1;
        const int VK_MEDIA_STOP = 0xB2;
        const int VK_MEDIA_PLAY_PAUSE = 0xB3;

        public static KeyboardHook hook = new KeyboardHook();
        bool windowInitialized;

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem HideShow;
        private ToolStripMenuItem About;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Exit;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem Previous;
        private ToolStripMenuItem Next;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private Timer currentSongTimer;
        private ToolStripButton toolStripButton4;
        private Timer alwaysListeningTimer;
        private ToolStripMenuItem Play;

        //These constants are needed to disable the IE click sound
        private const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        private const int SET_FEATURE_ON_THREAD = 0x00000001;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;
        private const int SET_FEATURE_IN_REGISTRY = 0x00000004;
        private const int SET_FEATURE_ON_THREAD_LOCALMACHINE = 0x00000008;
        private const int SET_FEATURE_ON_THREAD_INTRANET = 0x00000010;
        private const int SET_FEATURE_ON_THREAD_TRUSTED = 0x00000020;
        private const int SET_FEATURE_ON_THREAD_INTERNET = 0x00000040;
        private ToolStripMenuItem Like;
        private ToolStripMenuItem Dislike;
        private ToolStripMenuItem shuffleToolStripMenuItem;
        private ToolStripMenuItem loopSongsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem muteToolStripMenuItem;
        private const int SET_FEATURE_ON_THREAD_RESTRICTED = 0x00000080;
        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(
        int FeatureEntry,
        [MarshalAs(UnmanagedType.U4)] int dwFlags,
        bool fEnable);

        //Windows 7 features
        private ThumbnailToolbarButton buttonPrev;
        private bool isbuttonPaused, isMusicPlaying;
        private ThumbnailToolbarButton buttonPause;
        private ThumbnailToolbarButton buttonNext;
        private TabbedThumbnail _customThumbnail; //Taskbar image icon

        private string _cachedSongTitle; //Used to tell if song has changed

        public FrmMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.Resize += new EventHandler(FrmMain_Resize);

            //code to remember window size and position
            // this is the default
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            // check if the saved bounds are nonzero and visible on any screen
            if (Properties.Settings.Default.WindowPosition != Rectangle.Empty &&
                IsVisibleOnAnyScreen(Properties.Settings.Default.WindowPosition))
            {
                // first set the bounds
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = Properties.Settings.Default.WindowPosition;

                // afterwards set the window state to the saved value (which could be Maximized)
                this.WindowState = Properties.Settings.Default.WindowState;
            }
            else
            {
                // this resets the upper left corner of the window to windows standards
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;

                // we can still apply the saved size
                // msorens: added gatekeeper, otherwise first time appears as just a title bar!
                if (Properties.Settings.Default.WindowPosition != Rectangle.Empty)
                {
                    this.Size = Properties.Settings.Default.WindowPosition.Size;
                }
            }

            //
            // Win 7 toolbar buttons
            //
            //Moved here from initializeComponent method because it is recreated after adding any controls to the form
            buttonPrev = new ThumbnailToolbarButton(Properties.Resources.PlayerPrev, "Previous Music");
            buttonPause = new ThumbnailToolbarButton(Properties.Resources.PlayerPlay, "Pause/Play Music");
            isbuttonPaused = false;
            isMusicPlaying = false;
            buttonNext = new ThumbnailToolbarButton(Properties.Resources.PlayerNext, "Next Music");

            windowInitialized = true;
        }

        private bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // only save the WindowState if Normal or Maximized
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    Properties.Settings.Default.WindowState = this.WindowState;
                    break;

                default:
                    Properties.Settings.Default.WindowState = FormWindowState.Normal;
                    break;
            }

            # region msorens: this code does *not* handle minimized/maximized window.

            // reset window state to normal to get the correct bounds
            // also make the form invisible to prevent distracting the user
            //this.Visible = false;
            //this.WindowState = FormWindowState.Normal;
            //Settings.Default.WindowPosition = this.DesktopBounds;

            # endregion

            Properties.Settings.Default.Save();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TrackWindowState();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            TrackWindowState();
        }

        // On a move or resize in Normal state, record the new values as they occur.
        // This solves the problem of closing the app when minimized or maximized.
        private void TrackWindowState()
        {
            // Don't record the window setup, otherwise we lose the persistent values!
            if (!windowInitialized) { return; }

            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowPosition = this.DesktopBounds;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.HideShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.About = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Previous = new System.Windows.Forms.ToolStripMenuItem();
            this.Next = new System.Windows.Forms.ToolStripMenuItem();
            this.Play = new System.Windows.Forms.ToolStripMenuItem();
            this.Like = new System.Windows.Forms.ToolStripMenuItem();
            this.Dislike = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.loopSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shuffleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.currentSongTimer = new System.Windows.Forms.Timer(this.components);
            this.alwaysListeningTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 25);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(1517, 690);
            this.webBrowser1.TabIndex = 9;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "WinGrooves";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HideShow,
            this.toolStripMenuItem1,
            this.About,
            this.toolStripSeparator2,
            this.Previous,
            this.Next,
            this.Play,
            this.Like,
            this.Dislike,
            this.toolStripMenuItem2,
            this.loopSongsToolStripMenuItem,
            this.shuffleToolStripMenuItem,
            this.toolStripMenuItem3,
            this.muteToolStripMenuItem,
            this.toolStripSeparator1,
            this.Exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 292);
            // 
            // HideShow
            // 
            this.HideShow.Name = "HideShow";
            this.HideShow.Size = new System.Drawing.Size(181, 22);
            this.HideShow.Text = "Show/Hide";
            this.HideShow.Click += new System.EventHandler(this.HideShow_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem1.Text = "Options";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // About
            // 
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(181, 22);
            this.About.Text = "About";
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // Previous
            // 
            this.Previous.Name = "Previous";
            this.Previous.Size = new System.Drawing.Size(181, 22);
            this.Previous.Text = "Previous Song";
            this.Previous.Click += new System.EventHandler(this.Previous_Click);
            // 
            // Next
            // 
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(181, 22);
            this.Next.Text = "Next Song";
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // Play
            // 
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(181, 22);
            this.Play.Text = "Play/Pause";
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // Like
            // 
            this.Like.Name = "Like";
            this.Like.Size = new System.Drawing.Size(181, 22);
            this.Like.Text = "Like Current Song";
            this.Like.Click += new System.EventHandler(this.Like_Click);
            // 
            // Dislike
            // 
            this.Dislike.Name = "Dislike";
            this.Dislike.Size = new System.Drawing.Size(181, 22);
            this.Dislike.Text = "Dislike Current Song";
            this.Dislike.Click += new System.EventHandler(this.Dislike_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(178, 6);
            // 
            // loopSongsToolStripMenuItem
            // 
            this.loopSongsToolStripMenuItem.Name = "loopSongsToolStripMenuItem";
            this.loopSongsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loopSongsToolStripMenuItem.Text = "Loop Songs";
            this.loopSongsToolStripMenuItem.Click += new System.EventHandler(this.loopSongsToolStripMenuItem_Click);
            // 
            // shuffleToolStripMenuItem
            // 
            this.shuffleToolStripMenuItem.Name = "shuffleToolStripMenuItem";
            this.shuffleToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.shuffleToolStripMenuItem.Text = "Shuffle";
            this.shuffleToolStripMenuItem.Click += new System.EventHandler(this.shuffleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(178, 6);
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.muteToolStripMenuItem.Text = "Mute";
            this.muteToolStripMenuItem.Click += new System.EventHandler(this.muteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(181, 22);
            this.Exit.Text = "Exit";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1517, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Go back one page";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Go forward one page";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Options";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "About";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // currentSongTimer
            // 
            this.currentSongTimer.Interval = 3000;
            this.currentSongTimer.Tick += new System.EventHandler(this.currentSongTimer_Tick);
            // 
            // alwaysListeningTimer
            // 
            this.alwaysListeningTimer.Enabled = true;
            this.alwaysListeningTimer.Interval = 600000;
            this.alwaysListeningTimer.Tick += new System.EventHandler(this.alwaysListeningTimer_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1517, 715);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinGrooves";
            this.Activated += new System.EventHandler(this.FrmMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Check if application is aready running before running it again
            bool ok;
            object m = new System.Threading.Mutex(true, "WinGrooves", out ok);
            if (!ok)
            {
                MessageBox.Show("WinGrooves is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Application.Run(new FrmMain());
            GC.KeepAlive(m);
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            webBrowser1.Navigate("http://listen.grooveshark.com");
            // register the event that is fired after a key press.
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            webBrowser1.ObjectForScripting = this; //needed to capture JavaScript events

            //disable the IE "click sound"
            int feature = FEATURE_DISABLE_NAVIGATION_SOUNDS;
            CoInternetSetFeatureEnabled(feature, SET_FEATURE_ON_PROCESS, true);

            if (Properties.Settings.Default.startMinimized)
            {
                showHideWindow();
            }
            //Thumbnail buttons for win7 users
            if (TaskbarManager.IsPlatformSupported)
            {

                //Add a thumbnail image during peak
                _customThumbnail = new TabbedThumbnail(this.Handle, this.Handle);
                TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(_customThumbnail);


                buttonPrev.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(Previous_Click);
                buttonPause.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(Play_Click);
                buttonNext.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(Next_Click);

                //Add the buttons (kinda of ugly tough)
                ThumbnailToolbarButton[] buttonList = new ThumbnailToolbarButton[3] { buttonPrev, buttonPause, buttonNext };
                TaskbarManager.Instance.ThumbnailToolbars.AddButtons(this.Handle, buttonList);

            }
        }

        /*
         * Simulates a browser click on an html element
         /// <param name="action">the HTML id of the element to click on/param>
         * */
        private void playerExecute(string action)
        {
            webBrowser1.Document.GetElementById(action).InvokeMember("click");
        }

        /*
         * Injects some jQuery to select and click on specific elements
         /// <param name="action">the jquery selector for the element to click on ("#elementid .childclass")</param>
         * */
        private void htmlClickOn(string selector)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                Object[] objArray = new Object[1];
                objArray[0] = (Object)selector;
                webBrowser1.Document.InvokeScript("clickElement", objArray);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.trayMinimize)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    Hide();
                }
            }
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            SetupGlobalHotkeys();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            closingFromTray = true;
            Close();
        }

        private void HideShow_Click(object sender, EventArgs e)
        {
            showHideWindow();
        }

        private void showHideWindow()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                if (Properties.Settings.Default.trayMinimize)
                {
                    Hide();
                }
            }
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if (TaskbarManager.IsPlatformSupported)
            {
                //this only shortens the delay of the button change. Need to have a disabled state!
                if (!isbuttonPaused)
                {
                    buttonPause.Icon = Properties.Resources.PlayerPause;
                    isbuttonPaused = true;
                    playerExecute("play-pause");
                    isMusicPlaying = true;
                    return;
                }
                if (isbuttonPaused)
                {
                    buttonPause.Icon = Properties.Resources.PlayerPlay;
                    isbuttonPaused = false;
                    playerExecute("play-pause");
                    isMusicPlaying = false;
                    return;
                }
            }
            else
                playerExecute("play-pause");
        }

        private void Next_Click(object sender, EventArgs e)
        {
            playerExecute("play-next");
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            playerExecute("play-prev");
        }

        private void SetupGlobalHotkeys()
        {
            hook.unregisterAllHotkeys(); //first unregister everything

            // register the media keys
            try { hook.RegisterHotKey(global::ModifierKeys.None, (Keys)VK_MEDIA_PLAY_PAUSE); }
            catch (InvalidOperationException exception) { } //MessageBox.Show(exception.Message);
            try { hook.RegisterHotKey(global::ModifierKeys.None, (Keys)VK_MEDIA_NEXT_TRACK); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(global::ModifierKeys.None, (Keys)VK_MEDIA_PREV_TRACK); }
            catch (InvalidOperationException exception) { }

            //register other customizable hot keys
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyPlay), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyPlay)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyNext), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyNext)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyPrevious), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyPrevious)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyLike), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyLike)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyDislike), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyDislike)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyFavorite), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyFavorite)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyShowHide), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyShowHide)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyMute), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyMute)); }
            catch (InvalidOperationException exception) { }
            try { hook.RegisterHotKey(hook.Win32ModifiersFromKeys((Keys)Properties.Settings.Default.hotkeyShuffle), hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyShuffle)); }
            catch (InvalidOperationException exception) { }
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            switch (e.Key.ToString())
            {
                case "MediaPlayPause":
                    playerExecute("play-pause");
                    break;
                case "MediaNextTrack":
                    playerExecute("play-next");
                    break;
                case "MediaPreviousTrack":
                    playerExecute("play-prev");
                    break;
            }

            uint KeyAsInt = (uint)(e.Key | hook.keyToModifierKey(e.Modifier));
            if (KeyAsInt == Properties.Settings.Default.hotkeyPlay)
            {
                htmlClickOn("#play-pause");
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyNext)
            {
                htmlClickOn("#play-next");
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyPrevious)
            {
                htmlClickOn("#play-prev");
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyLike)
            {
                LikeCurrentSong();
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyDislike)
            {
                DislikeCurrentSong();
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyFavorite)
            {
                htmlClickOn("#np-fav");
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyMute)
            {
                htmlClickOn("#volume");
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyShowHide)
            {
                showHideWindow();
            }
            else if (KeyAsInt == Properties.Settings.Default.hotkeyShuffle)
            {
                htmlClickOn("#shuffle");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OptionsForm optionsDialog = new OptionsForm();
            optionsDialog.ShowDialog(this);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OptionsForm optionsDialog = new OptionsForm();
            optionsDialog.ShowDialog(this);
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutDialog = new AboutBox1();
            aboutDialog.ShowDialog(this);
        }

        private void currentSongTimer_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                try
                {
                    if (!injectedSongInfoFunctions)
                    {
                        HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
                        HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
                        IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
                        string injectedJquery = "function getSongTitle() {  return $(\"#now-playing-metadata .song\").text(); } " +
                            "function getSongArtist() {  return $(\"#now-playing-metadata .artist\").text(); }" +
                            "function mouseMove() {  $(\"#page-wrapper\").mousemove(); }" +
                            "function clickElement(selector) {  $(selector).click(); }" +
                            "function getMusicState() {return $(\"#play-pause\").hasClass(\"play\"); }" +
                            "function getShuffleActive() { return $(\"#shuffle\").hasClass(\"active\"); }" +
                            "function getRepeatActive() { return $(\"#repeat\").hasClass(\"active\") || $(\"#repeat\").hasClass(\"one\"); }" +
                            "function getMuteActive() { return $(\"#volume\").hasClass(\"mute\"); }"
                        ;
                        element.text = injectedJquery;
                        head.AppendChild(scriptEl);

                        injectedSongInfoFunctions = true;
                    }

                    object songTitle = webBrowser1.Document.InvokeScript("getSongTitle");
                    //Since the song title changed we must update all the information
                    if (String.IsNullOrEmpty(_cachedSongTitle) || _cachedSongTitle != songTitle.ToString())
                    {

                        //cache new song title so we can tell if it changes again
                        _cachedSongTitle = songTitle.ToString();

                        //Update thumbnail with album art cover
                        UpdateThumbnail();

                        //Set the Windows title
                        object songArtist = webBrowser1.Document.InvokeScript("getSongArtist");

                        if (songTitle.ToString().Length > 0)
                        {
                            this.Text = songTitle + " - " + songArtist + " - " + Application.ProductName;
                            //set the tray icon text if it is less than 63 characters (the max allowed)
                            if ((songTitle.ToString().Length + songArtist.ToString().Length + 3) < 63)
                            {
                                notifyIcon1.Text = songTitle + " - " + songArtist;
                            }
                            else
                            {
                                try // Get what you can up to max length.
                                {
                                    notifyIcon1.Text = (songTitle + " - " + songArtist).Substring(0, 62);
                                }
                                catch
                                {
                                    notifyIcon1.Text = Application.ProductName;
                                }
                            }
                        }

                        //control thumbail icons
                        if (TaskbarManager.IsPlatformSupported)
                        {
                            //the element class of the play button on grooveshark changes according to the music state (contains play/paused/nothing)
                            //I can't figure a better way to control the thumbnail states.
                            if (isMusicPlaying)
                                if (!Convert.ToBoolean(webBrowser1.Document.InvokeScript("getMusicState")))
                                {
                                    buttonPause.Icon = Properties.Resources.PlayerPause;
                                    isbuttonPaused = true;
                                }
                                else
                                {
                                    buttonPause.Icon = Properties.Resources.PlayerPlay;
                                    isbuttonPaused = false;
                                }
                        }
                        if (Settings.Default.showBalloonNotification && !Convert.ToBoolean(webBrowser1.Document.InvokeScript("getMusicState")) && notifyIcon1.Text != Application.ProductName)
                        {
                            notifyIcon1.ShowBalloonTip(Convert.ToInt32(Settings.Default.numBalloonDelay) * 1000, Application.ProductName,
                                String.Format("Now playing \"{0}\"", notifyIcon1.Text), ToolTipIcon.Info);
                        }
                        shuffleToolStripMenuItem.Checked = Convert.ToBoolean(webBrowser1.Document.InvokeScript("getShuffleActive"));
                        loopSongsToolStripMenuItem.Checked = Convert.ToBoolean(webBrowser1.Document.InvokeScript("getRepeatActive"));
                        muteToolStripMenuItem.Checked = Convert.ToBoolean(webBrowser1.Document.InvokeScript("getMuteActive"));
                    }
                }
                catch (NullReferenceException)
                {
                    //this should avoid weird errors
                }
            }
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            OptionsForm optionsDialog = new OptionsForm();
            optionsDialog.ShowDialog(this);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutDialog = new AboutBox1();
            aboutDialog.ShowDialog(this);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            currentSongTimer.Enabled = true;
            mshtml.HTMLDocumentEvents2_Event htmlEvent;
            htmlEvent = (mshtml.HTMLDocumentEvents2_Event)(mshtml.HTMLDocument)webBrowser1.Document.DomDocument;
            htmlEvent.onclick += new HTMLDocumentEvents2_onclickEventHandler(htmlEvent_onclick);
        }

        bool htmlEvent_onclick(IHTMLEventObj pEvtObj)
        {
            if (pEvtObj.srcElement.className == "icon play icon-play-white-outline")
            {
                isMusicPlaying = true;
            }
            return true;
        }

        private void alwaysListeningTimer_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                //this will simulate moving the mouse so that the player doesn't stop playing music after a few minutes of not interacting with the page
                webBrowser1.Document.InvokeScript("mouseMove");
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closingFromTray && Properties.Settings.Default.trayClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Like_Click(object sender, EventArgs e)
        {
            LikeCurrentSong();
        }

        private void LikeCurrentSong()
        {
            htmlClickOn("#player-wrapper .queue-item-active .smile");
        }

        private void DislikeCurrentSong()
        {
            htmlClickOn("#player-wrapper .queue-item-active .frown");
        }

        private void Dislike_Click(object sender, EventArgs e)
        {
            DislikeCurrentSong();
        }

        #region Thumbnail Methods

        /// <summary>
        /// Updates the thumbnail on the Taskbar in Windows 7
        /// </summary>
        private void UpdateThumbnail()
        {

            var url = webBrowser1.Document.GetElementById("now-playing-image").GetAttribute("src");

            if (String.IsNullOrEmpty(url) || url.EndsWith("40_album.png"))
            {
                //there is no album art for this song so lets just redraw the application.
                _customThumbnail.SetImage(ImageUtils.ApplicationThumbnail(this));
            }
            else
            {
                //Calculate Album Art URL
                string urlBase = url.Substring(0, url.LastIndexOf("/") + 1);
                string fileName = url.Substring(url.LastIndexOf("/") + 1);

                if (fileName.StartsWith("40_"))
                {
                    //Build the url for the 120 pixel album art
                    fileName = String.Format("120{0}", fileName.Substring(fileName.IndexOf("_")));
                    url = urlBase + fileName;
                }
                Bitmap albumPreview = ImageUtils.BitmapFromUrl(url);
                _customThumbnail.SetImage(albumPreview ?? ImageUtils.ApplicationThumbnail(this));
            }
        }
        #endregion

        private void shuffleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            htmlClickOn("#shuffle");
            shuffleToolStripMenuItem.Checked = !shuffleToolStripMenuItem.Checked;
        }

        private void loopSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            htmlClickOn("#repeat");
            loopSongsToolStripMenuItem.Checked = !loopSongsToolStripMenuItem.Checked;
        }

        private void muteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            htmlClickOn("#volume");
            muteToolStripMenuItem.Checked = !muteToolStripMenuItem.Checked;
        }

    }
}