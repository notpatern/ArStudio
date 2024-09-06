
using StereoKit;

public class PlayerHotKeys {
    Pose windowPosition;

    public PlayerHotKeys(Pose windowPosition) {
        this.windowPosition = windowPosition;
    }

    public void Update() {
        UI.WindowBegin("PlayerHotKeys", ref windowPosition, windowType: UIWin.Body);
        if (UI.Button("Bar")) {
            SkyLogEvents.F5.Invoke();
        }

        UI.SameLine();

        if (UI.Button("60s")) {
            SkyLogEvents.F4.Invoke();
        }

        UI.SameLine();

        if (UI.Button("10s")) {
            SkyLogEvents.F3.Invoke();
        }

        UI.SameLine();

        if (UI.Button("1s")) {
            SkyLogEvents.F2.Invoke();
        }

        UI.SameLine();

        if (UI.Button("1f")) {
            SkyLogEvents.F1.Invoke();
        }
        UI.WindowEnd();
    }
}
