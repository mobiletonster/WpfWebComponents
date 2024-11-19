using System.Text.Json;
using System.Windows.Controls;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for DumpGauge.xaml
/// </summary>
public partial class DumpGauge : UserControl
{
    public DumpGauge()
    {
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("cat", "/DumpGauge/", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
        WebView.CoreWebView2.Navigate("http://cat/index.html");
    }

    private async void CoreWebView2_DOMContentLoaded(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e)
    {
        //var result = await WebView.CoreWebView2.ExecuteScriptAsync($"setButtonText('{ButtonText}')");
    }

    private void WebView_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        var message = e.TryGetWebMessageAsString();
        var payload = JsonSerializer.Deserialize<Payload>(message);
        if (payload?.Action=="dumpStateChanged")
        {
            var dumpState = payload.DumpState;
        }
    }
}

public record Payload(string Action, string DumpState);