
using StereoKit;

namespace TestWebAr.Scritps
{
    public class ButtonWindow
    {
        string name;
        Pose position;
        public ButtonWindow(string name, Pose position) { 
            this.name = name;
            this.position = position;
        }
        
        public void UpdateWindow()
        {
            UI.WindowBegin("Buttons", ref position);
            UI.Button("Button 01");
            UI.SameLine();
            UI.Button("Button 02");

            UI.Button("Button 03");
            UI.SameLine();
            UI.Button("Button 04");

            UI.Button("Button 05");
            UI.SameLine();
            UI.Button("Button 06");

            UI.Button("Button 07");
            UI.SameLine();
            UI.Button("Button 08");

            UI.Button("Button 09");
            UI.SameLine();
            UI.Button("Button 10");

            UI.Button("Button 11");
            UI.SameLine();
            UI.Button("Button 12");

            UI.Button("Button 13");
            UI.SameLine();
            UI.Button("Button 14");

            UI.Button("Button 15");
            UI.SameLine();
            UI.Button("Button 16");

            UI.WindowEnd();
        }
    }
}
