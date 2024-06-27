using StereoKit;

namespace TestWebAr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: open multiple WINDOWS of the same browser
            App app = new App();
            app.Init();
            SK.Run(() =>
            {
                app.Update();
            });
        }
    }
}
