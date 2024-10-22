using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for SimpleButton.xaml
/// </summary>
public partial class SimpleButton : UserControl
{
    public event EventHandler? Clicked;
    public SimpleButton()
    {
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("SimpleButton", "/SimpleButton/", CoreWebView2HostResourceAccessKind.Allow);
        WebView.CoreWebView2.Navigate("http://SimpleButton/index.html");
    }

    private string LoadHtmlString()
    {
        string html = @"<html><body><button>Click Me!</button></body></html>";
        return html;
    }

    private string LoadHtmlFromFile()
    {
        string html = string.Empty;
        string? exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string filePath = Path.Combine(exePath, "SimpleButton", "index.html");

        if (File.Exists(filePath))
        {
            html = File.ReadAllText(filePath);
        }
        return html;
    }

    private async void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        await WebView.CoreWebView2.ExecuteScriptAsync($"setButtonText('{ButtonText}')");
    }

    private void WebView_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        var clicked = e.TryGetWebMessageAsString();
        if (clicked == "clicked")
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
    nameof(ButtonText),
    typeof(string),
    typeof(SimpleButton),
    new PropertyMetadata(string.Empty, ButtonTextValueChanged)
);

    private static void ButtonTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SimpleButton control)
        {
            control.ButtonText = e.NewValue.ToString();
        }
    }

    private string _buttonText=string.Empty;
    public string ButtonText
    {
        get => _buttonText;
        set => SetButtonText(_buttonText = value);
    }

    private async void SetButtonText(string buttonText)
    {
        await WebView.EnsureCoreWebView2Async();
        await WebView.CoreWebView2.ExecuteScriptAsync($"setButtonText('{buttonText}')");
    }

}
