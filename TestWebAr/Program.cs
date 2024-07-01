using StereoKit;
namespace TestWebAr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
            app.Init();
            SK.Run(() =>
            {
                app.Update();
            });
        }
    }
}
