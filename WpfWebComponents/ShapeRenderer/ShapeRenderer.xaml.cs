using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWebComponents;
/// <summary>
/// Interaction logic for ShapeRenderer.xaml
/// </summary>
public partial class ShapeRenderer : UserControl
{
    public ShapeRenderer()
    {
        InitializeComponent();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        WebView.WebMessageReceived += WebView_WebMessageReceived;
        await WebView.EnsureCoreWebView2Async();
        WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
        WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("ShapeRenderer", "/ShapeRenderer/", CoreWebView2HostResourceAccessKind.Allow);
        WebView.CoreWebView2.Navigate("http://ShapeRenderer/index.html");
    }


    public async void DrawPoints(List<Point> points)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string? jsonPoints = JsonSerializer.Serialize(points, options);

        await WebView.CoreWebView2.ExecuteScriptAsync($"drawPoints('{jsonPoints}')");
    }
    private async void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        //List<Point> points = new();
        //points.Add(new(40, 40));
        //points.Add(new(150, 50));
        //points.Add(new(200, 150));
        //points.Add(new(100, 200));
        //points.Add(new(50, 150));
        //points.Add(new(40, 40));

        //string? jsonPoints = JsonSerializer.Serialize(points);
        //await WebView.CoreWebView2.ExecuteScriptAsync($"drawPoints('{jsonPoints}')");
    }

    private void WebView_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {

    }
}
