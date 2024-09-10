using System;
using CefSharp;
using StereoKit;
using StereoKit.Framework;
using TestWebAr.Scritps.Objects;

public class App
{
    DefaultSkyLog skylog;
    delegate void UpdateHandler();
    event UpdateHandler UpdateEvent;

    Pose menuSelectPosition = new Pose(new Vec3(0, 0, -0.6f), Quat.LookDir(0, 0, 1));

    public void Init()
    {
        UpdateEvent = SelectMenuUpdate;

        SKSettings settings = new SKSettings
        {
            appName = "ArStudio",
            assetsFolder = "Assets",
            blendPreference = DisplayBlend.AnyTransparent,
            mode = AppMode.XR
        };

        var passthroughStepper = SK.AddStepper(new PassthroughFBExt());

        if (!SK.Initialize(settings))
            Environment.Exit(1);

        passthroughStepper.EnabledPassthrough = true;
    }

    private void SelectMenuUpdate()
    {
        UI.WindowBegin("Ar Studio", ref menuSelectPosition);
        if (UI.Button("Default"))
        {
            skylog = new DefaultSkyLog(volumeSlider: true, hotKeyPanel: true);
            UpdateEvent = skylog.Update;
        }

        if (UI.Button("Volume Demo"))
        {
            skylog = new VolumeSkyLog(volumeSlider: true);
            UpdateEvent = skylog.Update;
        }

        if (UI.Button("Obs Remote Control"))
        {
            skylog = new ObsSkyLog(volumeSlider: true);
            UpdateEvent = skylog.Update;
        }
        UI.WindowEnd();
    }

    public void Update()
    {
        UpdateEvent.Invoke();
    }
}

public class KeyForwarder
{

    public void CheckAndForwardKey(Browser selectedBrowser, Key skKey, VirtualKeyCode vkCode)
    {
        if (Input.Key(skKey).IsJustActive())
        {
            ForwardKeyToCef(selectedBrowser, vkCode);
        }
    }

    public void ForwardKeyToCef(Browser selectedBrowser, VirtualKeyCode key, bool ctrl = false, bool lowerCase = false, bool shift = false)
    {
        if (selectedBrowser != null)
        {
            int keyCode = (int)key; int charCode = keyCode;

            // Handle lowercase conversion
            if (lowerCase && keyCode >= (int)VirtualKeyCode.VK_A && keyCode <= (int)VirtualKeyCode.VK_Z)
            {
                charCode = keyCode + 32;
            }
            // Handle uppercase conversion if shift is held
            else if (shift && keyCode >= (int)VirtualKeyCode.VK_A && keyCode <= (int)VirtualKeyCode.VK_Z)
            {
                charCode = keyCode;
            }

            // Send Ctrl key down event if needed
            if (ctrl)
            {
                selectedBrowser.SendKey(
                        selectedBrowser.browser.GetBrowser(),
                        CefEventFlags.ControlDown,
                        KeyEventType.KeyDown,
                        (int)VirtualKeyCode.VK_CONTROL,  // CTRL key
                        0
                        );
            }

            // Send Shift key down event if needed
            if (shift)
            {
                selectedBrowser.SendKey(
                        selectedBrowser.browser.GetBrowser(),
                        CefEventFlags.ShiftDown,
                        KeyEventType.KeyDown,
                        (int)VirtualKeyCode.VK_SHIFT,  // Shift key
                        0
                        );
            }

            // Send the main key down event
            selectedBrowser.SendKey(
                    selectedBrowser.browser.GetBrowser(),
                    (ctrl ? CefEventFlags.ControlDown : CefEventFlags.None) |
                    (shift ? CefEventFlags.ShiftDown : CefEventFlags.None),
                    KeyEventType.KeyDown,
                    keyCode,
                    0
                    );

            // Send the character event if within printable ASCII range
            if (charCode >= 32 && charCode <= 126)
            {
                selectedBrowser.SendKey(
                        selectedBrowser.browser.GetBrowser(),
                        (ctrl ? CefEventFlags.ControlDown : CefEventFlags.None) |
                        (shift ? CefEventFlags.ShiftDown : CefEventFlags.None),
                        KeyEventType.Char,
                        charCode,
                        0
                        );
            }

            // Send the main key up event
            selectedBrowser.SendKey(
                    selectedBrowser.browser.GetBrowser(),
                    (ctrl ? CefEventFlags.ControlDown : CefEventFlags.None) |
                    (shift ? CefEventFlags.ShiftDown : CefEventFlags.None),
                    KeyEventType.KeyUp,
                    keyCode,
                    0
                    );

            // Send Shift key up event if it was held down
            if (shift)
            {
                selectedBrowser.SendKey(
                        selectedBrowser.browser.GetBrowser(),
                        CefEventFlags.ShiftDown,
                        KeyEventType.KeyUp,
                        (int)VirtualKeyCode.VK_SHIFT,
                        0
                        );
            }

            // Send Ctrl key up event if it was held down
            if (ctrl)
            {
                selectedBrowser.SendKey(
                        selectedBrowser.browser.GetBrowser(),
                        CefEventFlags.ControlDown,
                        KeyEventType.KeyUp,
                        (int)VirtualKeyCode.VK_CONTROL,
                        0
                        );
            }
        }
    }
}

public enum VirtualKeyCode
{
    VK_CONTROL = 0x11,
    VK_SHIFT = 0x10,
    VK_A = 0x41,
    VK_B = 0x42,
    VK_C = 0x43,
    VK_D = 0x44,
    VK_E = 0x45,
    VK_F = 0x46,
    VK_G = 0x47,
    VK_H = 0x48,
    VK_I = 0x49,
    VK_J = 0x4A,
    VK_K = 0x4B,
    VK_L = 0x4C,
    VK_M = 0x4D,
    VK_N = 0x4E,
    VK_O = 0x4F,
    VK_P = 0x50,
    VK_Q = 0x51,
    VK_R = 0x52,
    VK_S = 0x53,
    VK_T = 0x54,
    VK_U = 0x55,
    VK_V = 0x56,
    VK_W = 0x57,
    VK_X = 0x58,
    VK_Y = 0x59,
    VK_Z = 0x5A,
    VK_0 = 0x30,
    VK_1 = 0x31,
    VK_2 = 0x32,
    VK_3 = 0x33,
    VK_4 = 0x34,
    VK_5 = 0x35,
    VK_6 = 0x36,
    VK_7 = 0x37,
    VK_8 = 0x38,
    VK_9 = 0x39,
    VK_SPACE = 0x20,
    RETURN = 0x0D,
    BACK = 0x08,
    TAB = 0x09,
    ESCAPE = 0x1B,
    LEFT = 0x25,
    UP = 0x26,
    RIGHT = 0x27,
    DOWN = 0x28,
    F1 = 0x70,
    F2 = 0x71,
    F3 = 0x72,
    F4 = 0x73,
    F5 = 0x74,
    F6 = 0x75,
    F7 = 0x76,
    F8 = 0x77,
    F9 = 0x78,
    F10 = 0x79,
    F11 = 0x7A,
    F12 = 0x7B
}
