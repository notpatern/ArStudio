using System;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using StereoKit;

namespace ArStudio.Scritps.Services;

public class ObsWebSocket
{
    OBSWebsocket obs;
    Pose windowPosition;
    string scene;
    string url;
    string password;
    bool update = false;

    public ObsWebSocket(string url, string password = "") {
        obs = new OBSWebsocket();

        Quat lookDirection = Quat.LookDir(0, 0, 1);

        Vec3 position = new Vec3(0.65f , 0, -0.3f);

        Vec3 directionVector = Input.Head.position - position;
        directionVector.y = 0;
        directionVector = directionVector.Normalized;
        lookDirection = Quat.LookAt(Vec3.Forward, directionVector);

        windowPosition = new Pose(0.65f , 0, -0.3f, lookDirection);

        this.url = url;
        this.password = password;

        obs.Connected += onConnect;
        obs.Disconnected += onDisconnect;
    }

    private void onDisconnect(object sender, ObsDisconnectionInfo e)
    {
        update = false;
    }

    private void onConnect(object sender, EventArgs e)
    {
        update = true;
    }

    public void Connect() {
        if (obs == null) {
            return;
        }
        obs.ConnectAsync(url, password);
    }

    public void Update() {
        if (!update) {
            return;
        }

        if (scene == null) {
            scene = obs.GetSceneList().Scenes[0].Name.ToString();

        }

        UI.WindowBegin("Obs Remote Control", ref windowPosition);
        if (obs.GetRecordStatus().IsRecording) {
            if (UI.Button("Stop Recording")) {
                obs.StopRecord();
            }
        }
        else if (!obs.GetRecordStatus().IsRecording) {
            if (UI.Button("Start Recording")) {
                obs.StartRecord();
            }
        }

        UI.NextLine();

        foreach (var source in obs.GetSceneItemList(scene)) {
            if (!obs.GetSourceActive(source.SourceName).VideoShowing) {
                if (UI.Button("Hide " + source.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.ItemId, true);
                }
            }
            else {
                if (UI.Button("Show " + source.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.ItemId, false);
                }
            }
        }

        UI.WindowEnd();
        return;
    }
}
