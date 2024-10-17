using System;
using System.Collections.Generic;
using Scritps.Services;
using StereoKit;
using TestWebAr.Scritps.Objects;

public class DefaultSkyLog : IDisposable {
    bool bVolumeSlider;
    bool bButtons;
    bool bHotKeyPanel;

    HandTracking handTracking;
    ButtonWindow buttonWindow;
    PlayerHotKeys playerHotKeys;
    protected List<Browser> browserList = new List<Browser>();
    Browser selectedBrowser;
    Browser dirtyBrowser = null;
    VolumeSlider volumeSlider;
    Pose browserSelectPosition = new Pose(new Vec3(-0.3f, -0.2f, -0.3f), Quat.LookDir(0, 0, 1));
    KeyForwarder keyForwarder = new KeyForwarder();

    protected int browserAmount;
    protected Root config;
    protected string url;

    public DefaultSkyLog(Root config, bool volumeSlider = false, bool buttons = false, bool hotKeyPanel = false) {
        this.config = config;
        bVolumeSlider = volumeSlider;
        bButtons = buttons;
        bHotKeyPanel = hotKeyPanel;
        Init();
    }

    private void InitiateVolumeSlider() {
        if (!bVolumeSlider) {
            return;
        }

        volumeSlider = new VolumeSlider("Volume", new Pose(0.3f, -0.2f, -0.3f, Quat.LookDir(0, 0, 1)));
    }

    private void InitiateButtonWindow() {
        if (!bButtons) {
            return;
        }

        buttonWindow = new ButtonWindow("buttons", new Pose(0.4f, -0.2f, -0.3f, Quat.LookDir(0, 0, 1)));
    }

    private void InitiateHotKeyPanel() {
        if (!bHotKeyPanel) {
            return;
        }

        playerHotKeys = new PlayerHotKeys(new Pose(0, -0.2f, -0.4f, Quat.LookDir(0, 0, 1)));
    }

    protected virtual void Init()
    {
        InitiateVolumeSlider();
        InitiateButtonWindow();
        InitiateHotKeyPanel();
        InitSkyLogBrowsers();
        SkyLogEvents.keyForwarder = keyForwarder;
        BindEvents();
        handTracking = new HandTracking();
    }

    protected virtual void InitSkyLogBrowsers() {
        browserAmount = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0 && i == 1)
                {
                    continue;
                }

                Quat lookDirection = Quat.LookDir(0, 0, 1);

                Vec3 windowPosition = new Vec3(0.65f * j, 0.005f - (float)(0.36 * -i), -0.7f);

                Vec2 scale = new Vec2(0.5f, 0.5f);

                if (j == 0)
                {
                    windowPosition.y = 0.285f;
                    scale = new Vec2(0.8f, 0.8f);
                }

                if (j != 0)
                {
                    Vec3 directionVector = Input.Head.position - windowPosition;
                    directionVector.y = 0;
                    directionVector = directionVector.Normalized;
                    lookDirection = Quat.LookAt(Vec3.Forward, directionVector);

                    windowPosition.z = windowPosition.z + 0.1f;
                }

                url = "https://skylog-demo.broadteam.eu/";

                if (j == -1 && i == 0) {
                    url = config.defaultdemo.topLeftScreen;
                }
                else if (j == 1 && i == 0) {
                    url = config.defaultdemo.topRightScreen;
                }
                else if (j == -1 && i == 1) {
                    url = config.defaultdemo.bottomLeftScreen;
                }
                else if (j == 1 && i == 1) {
                    url = config.defaultdemo.bottomRightScreen;
                }

                browserList.Add(
                        new Browser(
                            url,
                            browserAmount.ToString(),
                            new Pose(windowPosition, lookDirection),
                            scale.x,
                            scale.y
                            )
                        );


                while (browserList[browserAmount].browser == null) { }
                while (!browserList[browserAmount].browser.IsBrowserInitialized) { }

                BindVolumeSlider(browserList[i]);

