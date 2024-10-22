using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;

namespace WpfWebComponents.Common;
public static class WebHelpers
{
    public static string GetHtmlContent(bool isDesignMode = false)
    {
        string html = string.Empty;
        Uri uri = new Uri("SortableListView/index.html", UriKind.Relative);

        using (var stream = Application.GetResourceStream(uri).Stream)
        {
            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    // Now you can use the 'htmlContent' in your application
                }
            }
        }

        return html;
    }

    public static Stream GetContentStream(string url)
    {
        var absoluteUri = new Uri(url, UriKind.Absolute);
        Uri uri = new Uri(absoluteUri.PathAndQuery, UriKind.Relative);
        return Application.GetResourceStream(uri).Stream;
    }

    public static CoreWebView2WebResourceResponse ProcessWebResourceResponse(object? sender, CoreWebView2WebResourceRequestedEventArgs args)
    {
        var coreWebView2 = sender as CoreWebView2;
        if (coreWebView2 != null)
        {
            try
            {
                if (args.Request.Uri.Contains("favicon.ico"))
                {
                    return coreWebView2.Environment.CreateWebResourceResponse(null, 200, "Ok", "");
                }

                var extension = $".{args.Request.Uri.Split(".").Last()}";
                string headers ="text/plain";
                if(ContentType.TryGetValue(extension, out string? contentType)){
                    headers = $"Content-Type:{contentType}";
                }
                Stream stream = WebHelpers.GetContentStream(args.Request.Uri);
                return coreWebView2.Environment.CreateWebResourceResponse(stream, 200, "OK", headers);
            }
            catch (Exception)
            {
                return coreWebView2.Environment.CreateWebResourceResponse(
                                                        null, 404, "Not found", "");
            }
        }
        throw new Exception("CoreWebView2 is null");
    }

    public static Dictionary<string, string> ContentType => new()
    {
        [".html"] = "text/html",
        [".ttf"] = "text/html",
        [".jpg"] = "image/jpeg",
        [".png"] = "image/png",
        [".css"] = "text/css",
        [".js"] = "application/javascript"
    };

}