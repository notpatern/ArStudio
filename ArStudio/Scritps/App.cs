using System;
using System.Collections.Generic;
using CefSharp;
using StereoKit;
using StereoKit.Framework;
using TestWebAr.Scritps.Objects;
using TestWebAr.Scritps.Services;

public class App
{
    ButtonWindow buttonWindow;

    HandTracking handTracking = new HandTracking();

    List<Browser> browserList = new List<Browser>();

    Material floorMaterial;

    Browser selectedBrowser;

    Browser dirtyBrowser = null;

    VolumeSlider volumeSlider;

    public void Init()
    {
        SKSettings settings = new SKSettings
        {
            appName = "TestWebAr",
            assetsFolder = "Assets",
            blendPreference = DisplayBlend.AnyTransparent,
            mode = AppMode.XR
        };

        var passthroughStepper = SK.AddStepper(new PassthroughFBExt());

        if (!SK.Initialize(settings))
            Environment.Exit(1);

        passthroughStepper.EnabledPassthrough = true;

        volumeSlider = new VolumeSlider("Volume", new Pose(0, 0, -0.3f, Quat.LookDir(0, 0, 1)));

        for (int i = 0; i < 1; i++)
        {
            browserList.Add(
                new Browser(
                    //"https://javascript.info/keyboard-events",
                    "http://192.168.101.52:23800/login",

                    i.ToString(),
                    new Pose(0.75f * i, 0, -0.5f, Quat.LookDir(0, 0, 1))
                )
            );
            while (browserList[i].browser == null) { }
            while (!browserList[i].browser.IsBrowserInitialized) { }

            browserList[i].BindBrowserSelect((browser) =>
            {
                selectedBrowser = browser;
                browser.selected = true;
                if (dirtyBrowser != null && dirtyBrowser != selectedBrowser)
                {
                    dirtyBrowser.Mute();
                    dirtyBrowser.selected = false;
                }
                dirtyBrowser = browser;
            });
            volumeSlider.BindVolumeAction(browserList[i].SetVolume);
        }

        floorMaterial = new Material("floor.hlsl");
        floorMaterial.Transparency = Transparency.Blend;

        buttonWindow = new ButtonWindow("buttons", new Pose(0.4f, 0, -0.3f, Quat.LookDir(0, 0, 1)));

        handTracking.Pause += PauseVideo;
        handTracking.Play += PlayVideo;
        handTracking.NewLog += NewLog;
        handTracking.CloseLog += CloseLog;
        handTracking.CancelLog += CancelLog;
        handTracking.CancelOpenLog += CancelOpenLog;
        handTracking.CopyPlayerTimeCode += CopyPlayerTimeCode;
        handTracking.Tab += Tab;
        handTracking.BackToLive += BackToLive;
        handTracking.RightFastHand += LogInRadioEdit;
    }

    private void UpdateBrowsers()
    {
        foreach (Browser browser in browserList)
        {
            browser.UpdateBrowser();
        }
    }

    private void LogInRadioEdit()
    {
        ForwardKeyToCef(VirtualKeyCode.VK_T);
        ForwardKeyToCef(VirtualKeyCode.VK_E);
        ForwardKeyToCef(VirtualKeyCode.VK_S);
        ForwardKeyToCef(VirtualKeyCode.VK_T);
        ForwardKeyToCef(VirtualKeyCode.TAB);
    }
    
