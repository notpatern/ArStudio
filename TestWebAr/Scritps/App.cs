using System;
using CefSharp;
using CefSharp.OffScreen;
using StereoKit;
using StereoKit.Framework;

public class App
{
    Browser browserInstance;
    ChromiumWebBrowser browser;
    Pose windowPose;

    Material floorMaterial;
    string userUrl;

    public void Init()
    {
        SKSettings settings = new SKSettings
        {
            appName = "TestWebAr",
            assetsFolder = "Assets",
            mode = AppMode.Simulator
        };

        if (!SK.Initialize(settings))
            Environment.Exit(1);

        browserInstance = new Browser("https://skylog-m6.broadteam.eu/login");
        windowPose = new Pose(0, 0, -0.5f, Quat.LookDir(0, 0, 1));

        floorMaterial = new Material("floor.hlsl");
        floorMaterial.Transparency = Transparency.Blend;

        userUrl = "https://skylog-m6.broadteam.eu/login";
    }

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

        CheckAndForwardKey(Key.Num0, VirtualKeyCode.VK_0);
        CheckAndForwardKey(Key.Num1, VirtualKeyCode.VK_1);
        CheckAndForwardKey(Key.Num2, VirtualKeyCode.VK_2);
        CheckAndForwardKey(Key.Num3, VirtualKeyCode.VK_3);
        CheckAndForwardKey(Key.Num4, VirtualKeyCode.VK_4);
        CheckAndForwardKey(Key.Num5, VirtualKeyCode.VK_5);
        CheckAndForwardKey(Key.Num6, VirtualKeyCode.VK_6);
        CheckAndForwardKey(Key.Num7, VirtualKeyCode.VK_7);
        CheckAndForwardKey(Key.Num8, VirtualKeyCode.VK_8);
        CheckAndForwardKey(Key.Num9, VirtualKeyCode.VK_9);

        CheckAndForwardKey(Key.Space, VirtualKeyCode.SPACE);
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

    private void ForwardKeyToCef(VirtualKeyCode key)
    {
        browser = browserInstance.browser;
        if (browser != null)
        {
            browserInstance.SendKey(
                browser.GetBrowser(),
                CefEventFlags.None,
                KeyEventType.RawKeyDown,
                (int)key,
                0
            );
            browserInstance.SendKey(
                browser.GetBrowser(),
                CefEventFlags.None,
                KeyEventType.Char,
                (int)key,
                0
            );
            browserInstance.SendKey(
                browser.GetBrowser(),
                CefEventFlags.None,
                KeyEventType.KeyUp,
                (int)key,
                0
            );
        }
    }

    private void UpdateBrowser()
    {
        UI.WindowBegin("Browser", ref windowPose, V.XY(0.6f, 0), UIWin.Body, UIMove.FaceUser);

        UI.PushEnabled(browserInstance.HasBack);
        if (UI.Button("Back"))
            browserInstance.Back();
        UI.PopEnabled();

        UI.SameLine();
        UI.PushEnabled(browserInstance.HasForward);
        if (UI.Button("Forward"))
            browserInstance.Forward();
        UI.PopEnabled();

        UI.SameLine();
        UI.PanelBegin();
        if (
            UI.Input("url", ref userUrl, V.XY(UI.LayoutRemaining.x, 0))
            && Input.Key(Key.Return).IsActive()
            && userUrl != ""
        )
        {
            browserInstance.Url = userUrl;
        }
        UI.Label(browserInstance.Url, V.XY(UI.LayoutRemaining.x, 0));
        UI.PanelEnd();
        browserInstance.StepAsUI();
        UI.WindowEnd();
    }

    public void Update()
    {
        if (SK.System.displayType == Display.Opaque)
            Default.MeshCube.Draw(
                floorMaterial,
                World.HasBounds
                    ? World.BoundsPose.ToMatrix(new Vec3(30, 0.1f, 30))
                    : Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30))
            );

        CaptureKeyboardInput();

        UpdateBrowser();
    }
}

public enum VirtualKeyCode
{
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
    SPACE = 0x20,
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
