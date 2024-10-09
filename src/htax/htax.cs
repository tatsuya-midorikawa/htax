using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace htax {
  public partial class htax : Form {
    private readonly string _htaxfile;
    private static readonly System.Net.Http.HttpClient http = new System.Net.Http.HttpClient();

    public htax(string htaxfile) {
      _htaxfile = htaxfile;
      InitializeComponent();
      this.Load += OnLoad;
      wv2.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
    }

    private async void OnLoad(object sender, EventArgs e) {
      await wv2.EnsureCoreWebView2Async();
      wv2.Source = new Uri(_htaxfile);
    }

    private void OnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
      wv2.CoreWebView2.DOMContentLoaded += OnDOMContentLoaded;
    }

    private async void OnDOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e) {
      this.Text = wv2.CoreWebView2.DocumentTitle;
      var icons = await Html.document.querySelectorAll(wv2, "link[rel~=\"icon\"]");
      var icon = (icons.FirstOrDefault(s => s.EndsWith("favicon.ico")) ?? icons.FirstOrDefault(s => s.EndsWith(".ico"))) ?? "";
      if (!string.IsNullOrEmpty(icon)) {
        var bytes = await http.GetByteArrayAsync(icon);
        using (var stream = new MemoryStream(bytes)) {
          stream.Seek(0, System.IO.SeekOrigin.Begin);
          this.Icon = icon != null ? new Icon(stream) : null;
        }
      }
    }
  }
}
