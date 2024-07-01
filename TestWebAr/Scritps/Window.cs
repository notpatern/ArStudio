using CefSharp;
using CefSharp.DevTools.Input;
using StereoKit;
using StereoKit.Framework;

public class Window {
    private Pose windowPosition;
    private string name;
    public Browser browser;
    private string userUrl;

    public Window(Browser browser, Pose windowPosition, string name, string url = null) {
        this.browser = browser;
        this.windowPosition = windowPosition;
        this.name = name;
        this.userUrl = url;
    }

    public void UpdateWindow() {
        UI.WindowBegin(name, ref windowPosition, V.XY(0.6f, 0), UIWin.Body, UIMove.FaceUser);

        UI.PushEnabled(browser.HasBack);
        if (UI.Button("Back"))
            browser.Back();
        UI.PopEnabled();

        UI.SameLine();
        UI.PushEnabled(browser.HasForward);
        if (UI.Button("Forward"))
            browser.Forward();
        UI.PopEnabled();

        UI.SameLine();
        UI.PanelBegin();
        if (
                UI.Input("url", ref userUrl, V.XY(UI.LayoutRemaining.x, 0))
                && Input.Key(Key.Return).IsActive()
                && userUrl != ""
           ) {
            browser.Url = userUrl;
        }
        UI.Label(browser.Url, V.XY(UI.LayoutRemaining.x, 0));
        UI.PanelEnd();
        StepAsUI();
        UI.WindowEnd();
    }

    Vec2 startAt;
    Vec2 prevAt;
    private void StepAsUI() {
        float width = UI.LayoutRemaining.x;
        Bounds bounds = UI.LayoutReserve(new Vec2(width, browser.browserAspect * width));

        bounds.center.z += 0.01f;
        bounds.dimensions.z += 0.03f;
        BtnState state = UI.VolumeAt("browser", bounds, UIConfirm.Push, out Handed hand);

        browser.material[MatParamName.DiffuseTex] = browser.Texture;
        Mesh.Quad.Draw(
                browser.material,
                Matrix.TS(bounds.center + V.XYZ(0, 0, -0.015f), bounds.dimensions)
                );

        if (browser == null || !browser.browser.IsBrowserInitialized)
            return;

        if (state.IsJustActive()) {
            TouchPoint pt = browser.TouchPoint(bounds, hand);

            startAt = prevAt = new Vec2((float)pt.X, (float)pt.Y);
            browser.browser
                .GetDevToolsClient()
                .Input.DispatchTouchEventAsync(
                        DispatchTouchEventType.TouchStart,
                        new TouchPoint[] { pt }
                        );
        }
        if (state.IsActive()) {
            TouchPoint pt = browser.TouchPoint(bounds, hand);
            Vec2 currAt = new Vec2((float)pt.X, (float)pt.Y);
            if (!Vec2.InRadius(currAt, startAt, 6) && !Vec2.InRadius(currAt, prevAt, 1)) {
                browser.browser
                    .GetDevToolsClient()
                    .Input.DispatchTouchEventAsync(
                            DispatchTouchEventType.TouchMove,
                            new TouchPoint[] { pt }
                            );
                prevAt = currAt;
            }
        }
        if (state.IsJustInactive()) {
            TouchPoint pt = new TouchPoint { X = prevAt.x, Y = prevAt.y, };
            browser.browser
                .GetDevToolsClient()
                .Input.DispatchTouchEventAsync(
                        DispatchTouchEventType.TouchEnd,
                        new TouchPoint[] { pt }
                        );
        }
    }
}
