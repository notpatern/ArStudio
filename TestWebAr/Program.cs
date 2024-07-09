using StereoKit;
namespace TestWebAr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CefInstance.InitializeCef();
            App app = new App();
            app.Init();
            SK.Run(() =>
            {
                app.Update();
            });
        }
    }
}
