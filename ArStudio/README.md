# ArStudio Documentation

## Project Overview:
### Project Structure:
```
ArStudio/
    ├───Program.cs
    └───Scritps/
        ├───App.cs
        ├───Config/
        │   └───ConfigJason.cs
        ├───Objects/
        │   ├───Browser.cs
        │   ├───ButtonWindow.cs
        │   ├───PlayerHotkeys.cs
        │   └───VolumeSlider.cs
        └───Services/
            ├───CefInstance.cs
            ├───CefOffScreenHandler.cs
            ├───DefaultSkyLog.cs
            ├───HandTracking.cs
            ├───ObsSkyLog.cs
            ├───ObsWebSocket.cs
            └───VolumeSkyLog.cs
```

### Project Dependencies:

[Stereokit](https://github.com/StereoKit/StereoKit) - AR/VR Engine
[obs-websocket-dotnet](https://github.com/BarRaider/obs-websocket-dotnet) - Remote Control OBS
[CefSharp](https://github.com/cefsharp/CefSharp) - Bowser Embedding

### Specifications:

If working on the project, feed the config.json located in the Config directory.
In the config you will find three objects: "default-demo", "volume-demo", "obs-demo".
This file is needed for the program to run. You can feed any URL in the screen fields in each demo object and it will render the Web Page on the AR Screen. However, one must know that feeding an incorrect Ip or Port in the "obs-demo" object *will* crash the program when launching the ObsDemo.

If the project is published, the config file has to be in the *Config* directory where the *ArStudio.exe* is.
The same constraints apply.

## Purpose:

ArStudio's goal is to pilot SkyLog and Obs (as of now) in an AR Environment.
With the rise of Remote Work, ArStudio aims to allow Tv Studio management to work from home by providing them with a fully fledged workspace in AR.

## Code Documentation:

*Program.cs*, and more precisely *void Main()* runs the app.
In order to run [Cef](https://github.com/chromiumembedded) sub-processes such as the *Off-Screen Browser Renderer* (contained in the Cefsharp dependency) the program must first Initialize it using:

```cs
CefInstance.InitializeCef();
```
This allocates a thread to the Cef process and passes in the config info set in the *CefInstance* class.
Then the program instantiates a new *App* object and initializes it, which also initializes *StereoKit* in the process. Finally it calls the *StereoKit* loop which will, well, "run" the processes needed for the app to work :)

**This documentation will go over the *Important* parts of the code that a developer might need to understand before working hands on the project.**

### App.cs

There are 2 main methods in the *App* class: *Init()* and *Update()*.

The *Init()* is called once att the begining of the program. It parses the *config.json* file and initializes *StereoKit*.
```cs
    public void Init()
    {
        string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string configPath = Path.Combine(exeDirectory, "Config\\config.json");
        Console.WriteLine(configPath);
        string configJson = File.ReadAllText(configPath);

        config = JsonConvert.DeserializeObject<Root>(configJson);
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
```

The *Update()* method is called every tick of the program. It invokes the *UpdateEvent*.
```cs
    public void Update()
    {
        UpdateEvent.Invoke();
    }
```

The *UpdateEvent* is an event of *UpdateHandler* type:
```cs
    delegate void UpdateHandler();
    event UpdateHandler UpdateEvent;
```

Depending on which environment has to be displayed, the method bound to *UpdateEvent* will change.
Initially, the *SelectMenuUpdate()*.

If one needs to add a new Demo Scene, one must add their Scene to the following method in *App.cs*.
```cs
    private void SelectMenuUpdate()
    {
        UI.WindowBegin("Ar Studio", ref menuSelectPosition);
        if (UI.Button("Default"))
        {
            skylog = new DefaultSkyLog(config, volumeSlider: true, hotKeyPanel: true);
            UpdateEvent = skylog.Update;
            UpdateEvent += BackToMenu;
        }

        if (UI.Button("Volume Demo"))
        {
            skylog = new VolumeSkyLog(config, volumeSlider: true);
            UpdateEvent = skylog.Update;
            UpdateEvent += BackToMenu;
        }

        if (UI.Button("Obs Remote Control"))
        {
            skylog = new ObsSkyLog(config, volumeSlider: true);
            UpdateEvent = skylog.Update;
            UpdateEvent += BackToMenu;
        }

        // The following code is an example of how one might implement a new Scene option.
        if (UI.Button("New Scene"))
        {
            skylog = new Scene();
            UpdateEvent = skylog.Update;
            UpdateEvent += BackToMenu;
        }
        // End of example.

        UI.WindowEnd();
    }
```

### Browser.cs

The *Browser* class implements the *Cef off screen browser*. To render this browser in the program, it binds the *Browser_Paint()* method to the *Paint* event contained in *ChromiumWebBrowser*.
```cs
    private void Browser_Paint(object sender, OnPaintEventArgs e)
    {
        tex[texCurr].SetColors(e.Width, e.Height, e.BufferHandle);
        Texture = tex[texCurr];
        texCurr = (texCurr + 1) % 2;
    }
```

It copies the browser's renderer's bitmap to a texture. To interact with the browser in the Ar environment, *Browser* also contains *StepAsUi()*.
```cs
    private void StepAsUI()
    {
        float width = UI.LayoutRemaining.x;
        Bounds bounds = UI.LayoutReserve(new Vec2(browserScale.x, browserScale.y * browserAspect));
        bounds.center.z += 0.01f;
        bounds.dimensions.z += 0.03f;
        BtnState state = UI.VolumeAt("browser", bounds, UIConfirm.Push, out Handed hand);

        material[MatParamName.DiffuseTex] = Texture;

        Matrix transform = Matrix.TRS(bounds.center + V.XYZ(0, 0, -0.015f),
                Quat.Identity,
                bounds.dimensions);

        Mesh.Quad.Draw(material, transform);

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
```

When the user's hand get in contact with the Texture, a TouchPoint (click position) is dispatched to the browser, letting the user interact with the Off Screen browser.

**Note**: If an undefined error is encontered when interacting with the browser, one might need to debug this class. The error is most probably in *Browser* or *ChromiumWebBrowser*.

### SkyLogEvents.cs

*SkyLogEvents* is a static class which's purpose is to dispatch Keyboard shortcuts to the browser in order to pilot the *SkyLog* Web app.
It declares many public static events that any class can Invoke. This choice is architecturally *mid* but was made when the project was still only in the Research and Development phase.
In order to bind all the events to their respective methods, the *BindEvents()* method is called in the *DefaultSkyLog*'s base *Init()* method.
Although virtually any class could access to the contents and logic of *SkyLogEvents*, only *HandTracking* does.

### HandTracking.cs

*HandTracking* Analyzes the user's hands and interprets their info (position, velocity, shape) to call the *SkyLogEvents* accordingly.
Every 20 milliseconds, it calculates the velocity and angle of the hands as well as runs checks in order to call the events.
If one wants to change or add Gestures to control the SkyLogEvents, they will have to add a condition and an event invoke in *UpdateHandTrackingChecks()*
```cs
        private void UpdateHandTrackingChecks(object sender, ElapsedEventArgs e)
        {
            if (inputBuffer < 0.3f) {
                inputBuffer += (float)fixedDeltaTime;
                return;
            }

            // log commands
            if (Vec3.Distance(leftHandData.position, rightHandData.position) <= 0.07f)
            {
                if (leftHandData.velocity.x >= 100 && rightHandData.velocity.x <= 100)
                {
                    if (leftHandData.hand.IsPinched && rightHandData.hand.IsPinched)
                    {
                        SkyLogEvents.CancelOpenLog.Invoke();
                        inputBuffer = 0;
                    }
                }
            }

            if (Vec3.Dot(leftHandData.hand.palm.Forward, headForward) <= -0.60)
            {
                if (rightHandData.velocity.x >= 120 && rightHandData.angleDegree.x >= 25)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.NewLog.Invoke();
                        inputBuffer = 0;
                    }
                }

                if (rightHandData.velocity.x <= -120 && rightHandData.angleDegree.x <= -25)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.CloseLog.Invoke();
                        inputBuffer = 0;
                    }
                }

                if (rightHandData.velocity.y >= 120 && rightHandData.angleDegree.y >= 20)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.CancelLog.Invoke();
                        inputBuffer = 0;
                    }
                }
            }

            // player commands
            if (leftHandData.hand.IsGripped && leftHandData.angleDegree.y > -10) {
                if (rightHandData.velocity.y <= -100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.Pause.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.y >= 100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.Play.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.x >= 100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.RightFrame.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.x <= -100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.LeftFrame.Invoke();
                    inputBuffer = 0;
                }
            }

            if (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, leftHandData.hand.palm.Forward.Normalized) <= -0.60) {
                if (rightHandData.velocity.y >= 120) {
                    SkyLogEvents.BackToLive.Invoke();
                    inputBuffer = 0;
                }
            }

            if (leftHandData.velocity.x <= -100 && rightHandData.velocity.x >= 100) {
                if (leftHandData.hand.IsPinched && rightHandData.hand.IsPinched) {
                    SkyLogEvents.ClearMarkers.Invoke();
                    inputBuffer = 0;
                }
            }

            if (leftHandData.hand.IsGripped) {
                SkyLogEvents.CopyPlayerTimeCode.Invoke();
                // no need for input buffer as it sends space repetively so that the use can copy.
            }

            if (leftHandData.velocity.x >= 120 && leftHandData.angleDegree.x <= -30)
            {
                SkyLogEvents.Tab.Invoke();
                inputBuffer = 0;
            }
        }
```
*HandTracking* implements *IDisposable* allowing the program to manually clear the memory and processes used by the class.
This was implemented in order to fix an issue where the hands kepts on being tracked although the SkyLog scene had been disposed of.