    private void CopyPlayerTimeCode()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Space keydown event
        var spaceDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x20,                             // VK_SPACE (Space key)
            NativeKeyCode = 0x39,                              // Scan code for Space
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Space
            Modifiers = CefEventFlags.None                     // No modifiers
        };
        browserHost.SendKeyEvent(spaceDownEvent);

        // Send the Space keyup event
        var spaceUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x20,                             // VK_SPACE (Space key)
            NativeKeyCode = 0x39,                              // Scan code for Space
            Type = KeyEventType.KeyUp,                         // KeyUp event for Space
            Modifiers = CefEventFlags.None                     // No modifiers
        };
        browserHost.SendKeyEvent(spaceUpEvent);
    }

    private void BackToLive() {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl (Left) keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
                           NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
                           Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is down
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the L keydown event
        var lDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x4C,                             // VK_L (L key)
                           NativeKeyCode = 0x26,                              // Scan code for L
                           Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for L
                           Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(lDownEvent);

        // Send the L keyup event
        var lUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x4C,                             // VK_L (L key)
                           NativeKeyCode = 0x26,                              // Scan code for L
                           Type = KeyEventType.KeyUp,                         // KeyUp event for L
                           Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(lUpEvent);

        // Send the Ctrl (Left) keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
                           NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
                           Type = KeyEventType.KeyUp,                         // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                     // No modifiers as Ctrl is being released
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private void Tab()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Tab keydown event
        var tabDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x09,                             // VK_TAB (Tab key)
            NativeKeyCode = 0x0F,                              // Scan code for Tab
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Tab
            Modifiers = CefEventFlags.None                     // No modifiers
        };
        browserHost.SendKeyEvent(tabDownEvent);

        // Send the Tab keyup event
        var tabUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x09,                             // VK_TAB (Tab key)
            NativeKeyCode = 0x0F,                              // Scan code for Tab
            Type = KeyEventType.KeyUp,                         // KeyUp event for Tab
            Modifiers = CefEventFlags.None                     // No modifiers
        };
        browserHost.SendKeyEvent(tabUpEvent);
    }

    private void CancelOpenLog()
    {
        if (selectedBrowser == null)
        {
            return;
        }

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        var deleteDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x2E,                             // VK_DELETE (Delete key)
            NativeKeyCode = 0x53,                              // Scan code for Delete
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Delete
        };
        browserHost.SendKeyEvent(deleteDownEvent);

        // Send the Delete keyup event
        var deleteUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x2E,                             // VK_DELETE (Delete key)
            NativeKeyCode = 0x53,                              // Scan code for Delete
            Type = KeyEventType.KeyUp,                         // KeyUp event for Delete
        };
        browserHost.SendKeyEvent(deleteUpEvent);
    }

    private void CancelLog()
    {
        if (selectedBrowser == null)
        {
            return;
        }

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl (Left) keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
            NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Ctrl
            Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is down
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Delete keydown event
        var deleteDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x2E,                             // VK_DELETE (Delete key)
            NativeKeyCode = 0x53,                              // Scan code for Delete
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Delete
            Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(deleteDownEvent);

        // Send the Delete keyup event
        var deleteUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x2E,                             // VK_DELETE (Delete key)
            NativeKeyCode = 0x53,                              // Scan code for Delete
            Type = KeyEventType.KeyUp,                         // KeyUp event for Delete
            Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(deleteUpEvent);

        // Send the Ctrl (Left) keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
            NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
            Type = KeyEventType.KeyUp,                         // KeyUp event for Ctrl
            Modifiers = CefEventFlags.None                     // No modifiers as Ctrl is being released
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private void CloseLog()
    {
        if (selectedBrowser == null)
        {
            return;
        }

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,
            NativeKeyCode = 0x1D,
            Type = KeyEventType.RawKeyDown,
            Modifiers = CefEventFlags.ControlDown
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        var enterDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x0D,
            NativeKeyCode = 0x1C,
            Type = KeyEventType.RawKeyDown,
            Modifiers = CefEventFlags.ControlDown
        };
        browserHost.SendKeyEvent(enterDownEvent);

        var enterUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x0D,
            NativeKeyCode = 0x1C,
            Type = KeyEventType.KeyUp,
            Modifiers = CefEventFlags.ControlDown
        };
        browserHost.SendKeyEvent(enterUpEvent);

        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,
            NativeKeyCode = 0x1D,
            Type = KeyEventType.KeyUp,
            Modifiers = CefEventFlags.None
        };
        browserHost.SendKeyEvent(ctrlUpEvent);

    }

    private void NewLog()
    {
        ForwardKeyToCef(VirtualKeyCode.VK_A, ctrl: true);
       //if (selectedBrowser == null)
       // {
       //     return;
       // }

       // var browserHost = selectedBrowser.browser.GetBrowserHost();

       // // Send the Ctrl (Left) keydown event
       // var ctrlDownEvent = new KeyEvent
       // {
       //     WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
       //     NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
       //     Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Ctrl
       //     Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is down
       // };
       // browserHost.SendKeyEvent(ctrlDownEvent);

       // // Send the A keydown event
       // var aDownEvent = new KeyEvent
       // {
       //     WindowsKeyCode = 0x41,                             // VK_A
       //     NativeKeyCode = 0x1E,                              // Scan code for A
       //     Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for A
       //     Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
       // };
       // browserHost.SendKeyEvent(aDownEvent);

       // // Send the A keyup event
       // var aUpEvent = new KeyEvent
       // {
       //     WindowsKeyCode = 0x41,                             // VK_A
       //     NativeKeyCode = 0x1E,                              // Scan code for A
       //     Type = KeyEventType.KeyUp,                         // KeyUp event for A
       //     Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
       // };
       // browserHost.SendKeyEvent(aUpEvent);

       // // Send the Ctrl (Left) keyup event
       // var ctrlUpEvent = new KeyEvent
       // {
       //     WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
       //     NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
       //     Type = KeyEventType.KeyUp,                         // KeyUp event for Ctrl
       //     Modifiers = CefEventFlags.None                     // No modifiers as Ctrl is being released
       // };
       // browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private void PlayVideo()
    {
        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl (Left) keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
            NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Ctrl
            Modifiers = CefEventFlags.ControlDown              //rightHand.palm.Up Indicates the Ctrl key is down
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Space keydown event
        var spaceDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x20,                             // VK_SPACE (Space key)
            NativeKeyCode = 0x39,                              // Scan code for Space (common value)
            Type = KeyEventType.RawKeyDown,                    // RawKeyDown event for Space
            Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(spaceDownEvent);

        // Send the Space keyup event
        var spaceUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x20,                             // VK_SPACE (Space key)
            NativeKeyCode = 0x39,                              // Scan code for Space (common value)
            Type = KeyEventType.KeyUp,                         // KeyUp event for Space
            Modifiers = CefEventFlags.ControlDown              // Indicates the Ctrl key is still down
        };
        browserHost.SendKeyEvent(spaceUpEvent);

        // Send the Ctrl (Left) keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0xA2,                             // VK_LCONTROL (Left Control key)
            NativeKeyCode = 0x1D,                              // Scan code for Left Control (LCtrl)
            Type = KeyEventType.KeyUp,                         // KeyUp event for Ctrl
            Modifiers = CefEventFlags.None                     // No modifiers as Ctrl is being released
        };
        browserHost.SendKeyEvent(ctrlUpEvent);

    }

    void PauseVideo()
    {
        ForwardKeyToCef(VirtualKeyCode.VK_P, ctrl: true);
    }

    public void Update()
    {
        CaptureKeyboardInput();
        volumeSlider.UpdateSlider();
        UpdateBrowsers();
        buttonWindow.UpdateWindow();
    }

    // ------------------------------------------------------------------------------------------------------------

    private void CaptureKeyboardInput()
    {
        // Add more keys as needed
        CheckAndForwardKey(Key.A, VirtualKeyCode.VK_A);
        CheckAndForwardKey(Key.B, VirtualKeyCode.VK_B);
        CheckAndForwardKey(Key.C, VirtualKeyCode.VK_C);
        CheckAndForwardKey(Key.D, VirtualKeyCode.VK_D);
        CheckAndForwardKey(Key.E, VirtualKeyCode.VK_E);
        CheckAndForwardKey(Key.F, VirtualKeyCode.VK_F);
        CheckAndForwardKey(Key.G, VirtualKeyCode.VK_G);
        CheckAndForwardKey(Key.H, VirtualKeyCode.VK_H);
        CheckAndForwardKey(Key.I, VirtualKeyCode.VK_I);
        CheckAndForwardKey(Key.J, VirtualKeyCode.VK_J);
        CheckAndForwardKey(Key.K, VirtualKeyCode.VK_K);
        CheckAndForwardKey(Key.L, VirtualKeyCode.VK_L);
        CheckAndForwardKey(Key.M, VirtualKeyCode.VK_M);
        CheckAndForwardKey(Key.N, VirtualKeyCode.VK_N);
        CheckAndForwardKey(Key.O, VirtualKeyCode.VK_O);
        CheckAndForwardKey(Key.P, VirtualKeyCode.VK_P);
        CheckAndForwardKey(Key.Q, VirtualKeyCode.VK_Q);
        CheckAndForwardKey(Key.R, VirtualKeyCode.VK_R);
        CheckAndForwardKey(Key.S, VirtualKeyCode.VK_S);
        CheckAndForwardKey(Key.T, VirtualKeyCode.VK_T);
        CheckAndForwardKey(Key.U, VirtualKeyCode.VK_U);
        CheckAndForwardKey(Key.V, VirtualKeyCode.VK_V);
        CheckAndForwardKey(Key.W, VirtualKeyCode.VK_W);
        CheckAndForwardKey(Key.X, VirtualKeyCode.VK_X);
        CheckAndForwardKey(Key.Y, VirtualKeyCode.VK_Y);
        CheckAndForwardKey(Key.Z, VirtualKeyCode.VK_Z);

        CheckAndForwardKey(Key.N0, VirtualKeyCode.VK_0);
        CheckAndForwardKey(Key.N1, VirtualKeyCode.VK_1);
        CheckAndForwardKey(Key.N2, VirtualKeyCode.VK_2);
        CheckAndForwardKey(Key.N3, VirtualKeyCode.VK_3);
        CheckAndForwardKey(Key.N4, VirtualKeyCode.VK_4);
        CheckAndForwardKey(Key.N5, VirtualKeyCode.VK_5);
        CheckAndForwardKey(Key.N6, VirtualKeyCode.VK_6);
        CheckAndForwardKey(Key.N7, VirtualKeyCode.VK_7);
        CheckAndForwardKey(Key.N8, VirtualKeyCode.VK_8);
        CheckAndForwardKey(Key.N9, VirtualKeyCode.VK_9);

        CheckAndForwardKey(Key.Space, VirtualKeyCode.VK_SPACE);
        CheckAndForwardKey(Key.Return, VirtualKeyCode.RETURN);
        CheckAndForwardKey(Key.Backspace, VirtualKeyCode.BACK);
        CheckAndForwardKey(Key.Tab, VirtualKeyCode.TAB);
        CheckAndForwardKey(Key.Esc, VirtualKeyCode.ESCAPE);
        CheckAndForwardKey(Key.Left, VirtualKeyCode.LEFT);
        CheckAndForwardKey(Key.Right, VirtualKeyCode.RIGHT);
        CheckAndForwardKey(Key.Up, VirtualKeyCode.UP);
        CheckAndForwardKey(Key.Down, VirtualKeyCode.DOWN);

        // Add function keys
        CheckAndForwardKey(Key.F1, VirtualKeyCode.F1);
        CheckAndForwardKey(Key.F2, VirtualKeyCode.F2);
        CheckAndForwardKey(Key.F3, VirtualKeyCode.F3);
        CheckAndForwardKey(Key.F4, VirtualKeyCode.F4);
        CheckAndForwardKey(Key.F5, VirtualKeyCode.F5);
        CheckAndForwardKey(Key.F6, VirtualKeyCode.F6);
        CheckAndForwardKey(Key.F7, VirtualKeyCode.F7);
        CheckAndForwardKey(Key.F8, VirtualKeyCode.F8);
        CheckAndForwardKey(Key.F9, VirtualKeyCode.F9);
        CheckAndForwardKey(Key.F10, VirtualKeyCode.F10);
        CheckAndForwardKey(Key.F11, VirtualKeyCode.F11);
        CheckAndForwardKey(Key.F12, VirtualKeyCode.F12);
    }

    private void CheckAndForwardKey(Key skKey, VirtualKeyCode vkCode)
    {
        if (Input.Key(skKey).IsJustActive())
        {
            ForwardKeyToCef(vkCode);
        }
    }

    private void ForwardKeyToCef(VirtualKeyCode key, bool ctrl = false, bool lowerCase = false)
    {
        if (selectedBrowser != null)
        {
            int keyCode = (int)key;
            int charCode = keyCode;

            if (lowerCase)
            {
                if (keyCode >= (int)VirtualKeyCode.VK_A && keyCode <= (int)VirtualKeyCode.VK_Z)
                {
                    charCode = keyCode + 32;
                }
            }

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

            selectedBrowser.SendKey(
                selectedBrowser.browser.GetBrowser(),
                ctrl ? CefEventFlags.ControlDown : CefEventFlags.None,
                KeyEventType.KeyDown,
                keyCode,
                0
            );

            if (charCode >= 32 && charCode <= 126)
            {
                selectedBrowser.SendKey(
                    selectedBrowser.browser.GetBrowser(),
                    ctrl ? CefEventFlags.ControlDown : CefEventFlags.None,
                    KeyEventType.Char,
                    charCode,
                    0
                );
            }

            selectedBrowser.SendKey(
                selectedBrowser.browser.GetBrowser(),
                ctrl ? CefEventFlags.ControlDown : CefEventFlags.None,
                KeyEventType.KeyUp,
                keyCode,
                0
            );

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
