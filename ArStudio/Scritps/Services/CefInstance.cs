using System.IO;
using System;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

public static class CefInstance {
    public static async Task InitializeCef() {
        CefSettings settings = new CefSettings {
            //BrowserSubprocessPath = "E:\\Github\\ArStudio\\ArStudio\\bin\\x64\\Debug\\net8.0-windows\\runtimes\\win-x64\\native\\CefSharp.BrowserSubprocess.exe",
            UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.79 Safari/537.36",
            CachePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CefSharp\\Cache")
        };

        settings.WindowlessRenderingEnabled = true;
        settings.CefCommandLineArgs.Add("enable-media-stream", "1");
        settings.CefCommandLineArgs.Add("enable-widevine-cdm", "1");
        settings.CefCommandLineArgs.Add("ignore-certificate-errors", "1");
        settings.CefCommandLineArgs.Add("enable-gpu-rasterization", "1");
        settings.CefCommandLineArgs.Add("disable-gpu", "0");
        settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
        settings.CefCommandLineArgs.Add("enable-features", "NetworkService,NetworkServiceInProcess");
        settings.CefCommandLineArgs.Remove("mute-audio");

        await Cef.InitializeAsync(
            settings
        );
    }
}

