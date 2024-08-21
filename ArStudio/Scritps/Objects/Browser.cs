using System;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.DevTools.Input;
using CefSharp.OffScreen;
using StereoKit;

namespace TestWebAr.Scritps.Objects;
 class Browser
{
    public Tex Texture { get; internal set; }
    public string Url
    {
        get => browser == null ? url : browser.Address;
        set
        {
            url = value;
            if (browser != null)
                browser.LoadUrl(url);
        }
    }

    Action<Browser> setSelectedBrowser;

    public ChromiumWebBrowser browser;
    private CefOffScreenDropdownHandler dropDownHandler;

    Tex[] tex;
    int texCurr = 0;
    string url;

    public bool selected = false;

    public float browserAspect = 9.0f / 16.0f;
    public Material material;

    Pose windowPosition;
    public string name;

    public Browser(string url, string name, Pose windowPosition)
    {
        this.name = name;
        this.windowPosition = windowPosition;

        Texture = Tex.White;
        Url = url;
        tex = new Tex[]
        {
            new Tex(TexType.ImageNomips, TexFormat.Bgra32),
            new Tex(TexType.ImageNomips, TexFormat.Bgra32)
        };
        tex[0].AddressMode = TexAddress.Clamp;
        tex[1].AddressMode = TexAddress.Clamp;
        material = Material.Unlit.Copy();

#if ANYCPU
        CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
        Init();
    }


    async Task Init()
    {
        browser = new ChromiumWebBrowser(Url);
        dropDownHandler = new CefOffScreenDropdownHandler(browser);
        await browser.WaitForInitialLoadAsync();
        browser.Paint += Browser_Paint;
        browserAspect = browser.Size.Height / (float)browser.Size.Width;
        Mute();
        InitializeSetVolumerJsFunction();
        browser.ShowDevTools();
    }

    private void Browser_Paint(object sender, OnPaintEventArgs e)
    {
        tex[texCurr].SetColors(e.Width, e.Height, e.BufferHandle);
        Texture = tex[texCurr];
        texCurr = (texCurr + 1) % 2;
    }

    public void SendKey(
        IBrowser browser,
        CefEventFlags modifiers,
        KeyEventType type,
        int windowsKeyCode,
        int nativeKeyCode
    )
    {
        KeyEvent keyEvent = new KeyEvent
        {
            Type = type,
            Modifiers = modifiers,
            WindowsKeyCode = windowsKeyCode,
            NativeKeyCode = nativeKeyCode,
            FocusOnEditableField = true
        };

        browser.GetHost().SendKeyEvent(keyEvent);
    }

    private void InitializeSetVolumerJsFunction()
    {
        var script = @"
            (function() {
                window.setVolume = function(volume) {
                    document.querySelectorAll('video, audio').forEach(function(mediaElement) {
                        mediaElement.volume = volume;
                    });
                };
            })();
        ";

        browser.ExecuteScriptAsync(script);
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.02)
        {
            volume = 0;
        }
        browser.ExecuteScriptAsync($"setVolume({volume.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
    }

    public void Mute()
    {
        browser.GetBrowserHost().SetAudioMuted(true);
    }

    public void UnMute()
    {
        browser.GetBrowserHost().SetAudioMuted(false);
        browser.GetBrowserHost().SetFocus(true);
    }

    public void BindBrowserSelect(Action<Browser> action)
    {
        setSelectedBrowser += action;
        setSelectedBrowser += (browser) =>
        {
            UnMute();
        };
    }

    public TouchPoint TouchPoint(Bounds bounds, Handed hand)
    {

        Hand h = Input.Hand(hand);
        HandJoint j = h[FingerId.Index, JointId.Tip];
        Plane p = new Plane(
            V.XYZ(bounds.center.x, bounds.center.y, bounds.center.z - bounds.dimensions.z / 2),
            Vec3.Forward
        );
        Vec3 at = p.Closest(Hierarchy.ToLocal(j.position));

        //Mesh.Sphere.Draw(Material.Default, Matrix.TS(at, 0.01f));

        Vec3 pt = at - (bounds.center + bounds.dimensions * 0.5f);
        pt = new Vec3(-pt.x / bounds.dimensions.x, -pt.y / bounds.dimensions.y, 0);

        return new TouchPoint
        {
            X = pt.x * browser.Size.Width,
            Y = pt.y * browser.Size.Height,
            RadiusX = j.radius,
            RadiusY = j.radius,
        };
    }

