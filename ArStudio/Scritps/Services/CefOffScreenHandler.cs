using CefSharp.OffScreen;
using CefSharp;
using System;

public class CefOffScreenDropdownHandler {
    private readonly ChromiumWebBrowser _browser;
    private bool _dropdownOpened = false;

    public CefOffScreenDropdownHandler(ChromiumWebBrowser browser) {
        _browser = browser;

        // Inject JavaScript to detect dropdown opening
        _browser.FrameLoadEnd += OnFrameLoadEnd;
        _browser.JavascriptMessageReceived += OnJavascriptMessageReceived;
    }

    private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
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
        }
    }

    private void OnJavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e) {
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

