using Microsoft.Web.WebView2.Core;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WpfWebComponents.Common;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for SortableListViewControl.xaml
/// </summary>
public partial class SortableListViewControl : UserControl
{
    public SortableListViewControl()
    {
        InitializeComponent();
        InitializeAsync("http://controls.ui/SortableListView/index.html");
    }

    private async void InitializeAsync(string url)
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        WebView.ContentLoading += WebView_ContentLoading;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        WebView.CoreWebView2.AddWebResourceRequestedFilter("*/controls.ui/*", CoreWebView2WebResourceContext.All);

        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            WebView.Source = new Uri(url);
        }
    }



    private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs args)
    {
        args.Response = WebHelpers.ProcessWebResourceResponse(sender, args);
    }

    private void WebView_ContentLoading(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
    {
        // throw new NotImplementedException();
    }

    private void WebView_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        // throw new NotImplementedException();
        var item = e;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        WebView.WebMessageReceived -= WebView_WebMessageReceived;
        WebView.ContentLoading -= WebView_ContentLoading;
        WebView.CoreWebView2.WebResourceRequested -= CoreWebView2_WebResourceRequested;
    }
}
