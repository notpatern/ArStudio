using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

public static class CefInstance {
    public static async Task InitializeCef() {
        CefSettings settings = new CefSettings
        {
            BrowserSubprocessPath = "E:\\Github\\ArStudio\\ArStudio\\bin\\x64\\Debug\\net8.0-windows\\runtimes\\win-x64\\native\\CefSharp.BrowserSubprocess.exe",
            CachePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CefSharp\\Cache"
            )
        };

        settings.CefCommandLineArgs.Add("enable-media-stream", "1");
        settings.CefCommandLineArgs.Add("enable-widevine-cdm", "1");
        settings.CefCommandLineArgs.Add("ignore-certificate-errors", "1");
        settings.CefCommandLineArgs.Add("enable-gpu-rasterization", "1");
        settings.CefCommandLineArgs.Add("disable-gpu", "0"); 

        settings.CefCommandLineArgs.Add("enable-media-stream", "1");
        settings.CefCommandLineArgs.Add("enable-features", "NetworkService,NetworkServiceInProcess");

        settings.CefCommandLineArgs.Remove("mute-audio");

        await Cef.InitializeAsync(
            settings
        );
    }
}

