using StereoKit;
using System;

public class VolumeSlider {
    string name;
    Pose position;

    public float volume = 0;
    float dirtyVolume = 0;

    Action<float> onVolumeChange;

    public VolumeSlider(string name, Pose position) {
        this.name = name;
        this.position = position;
    }

    public void UpdateSlider() {
        UI.WindowBegin("Volume", ref position, new Vec2(0.2f, 0.05f));
        UI.HSlider("Volume Slide", ref volume, 0, 1, 0);
        if (dirtyVolume != volume)
        {
            onVolumeChange.Invoke(volume);
            dirtyVolume = volume;
        }
        UI.WindowEnd();
    }

    public void BindVolumeAction(Action<float> action)
    {
        onVolumeChange += action;
    }
}

