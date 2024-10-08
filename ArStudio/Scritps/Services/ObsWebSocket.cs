using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
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

    delegate void UpdateDelegate();
    event UpdateDelegate updateHandler;

    WindowCreator creator = new WindowCreator();

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

        updateHandler = UpdateObsHierarchy;
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
        foreach (var inputKind in obs.GetInputKindList()) {
            Console.WriteLine(inputKind);
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

    private void CreateSource(string name, string inputKind, JObject settings)
    {
        obs.CreateInput(scene, UniqueName(name), inputKind, settings, true);
    }

    private string UniqueName(string name, int index = 0) {
        if (sceneDetails.Any(obj => obj.SourceName == name + index)) {
            return UniqueName(name, ++index);
        }
        return name + index;
    }

    public void UpdateObsHierarchy() {
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
            updateHandler = UpdateNewSourceWindow;
            return;
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
    }

    string dirtyKey;
    string selectedKey;
    public void UpdateNewSourceWindow() {
        UI.WindowBegin("NewSource", ref windowPosition);
        foreach (var sourceKind in InputKind.sourceArray) {
            bool currentValue = creator.sourceStates[sourceKind];
            if (UI.Toggle(sourceKind, ref currentValue)) {
                currentValue = true;
                selectedKey = sourceKind;
                if (dirtyKey != null && dirtyKey != selectedKey) {
                    creator.sourceStates[dirtyKey] = false;
                }
                creator.sourceStates[selectedKey] = true;
                dirtyKey = sourceKind;
            }
        }
        if (UI.Button("Confirm")) {
            CreateSource(selectedKey, selectedKey, creator.sourceSettings[selectedKey]);
            updateHandler = UpdateObsHierarchy;
        }
        UI.WindowEnd();
    }

    public void Update() {
        if (!update) {
            return;
        }

        updateHandler.Invoke();
    }
}

public static class InputKind {
    public const string MonitorCapture = "monitor_capture";
    public const string ColorSource = "color_source_v3";

    public static string[] sourceArray = { MonitorCapture, ColorSource };
}

public struct WindowCreator {
    public Dictionary<string, bool> sourceStates = new Dictionary<string, bool>();
    public Dictionary<string, JObject> sourceSettings = new Dictionary<string, JObject>();

    public string selectedSourceType;

    public WindowCreator() {
        FillStates();
        FillSettings();
    }

    void FillStates() {
        foreach (string sourceKind in InputKind.sourceArray) {
            sourceStates[sourceKind] = false;
        }
    }

    void FillSettings() {
        foreach (string sourceKind in InputKind.sourceArray) {
            if (sourceKind == InputKind.MonitorCapture) {
                sourceSettings[sourceKind] = new JObject {
                    { "monitor", 1 },
                    { "capture_cursor", true }
                };
            }

            else if (sourceKind == InputKind.ColorSource) {
                sourceSettings[sourceKind] = new JObject {
                    { "color", "0xFF0000FF" },
                    { "width", 1920 },
                    { "height", 1080 }
                };
            }
        }
    }
}
