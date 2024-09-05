
using System;
using OBSWebsocketDotNet;
using StereoKit;

namespace ArStudio.Scritps.Services;

public class ObsWebSocket
{
    OBSWebsocket obs;
    Pose windowPosition;
    string scene;

    public ObsWebSocket() {
        obs = new OBSWebsocket();
        windowPosition = new Pose(0, 0, 0.2f);
    }

    public void Connect() {
    }

    public void Update() {
        if (!obs.IsConnected) {
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
                if (UI.Button("UnMute " + source.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.ItemId, true);
                }
            }
            else {
                if (UI.Button("Mute " + source.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.ItemId, false);
                }
            }
        }

        UI.WindowEnd();
    }
}
