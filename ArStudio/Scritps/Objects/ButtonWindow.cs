
using StereoKit;

namespace TestWebAr.Scritps.Objects
{
    public class ButtonWindow
    {
        string name;
        Pose position;
        public ButtonWindow(string name, Pose position)
        {
            this.name = name;
            this.position = position;
        }

        public void UpdateWindow()
        {
            UI.WindowBegin("Buttons", ref position);
            if (UI.Button("Play")) {
                SkyLogEvents.Play.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Pause")) {
                SkyLogEvents.Pause.Invoke();
            }

            if (UI.Button("Cancel Log")) {
                SkyLogEvents.CancelLog.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Clear Markers")) {
                SkyLogEvents.ClearMarkers.Invoke();
            }

            if (UI.Button("Copy Log")) {
                SkyLogEvents.CopyLog.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Paste Log")) {
                SkyLogEvents.PasteLog.Invoke();
            }

            if (UI.Button("Focus/Unfocus text box")) {
                SkyLogEvents.Tab.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Copy Time Code")) {
                SkyLogEvents.CopyPlayerTimeCode.Invoke();
            }

            if (UI.Button("Paste Time Code")) {
                SkyLogEvents.PastePlayerTimeCode.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Change log TCIN")) {
                SkyLogEvents.ChangeLogTCIN.Invoke();
            }

            if (UI.Button("Change log TCOUT")) {
                SkyLogEvents.ChangeLogTCOUT.Invoke();
            }
            UI.SameLine();
            if (UI.Button("Back to Live")) {
                SkyLogEvents.BackToLive.Invoke();
            }
            UI.WindowEnd();
        }
    }
}
