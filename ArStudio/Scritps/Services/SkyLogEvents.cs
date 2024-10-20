
using System;
using CefSharp;
using TestWebAr.Scritps.Objects;

public static class SkyLogEvents
{
    public static Browser selectedBrowser;

    public static KeyForwarder keyForwarder;

    public static Action NewLog;
    public static Action CloseLog;
    public static Action CancelLog;
    public static Action Play;
    public static Action Pause;
    public static Action CancelOpenLog;
    public static Action Tab;
    public static Action BackToLive;
    public static Action CopyPlayerTimeCode;
    public static Action ClearMarkers;
    public static Action CopyLog;
    public static Action PasteLog;
    public static Action ChangeLogTCIN;
    public static Action ChangeLogTCOUT;
    public static Action RightFrame;
    public static Action LeftFrame;
    public static Action Paste;

    public static Action F1;
    public static Action F2;
    public static Action F3;
    public static Action F4;
    public static Action F5;

    public static void BindEvents()
    {
        Pause += PauseVideoMethod;
        Play += PlayVideoMethod;
        NewLog += NewLogMethod;
        CloseLog += CloseLogMethod;
        CancelLog += CancelLogMethod;
        CancelOpenLog += CancelOpenLogMethod;
        CopyPlayerTimeCode += CopyPlayerTimeCodeMethod;
        Tab += TabMethod;
        BackToLive += BackToLiveMethod;
        ClearMarkers += ClearMarkersMethod;
        CopyLog += CopyLogMethod;
        PasteLog += PasteLogMethod;
        ChangeLogTCIN += ChangeLogTCINMethod;
        ChangeLogTCOUT += ChangeLogTCINMethod;
        RightFrame += RightFrameMethod;
        LeftFrame += LeftFrameMethod;
        F1 += F1Method;
        F2 += F2Method;
        F3 += F3Method;
        F4 += F4Method;
        F5 += F5Method;
    }

    private static void F1Method()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Shift keydown event
        var shiftDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Shift
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(shiftDownEvent);

        // Send the F1 keydown event
        var f1DownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x70,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3B,                               // Scan code for F1
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f1DownEvent);

        // Send the F1 keyup event
        var f1UpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x70,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3B,                               // Scan code for F1
                           Type = KeyEventType.KeyUp,                          // KeyUp event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f1UpEvent);

        // Send the Shift keyup event
        var shiftUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Shift
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(shiftUpEvent);

        // Send the Ctrl keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                      // No modifiers
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private static void F2Method()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Shift keydown event
        var shiftDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Shift
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(shiftDownEvent);

        var f2DownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x71,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3C,                               // Scan code for F1
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f2DownEvent);

        var f2UpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x71,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3C,                               // Scan code for F1
                           Type = KeyEventType.KeyUp,                          // KeyUp event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f2UpEvent);

        // Send the Shift keyup event
        var shiftUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Shift
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(shiftUpEvent);

        // Send the Ctrl keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                      // No modifiers
        };
        browserHost.SendKeyEvent(ctrlUpEvent);

    }

    private static void F3Method()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Shift keydown event
        var shiftDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Shift
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(shiftDownEvent);

        var f3DownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x72,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3D,                               // Scan code for F1
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f3DownEvent);

        var f3UpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x72,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3D,                               // Scan code for F1
                           Type = KeyEventType.KeyUp,                          // KeyUp event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f3UpEvent);

        // Send the Shift keyup event
        var shiftUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Shift
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(shiftUpEvent);

        // Send the Ctrl keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                      // No modifiers
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private static void F4Method()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Shift keydown event
        var shiftDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Shift
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(shiftDownEvent);

        var f4DownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x73,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3E,                               // Scan code for F1
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f4DownEvent);

        var f4UpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x73,                              // VK_F1 (F1 key)
                           NativeKeyCode =0x3E,                               // Scan code for F1
                           Type = KeyEventType.KeyUp,                          // KeyUp event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f4UpEvent);

        // Send the Shift keyup event
        var shiftUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Shift
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(shiftUpEvent);

        // Send the Ctrl keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                      // No modifiers
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private static void F5Method()
    {
        if (selectedBrowser == null)
            return;

        var browserHost = selectedBrowser.browser.GetBrowserHost();

        // Send the Ctrl keydown event
        var ctrlDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Ctrl
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(ctrlDownEvent);

        // Send the Shift keydown event
        var shiftDownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for Shift
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(shiftDownEvent);

        var f5DownEvent = new KeyEvent
        {
            WindowsKeyCode = 0x74,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3F,                               // Scan code for F1
                           Type = KeyEventType.RawKeyDown,                     // RawKeyDown event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f5DownEvent);

        var f5UpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x74,                              // VK_F1 (F1 key)
                           NativeKeyCode = 0x3F,                               // Scan code for F1
                           Type = KeyEventType.KeyUp,                          // KeyUp event for F1
                           Modifiers = CefEventFlags.ShiftDown | CefEventFlags.ControlDown  // Shift and Ctrl modifiers
        };
        browserHost.SendKeyEvent(f5UpEvent);

        // Send the Shift keyup event
        var shiftUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x10,                              // VK_SHIFT (Shift key)
                           NativeKeyCode = 0x2A,                               // Scan code for Shift
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Shift
                           Modifiers = CefEventFlags.ControlDown               // Ctrl modifier
        };
        browserHost.SendKeyEvent(shiftUpEvent);

        // Send the Ctrl keyup event
        var ctrlUpEvent = new KeyEvent
        {
            WindowsKeyCode = 0x11,                              // VK_CONTROL (Ctrl key)
                           NativeKeyCode = 0x1D,                               // Scan code for Ctrl
                           Type = KeyEventType.KeyUp,                          // KeyUp event for Ctrl
                           Modifiers = CefEventFlags.None                      // No modifiers
        };
        browserHost.SendKeyEvent(ctrlUpEvent);
    }

    private static void PasteMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_V, ctrl: true);
    }

    private static void RightFrameMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.RIGHT, ctrl: true);
    }

    private static void LeftFrameMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.LEFT, ctrl: true);
    }

    private static void CopyLogMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_C, lowerCase: true, ctrl: true);
    }

    private static void PasteLogMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_V, lowerCase: true, ctrl: true);
    }

    private static void ChangeLogTCINMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_I, lowerCase: true, ctrl: true);
    }

    private static void ChangeLogTCOUTMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_O, lowerCase: true, ctrl: true);
    }

    public static void LogInRadioEdit()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_D, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_E, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_M, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_O, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.TAB, lowerCase: true);
    }

    private static void ClearMarkersMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_R, ctrl: true);
    }

    private static void CopyPlayerTimeCodeMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
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

    private static void BackToLiveMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
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

    private static void TabMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
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

    private static void CancelOpenLogMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
            return;

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

    private static void CancelLogMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
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

    private static void CloseLogMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
            return;

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

    private static void NewLogMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_A, ctrl: true);
    }

    private static void PlayVideoMethod()
    {
        if (selectedBrowser == null || selectedBrowser.browser.GetBrowserHost() == null)
            return;

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

    private static void PauseVideoMethod()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_P, ctrl: true);
    }
}
