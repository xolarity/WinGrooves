using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinGrooves
{
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//Try to register hotkeys
			FrmMain.hook.unregisterAllHotkeys(); //first unregister everything so there aren't any conflicts
			try
			{
                if (hotkeyControlPlay.Text != "None")       FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlPlay.HotkeyModifiers), (Keys)hotkeyControlPlay.Hotkey);
                if (hotkeyControlNext.Text != "None")       FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlNext.HotkeyModifiers), (Keys)hotkeyControlNext.Hotkey);
                if (hotkeyControlPrevious.Text != "None")   FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlPrevious.HotkeyModifiers), (Keys)hotkeyControlPrevious.Hotkey);
                if (hotkeyControlLike.Text != "None")       FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlLike.HotkeyModifiers), (Keys)hotkeyControlLike.Hotkey);
                if (hotkeyControlDislike.Text != "None")    FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlDislike.HotkeyModifiers), (Keys)hotkeyControlDislike.Hotkey);
                if (hotkeyControlFavorite.Text != "None")   FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlFavorite.HotkeyModifiers), (Keys)hotkeyControlFavorite.Hotkey);
                if (hotkeyControlShowHide.Text != "None")   FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlShowHide.HotkeyModifiers), (Keys)hotkeyControlShowHide.Hotkey);
                if (hotkeyControlMute.Text != "None")       FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlMute.HotkeyModifiers), (Keys)hotkeyControlMute.Hotkey);
                if (hotkeyControlShuffle.Text != "None")    FrmMain.hook.RegisterHotKey((ModifierKeys)FrmMain.hook.Win32ModifiersFromKeys(hotkeyControlShuffle.HotkeyModifiers), (Keys)hotkeyControlShuffle.Hotkey);

				//store the values in the application settings and save them
				Properties.Settings.Default.hotkeyPlay = (uint)(hotkeyControlPlay.Hotkey | hotkeyControlPlay.HotkeyModifiers);
				Properties.Settings.Default.hotkeyNext = (uint)(hotkeyControlNext.Hotkey | hotkeyControlNext.HotkeyModifiers);
				Properties.Settings.Default.hotkeyPrevious = (uint)(hotkeyControlPrevious.Hotkey | hotkeyControlPrevious.HotkeyModifiers);
				Properties.Settings.Default.hotkeyLike = (uint)(hotkeyControlLike.Hotkey | hotkeyControlLike.HotkeyModifiers);
				Properties.Settings.Default.hotkeyDislike = (uint)(hotkeyControlDislike.Hotkey | hotkeyControlDislike.HotkeyModifiers);
				Properties.Settings.Default.hotkeyFavorite = (uint)(hotkeyControlFavorite.Hotkey | hotkeyControlFavorite.HotkeyModifiers);
				Properties.Settings.Default.hotkeyShowHide = (uint)(hotkeyControlShowHide.Hotkey | hotkeyControlShowHide.HotkeyModifiers);
				Properties.Settings.Default.hotkeyMute = (uint)(hotkeyControlMute.Hotkey | hotkeyControlMute.HotkeyModifiers);
				Properties.Settings.Default.hotkeyShuffle = (uint)(hotkeyControlShuffle.Hotkey | hotkeyControlShuffle.HotkeyModifiers);
				Properties.Settings.Default.Save();

				this.Close();
			}
			catch (InvalidOperationException exception)
			{
				MessageBox.Show(exception.Message);
			}
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			hotkeyControlPlay.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyPlay);
			hotkeyControlPlay.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyPlay);

			hotkeyControlNext.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyNext);
			hotkeyControlNext.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyNext);

			hotkeyControlPrevious.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyPrevious);
			hotkeyControlPrevious.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyPrevious);

			hotkeyControlLike.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyLike);
			hotkeyControlLike.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyLike);

			hotkeyControlDislike.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyDislike);
			hotkeyControlDislike.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyDislike);

			hotkeyControlFavorite.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyFavorite);
			hotkeyControlFavorite.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyFavorite);

			hotkeyControlShowHide.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyShowHide);
			hotkeyControlShowHide.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyShowHide);

			hotkeyControlMute.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyMute);
			hotkeyControlMute.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyMute);

			hotkeyControlShuffle.Hotkey = FrmMain.hook.getKeyWithoutModifier((Keys)Properties.Settings.Default.hotkeyShuffle);
			hotkeyControlShuffle.HotkeyModifiers = FrmMain.hook.getModifierKey((Keys)Properties.Settings.Default.hotkeyShuffle);

		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
	}
}