                browserList[browserAmount].BindBrowserSelect(SelectBrowser);
                browserAmount++;
            }
        }
    }

    protected void BindVolumeSlider(Browser browser) {
        if (!bVolumeSlider) {
            return;
        }

        volumeSlider.BindVolumeAction(browser.SetVolume);
    }

    protected void SelectBrowser(Browser browser)
    {
        selectedBrowser = browser;
        browser.selected = true;
        SkyLogEvents.selectedBrowser = selectedBrowser;
        if (dirtyBrowser != null && dirtyBrowser != selectedBrowser)
        {
            dirtyBrowser.Mute();
            dirtyBrowser.selected = false;
        }
        dirtyBrowser = browser;
    }

    private void BindEvents()
    {
        SkyLogEvents.BindEvents();
    }

    private void UpdateBrowsers()
    {
        foreach (Browser browser in browserList)
        {
            browser.UpdateBrowser();
        }
    }

    protected virtual void LogInRadioEdit()
    {
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_D, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_E, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_M, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.VK_O, lowerCase: true);
        keyForwarder.ForwardKeyToCef(selectedBrowser, VirtualKeyCode.TAB, lowerCase: true);
    }

    private void BrowserSelectPanel()
    {
        UI.WindowBegin("Select Window", ref browserSelectPosition);
        for (int i = 0; i < browserAmount; i++)
        {
            if (UI.Button("Screen " + (i + 1)))
            {
                SelectBrowser(browserList[i]);
            }
        }

        UI.VSpace(0.02f);

        if (UI.Button("Refresh")) {
            if (selectedBrowser != null)
            {
                selectedBrowser.Refresh();
            }
        }

        if (UI.Button("Refresh All")) {
            foreach (Browser browser in browserList) {
                browser.Refresh();
            }
        }

        UI.WindowEnd();
    }

    public virtual void Update()
    {
        if (bVolumeSlider) {
            volumeSlider.UpdateSlider();
        }
        if (bButtons) {
            buttonWindow.UpdateWindow();
        }
        if (bHotKeyPanel) {
            playerHotKeys.Update();
        }
        CaptureKeyboardInput();
        UpdateBrowsers();
        BrowserSelectPanel();
    }

    public void Dispose() {
        foreach (Browser browser in browserList) {
            browser.browser.Dispose();
            browser.browser = null;
        }
        handTracking.Dispose();
        GC.SuppressFinalize(this);
    }

    // ------------------------------------------------------------------------------------------------------------

    private void CaptureKeyboardInput()
    {
        // Add more keys as needed
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.A, VirtualKeyCode.VK_A);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.B, VirtualKeyCode.VK_B);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.C, VirtualKeyCode.VK_C);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.D, VirtualKeyCode.VK_D);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.E, VirtualKeyCode.VK_E);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F, VirtualKeyCode.VK_F);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.G, VirtualKeyCode.VK_G);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.H, VirtualKeyCode.VK_H);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.I, VirtualKeyCode.VK_I);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.J, VirtualKeyCode.VK_J);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.K, VirtualKeyCode.VK_K);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.L, VirtualKeyCode.VK_L);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.M, VirtualKeyCode.VK_M);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N, VirtualKeyCode.VK_N);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.O, VirtualKeyCode.VK_O);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.P, VirtualKeyCode.VK_P);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Q, VirtualKeyCode.VK_Q);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.R, VirtualKeyCode.VK_R);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.S, VirtualKeyCode.VK_S);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.T, VirtualKeyCode.VK_T);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.U, VirtualKeyCode.VK_U);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.V, VirtualKeyCode.VK_V);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.W, VirtualKeyCode.VK_W);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.X, VirtualKeyCode.VK_X);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Y, VirtualKeyCode.VK_Y);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Z, VirtualKeyCode.VK_Z);

        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N0, VirtualKeyCode.VK_0);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N1, VirtualKeyCode.VK_1);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N2, VirtualKeyCode.VK_2);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N3, VirtualKeyCode.VK_3);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N4, VirtualKeyCode.VK_4);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N5, VirtualKeyCode.VK_5);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N6, VirtualKeyCode.VK_6);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N7, VirtualKeyCode.VK_7);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N8, VirtualKeyCode.VK_8);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.N9, VirtualKeyCode.VK_9);

        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Space, VirtualKeyCode.VK_SPACE);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Return, VirtualKeyCode.RETURN);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Backspace, VirtualKeyCode.BACK);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Tab, VirtualKeyCode.TAB);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Esc, VirtualKeyCode.ESCAPE);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Left, VirtualKeyCode.LEFT);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Right, VirtualKeyCode.RIGHT);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Up, VirtualKeyCode.UP);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.Down, VirtualKeyCode.DOWN);

        // Add function keys
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F1, VirtualKeyCode.F1);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F2, VirtualKeyCode.F2);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F3, VirtualKeyCode.F3);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F4, VirtualKeyCode.F4);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F5, VirtualKeyCode.F5);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F6, VirtualKeyCode.F6);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F7, VirtualKeyCode.F7);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F8, VirtualKeyCode.F8);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F9, VirtualKeyCode.F9);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F10, VirtualKeyCode.F10);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F11, VirtualKeyCode.F11);
        keyForwarder.CheckAndForwardKey(selectedBrowser, Key.F12, VirtualKeyCode.F12);
    }
}
