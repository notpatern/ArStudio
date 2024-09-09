using ArStudio.Scritps.Services;
using StereoKit;
using TestWebAr.Scritps.Objects;

public class ObsSkyLog : DefaultSkyLog {
    ObsWebSocket obs;
    public ObsSkyLog(bool volumeSlider = false, bool buttons = false, bool hotKeyPanel = false) : base(volumeSlider, buttons, hotKeyPanel) {

    }

    protected override void Init() {
        obs = new ObsWebSocket("ws://192.168.0.103:4455");
        obs.Connect();
        base.Init();
    }

    protected override void InitSkyLogBrowsers()
    {
        for (int i = 0; i < 2; i++)
        {
            Quat lookDirection = Quat.LookDir(0, 0, 1);

            Vec3 windowPosition = new Vec3(0.65f * i, 0.36f, -0.5f);

            Vec2 scale = new Vec2(0.8f, 0.8f);

            if (i != 0)
            {
                Vec3 directionVector = Input.Head.position - windowPosition;
                directionVector.y = 0;
                directionVector = directionVector.Normalized;
                lookDirection = Quat.LookAt(Vec3.Forward, directionVector);

                windowPosition.z = windowPosition.z + 0.1f;
                scale = new Vec2(0.5f, 0.5f);
            }

            browserList.Add(
                    new Browser(
                        "https://skylog-demo.broadteam.eu/",
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

    public override void Update()
    {
        obs.Update();
        base.Update();
    }
}
