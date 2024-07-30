using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

public static class CefInstance {
    public static async Task InitializeCef() {
        CefSettings settings = new CefSettings
        {
            UserAgent =
                "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.79 Mobile Safari/537.36",
            CachePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CefSharp\\Cache"
            )
        };

        settings.CefCommandLineArgs.Remove("mute-audio");
        settings.CefCommandLineArgs.Add("1");

        await Cef.InitializeAsync(
            settings
        );
    }
}

