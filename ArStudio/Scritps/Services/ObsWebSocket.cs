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

    public ObsWebSocket(string url, string password = "") {
        obs = new OBSWebsocket();
        windowPosition = new Pose(0, 0, -0.2f);

        this.url = url;
        this.password = password;

        obs.Connected += onConnect;
        obs.Disconnected += onDisconnect;
    }

    private void onDisconnect(object sender, ObsDisconnectionInfo e)
    {
        Console.WriteLine("disconnected");
    }

    private void onConnect(object sender, EventArgs e)
    {
        Console.WriteLine("connected");
    }

    public void Connect() {
        obs.ConnectAsync(url, password);
    }

    public void Update() {
        if (!obs.IsConnected) {
            return;
        }

        if (scene == null) {
            scene = obs.GetSceneList().Scenes[0].Name.ToString();

            Console.WriteLine("test0");
        }

        Console.WriteLine("test1");

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

        Console.WriteLine("test2");

        UI.NextLine();

        Console.WriteLine("test3");

        foreach (var source in obs.GetSceneItemList(scene)) {
            if (!obs.GetSourceActive(source.SourceName).VideoShowing) {
                if (UI.Button("UnMute " + source.SourceName)) {
                    obs.SetSceneItemEnabled("Scene", source.ItemId, true);
                }
            }
            else {
                if (UI.Button("Mute " + source.SourceName)) {
                    obs.SetSceneItemEnabled("Scene", source.ItemId, false);
                }
            }
        }

        Console.WriteLine("test4");

        UI.WindowEnd();
        return;
    }
}
