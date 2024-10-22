using Microsoft.Web.WebView2.Core;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfWebComponents.Common;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for SimpleButton.xaml
/// </summary>
public partial class FancyButton : UserControl
{
    public event EventHandler? Clicked;
    public FancyButton()
    {
        InitializeComponent();
        InitializeAsync("http://controls.ui/FancyButton/index.html");
    }

    private async void InitializeAsync(string url)
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.AddWebResourceRequestedFilter("*/controls.ui/*", CoreWebView2WebResourceContext.All);

        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            WebView.Source = new Uri(url);
        }
    }

    private async void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        await WebView.CoreWebView2.ExecuteScriptAsync($"setButtonText('{ButtonText}')");
    }

    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
"ButtonText",
    typeof(string),
    typeof(FancyButton), // Use the actual registering type (e.g., Button)
    new PropertyMetadata("", new PropertyChangedCallback(ButtonTextValueChanged))
    );

    private static void ButtonTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as FancyButton;
        if (control != null)
            control.ButtonText = e.NewValue.ToString();
    }

    private string _buttonText=string.Empty;
    public string ButtonText
    {
        get { return _buttonText; }
        set
        {
            _buttonText = value;
            SetButtonText(value);
        }
    }

    private async void SetButtonText(string buttonText)
    {
        await WebView.EnsureCoreWebView2Async();
        await WebView.CoreWebView2.ExecuteScriptAsync($"setButtonText('{ButtonText}')");
    }
    private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs args)
    {
        args.Response = WebHelpers.ProcessWebResourceResponse(sender, args);
    }


    private void WebView_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        var clicked = e.TryGetWebMessageAsString();
        if (clicked == "clicked")
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }


    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        WebView.WebMessageReceived -= WebView_WebMessageReceived;
        WebView.CoreWebView2.DOMContentLoaded -= CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.WebResourceRequested -= CoreWebView2_WebResourceRequested;
    }
}