    Vec2 startAt;
    Vec2 prevAt;

    public void UpdateBrowser()
    {
        UI.WindowBegin(name, ref windowPosition, V.XY(0.6f, 0), UIWin.Body, UIMove.FaceUser);
        if (selected)
        {
            UI.SetThemeColor(UIColor.Background, Color.White);
        }
        else
        {
            UI.SetThemeColor(UIColor.Background, Color.Black);
        }
        StepAsUI();
        UI.WindowEnd();

        if (dropDownHandler != null && dropDownHandler.IsDropdownOpened()) {
            // add custom drop down to manually select
            Pose buttonsPose = new Pose(windowPosition.position.x, windowPosition.position.y, windowPosition.position.z + 0.05f);
            UI.WindowBegin("Dropdown Menu", ref buttonsPose);
            if (UI.Button("Foot")) {
                SelectDropdownOption(browser.GetMainFrame(), "form-control ng-pristine ng-valid, ng-touched", "1");
            }
            if (UI.Button("Truc")) {
                SelectDropdownOption(browser.GetMainFrame(), "form-control ng-pristine ng-valid, ng-touched", "2");
            }
            UI.WindowEnd();
        }
    }

    private async Task SelectDropdownOption(IFrame frame, string selectId, string optionValue) {
        string script = $@"
        (function() {{
            function selectOption(selectId, optionValue) {{
                var selectElement = document.getElementById(selectId);
                if (selectElement) {{
                    selectElement.value = optionValue;
                    var event = new Event('change', {{ bubbles: true }});
                    selectElement.dispatchEvent(event);
                }} else {{
                    console.error('Select element with ID ' + selectId + ' not found.');
                }}
            }}

            selectOption('{selectId}', '{optionValue}');
        }})();
        ";

        frame.ExecuteJavaScriptAsync(script);
    } // form-control ng-pristine ng-valid ng-touched


    private void StepAsUI()
    {
        float width = UI.LayoutRemaining.x;
        Bounds bounds = UI.LayoutReserve(new Vec2(width, browserAspect * width));
        bounds.center.z += 0.01f;
        bounds.dimensions.z += 0.03f;
        BtnState state = UI.VolumeAt("browser", bounds, UIConfirm.Push, out Handed hand);

        material[MatParamName.DiffuseTex] = Texture;

        Mesh.Quad.Draw(
            material,
            Matrix.TS(bounds.center + V.XYZ(0, 0, -0.015f), bounds.dimensions)
        );

        if (browser == null || !browser.IsBrowserInitialized)
            return;

        if (state.IsJustActive())
        {
            TouchPoint pt = TouchPoint(bounds, hand);
            startAt = prevAt = new Vec2((float)pt.X, (float)pt.Y);
            browser
                .GetDevToolsClient()
                .Input.DispatchTouchEventAsync(
                    DispatchTouchEventType.TouchStart,
                    new TouchPoint[] { pt }
                );
        }
        if (state.IsActive())
        {
            TouchPoint pt = TouchPoint(bounds, hand);
            Vec2 currAt = new Vec2((float)pt.X, (float)pt.Y);
            if (!Vec2.InRadius(currAt, startAt, 6) && !Vec2.InRadius(currAt, prevAt, 1))
            {
                browser
                    .GetDevToolsClient()
                    .Input.DispatchTouchEventAsync(
                        DispatchTouchEventType.TouchMove,
                        new TouchPoint[] { pt }
                    );
                prevAt = currAt;
            }
        }
        if (state.IsJustInactive())
        {
            TouchPoint pt = new TouchPoint { X = prevAt.x, Y = prevAt.y, };
            browser
                .GetDevToolsClient()
                .Input.DispatchTouchEventAsync(
                    DispatchTouchEventType.TouchEnd,
                    new TouchPoint[] { pt }
                );

            setSelectedBrowser.Invoke(this);
        }
    }

    public bool HasForward => browser == null ? false : browser.CanGoForward;
    public bool HasBack => browser == null ? false : browser.CanGoBack;

    public void Back() => browser?.Back();
    public void Forward() => browser?.Forward();
}
