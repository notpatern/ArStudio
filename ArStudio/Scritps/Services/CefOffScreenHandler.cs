using CefSharp.OffScreen;
using CefSharp;
using System;

public class CefOffScreenDropdownHandler {
    private readonly ChromiumWebBrowser _browser;
    private bool _dropdownOpened = false;

    // remnant of a test i was doing and might need to resume depending on the web dev team's work
    public CefOffScreenDropdownHandler(ChromiumWebBrowser browser) {
        _browser = browser;

        _browser.FrameLoadEnd += OnFrameLoadEnd;
        _browser.JavascriptMessageReceived += OnJavascriptMessageReceived;
    }

    private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
        Console.WriteLine("JavaScriptLoaded");
        if (e.Frame.IsMain) {
            const string script = @"
                (function() {
                    var dropdowns = document.querySelectorAll('select');
                    dropdowns.forEach(function(dropdown) {
                        dropdown.addEventListener('mousedown', function() {
                            CefSharp.PostMessage('dropdown-opened');
                        });
                    });

                    document.addEventListener('click', function(event) {
                        if (event.target.tagName === 'SELECT') {
                            CefSharp.PostMessage('dropdown-opened');
                        }
                    });
                })();
            ";
            e.Frame.ExecuteJavaScriptAsync(script);
            Console.WriteLine("Scipt Executed");
        }
    }

    private void OnJavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e) {
        Console.WriteLine(e.Message.ToString());
        if (e.Message.ToString() == "dropdown-opened") {
            _dropdownOpened = true;
        }
    }

    public bool IsDropdownOpened() {
        bool isOpened = _dropdownOpened;
        _dropdownOpened = false; // Reset the flag
        return isOpened;
    }
}

