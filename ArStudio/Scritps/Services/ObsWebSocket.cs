using System;
using System.Collections.Generic;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;
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
    bool isRecording = false;
    List<SceneItemDetails> sceneDetails;
    Dictionary<SceneItemDetails, bool> sceneItemStates = new Dictionary<SceneItemDetails, bool>();

    public ObsWebSocket(string url, string password = "") {
        obs = new OBSWebsocket();

        Quat lookDirection = Quat.LookDir(0, 0, 1);

        Vec3 position = new Vec3(0.65f , 0, -0.3f);

        Vec3 directionVector = StereoKit.Input.Head.position - position;
        directionVector.y = 0;
        directionVector = directionVector.Normalized;
        lookDirection = Quat.LookAt(Vec3.Forward, directionVector);

        windowPosition = new Pose(0.65f , 0, -0.3f, lookDirection);

        this.url = url;
        this.password = password;

        obs.Connected += onConnect;
        obs.Disconnected += onDisconnect;
        obs.SceneItemRemoved += onSceneChanged;
        obs.SceneItemCreated += onSceneChanged;
        obs.RecordStateChanged += recordStatus;
    }

    private void recordStatus(object sender, EventArgs e) {
        isRecording = obs.GetRecordStatus().IsRecording;
    }

    private void onDisconnect(object sender, ObsDisconnectionInfo e)
    {
        update = false;
    }

    private void onConnect(object sender, EventArgs e)
    {
        scene = obs.GetSceneList().Scenes[0].Name;
        if (sceneDetails == null) {
            sceneDetails = new List<SceneItemDetails>();
            sceneDetails = obs.GetSceneItemList(scene);
        }
        foreach (var source in sceneDetails) {
            sceneItemStates.Add(source, obs.GetSourceActive(source.SourceName).VideoShowing);
        }

        update = true;
    }

    private void onSceneChanged(object sender, EventArgs e) {
        if (sceneDetails == null) {
            sceneDetails = new List<SceneItemDetails>();
        }
        sceneDetails = obs.GetSceneItemList(scene);
        sceneItemStates.Clear();
        foreach (var source in sceneDetails) {
            bool state = obs.GetSourceActive(source.SourceName).VideoShowing;
            sceneItemStates.Add(source, state);
        }
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

        UI.WindowBegin("Obs Remote Control", ref windowPosition);
        if (isRecording) {
            if (UI.Button("Stop Recording")) {
                obs.StopRecord();
            }
        }
        else {
            if (UI.Button("Start Recording")) {
                obs.StartRecord();
            }
        }

        UI.SameLine();
        if (UI.Button("New Source")) {
            Newtonsoft.Json.Linq.JObject settings = new Newtonsoft.Json.Linq.JObject();
            settings["monitor"] = 1;
            settings["capture_cursor"] = true;
            obs.CreateInput(scene, "Video", "monitor_capture", settings, true);
        }

        UI.NextLine();

        foreach (var source in sceneItemStates) {
            if (source.Value) {
                if (UI.Button("Hide " + source.Key.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.Key.ItemId, !source.Value);
                    sceneItemStates[source.Key] = !source.Value;
                }
            }
            else {
                if (UI.Button("Show " + source.Key.SourceName)) {
                    obs.SetSceneItemEnabled(scene, source.Key.ItemId, !source.Value);
                    sceneItemStates[source.Key] = !source.Value;
                }
            }
        }

        UI.WindowEnd();
        return;
    }
}
