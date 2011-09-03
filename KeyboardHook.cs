using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Specialized;

public sealed class KeyboardHook : IDisposable
{
    // Registers a hot key with Windows.
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    // Unregisters the hot key with Windows.
    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    /// <summary>
    /// Represents the window that is used internally to get the messages.
    /// </summary>
    private class Window : NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0x0312;

        public Window()
        {
            // create the handle for the window.
            this.CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Overridden to get the notifications.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // check if we got a hot key pressed.
            if (m.Msg == WM_HOTKEY)
            {
                // get the keys.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                // invoke the event to notify the parent.
                if (KeyPressed != null)
                    KeyPressed(this, new KeyPressedEventArgs(modifier, key));
            }
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            this.DestroyHandle();
        }

        #endregion
    }

    private Window _window = new Window();
    public int _currentId;

    public KeyboardHook()
    {
        // register the event of the inner native window.
        _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
        {
            if (KeyPressed != null)
                KeyPressed(this, args);
        };
    }

    /// <summary>
    /// Registers a hot key in the system.
    /// </summary>
    /// <param name="modifier">The modifiers that are associated with the hot key.</param>
    /// <param name="key">The key itself that is associated with the hot key.</param>
    public void RegisterHotKey(ModifierKeys modifier, Keys key)
    {
		//MessageBox.Show("Modifier: " + modifier.ToString() + "Key: "+ key.ToString());
        // increment the counter.
        _currentId = _currentId + 1;

        // register the hot key.
        if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
			throw new InvalidOperationException("Unable to register the hotkey " + modifier.ToString() + " "+ key.ToString() + ". Please choose a different one (or find the key or program that has registered it already).");
    }

    /// <summary>
    /// A hot key has been pressed.
    /// </summary>
    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    #region IDisposable Members

    public void Dispose()
    {
        // unregister all the registered hot keys.
        for (int i = _currentId; i > 0; i--)
        {
            UnregisterHotKey(_window.Handle, i);
        }

        // dispose the inner native window.
        _window.Dispose();
    }

    #endregion

	public void unregisterAllHotkeys()
	{
		 // unregister all the registered hot keys.
        for (int i = _currentId; i > 0; i--)
        {
            UnregisterHotKey(_window.Handle, i);
        }
	}


    /// <summary>
    /// Calculates the Win32 Modifiers total for a Keys enum
    /// </summary>
    /// <param name="k">An instance of the Keys enumaration</param>
    /// <returns>The Win32 Modifiers total as required by RegisterHotKey</returns>
	public ModifierKeys Win32ModifiersFromKeys(Keys k)
    {
        byte total = 0;
		const byte ModAlt = 1, ModControl = 2, ModShift = 4, ModWin = 8;

        if (((int)k & (int)Keys.Shift) == (int)Keys.Shift)
            total += ModShift;
        if (((int)k & (int)Keys.Control) == (int)Keys.Control)
            total += ModControl;
        if (((int)k & (int)Keys.Alt) == (int)Keys.Alt)
            total += ModAlt;
        if (((int)k & (int)Keys.LWin) == (int)Keys.LWin)
            total += ModWin;

		return (ModifierKeys)total;
    }

	public Keys getModifierKey(Keys k)
	{
		Keys modKey = 0;

		if (((int)k & (int)Keys.Shift) == (int)Keys.Shift)
			modKey += (int)Keys.Shift;
		if (((int)k & (int)Keys.Control) == (int)Keys.Control)
			modKey += (int)Keys.Control;
		if (((int)k & (int)Keys.Alt) == (int)Keys.Alt)
			modKey += (int)Keys.Alt;
		if (((int)k & (int)Keys.LWin) == (int)Keys.LWin)
			modKey += (int)Keys.LWin;

		return modKey;
	}

	public Keys keyToModifierKey(ModifierKeys k)
	{
		Keys modKey = 0;

		if (((int)k & (int)ModifierKeys.Shift) == (int)ModifierKeys.Shift)
			modKey += (int)Keys.Shift;
		if (((int)k & (int)ModifierKeys.Control) == (int)ModifierKeys.Control)
			modKey += (int)Keys.Control;
		if (((int)k & (int)ModifierKeys.Alt) == (int)ModifierKeys.Alt)
			modKey += (int)Keys.Alt;
		if (((int)k & (int)ModifierKeys.Win) == (int)ModifierKeys.Win)
			modKey += (int)Keys.LWin;

		return modKey;
	}

	/// <summary>
	/// Returns a Key value for the key without a modifier (if the Keys for Ctrl + Shift + A is passed in, the Keys for A will be returned)
	/// </summary>
	/// <param name="k">An instance of the Keys enumaration</param>
	/// <returns>The character code of the key</returns>
	public Keys getKeyWithoutModifier(Keys k)
	{
		Keys keyWithout = k & Keys.KeyCode;

		return keyWithout;
	}
}

/// <summary>
/// Event Args for the event that is fired after the hot key has been pressed.
/// </summary>
public class KeyPressedEventArgs : EventArgs
{
    private ModifierKeys _modifier;
    private Keys _key;

    internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
    {
        _modifier = modifier;
        _key = key;
    }

    public ModifierKeys Modifier
    {
        get { return _modifier; }
    }

    public Keys Key
    {
        get { return _key; }
    }
}

/// <summary>
/// The enumeration of possible modifiers.
/// </summary>
[Flags]
public enum ModifierKeys : uint
{
    None = 0,
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}