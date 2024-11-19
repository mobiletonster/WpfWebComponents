using System.Windows;
using System.Windows.Controls;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for DumpMeter.xaml
/// </summary>
public partial class DumpMeter : UserControl
{
    public DumpMeter()
    {
        InitializeComponent();
        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("cat", "/DumpMeter/", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
        WebView.CoreWebView2.Navigate("http://cat/index.html");
    }

    private async void CoreWebView2_DOMContentLoaded(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs e)
    {
        OnMeterValueChanged(MeterValue);
    }

    private void WebView_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        var message = e.TryGetWebMessageAsString();
    }

    public static readonly DependencyProperty MeterValueProperty = DependencyProperty.Register(
    nameof(MeterValue),
    typeof(int),
    typeof(DumpMeter),
    new PropertyMetadata(0, MeterValueChanged)
);

    private static void MeterValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DumpMeter control)
        {
            control.OnMeterValueChanged((int)e.NewValue);
        }
    }

    private int _meterValue =0;

    public int MeterValue
    {
        get => (int)GetValue(MeterValueProperty);
        set => SetValue(MeterValueProperty, value);
    }

    // This method is called when MeterValue changes
    private async void OnMeterValueChanged(int newValue)
    {
        _meterValue = newValue;
        await WebView.EnsureCoreWebView2Async();
        string val = $"setMeterValue({newValue})";
        await WebView.CoreWebView2.ExecuteScriptAsync(val);
    }
}
