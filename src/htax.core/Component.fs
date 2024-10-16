﻿namespace Htax

module Form =
  let inline suspendLayout (form: System.Windows.Forms.Form) =
    form.SuspendLayout(); form

  let inline resumeLayout (perform: bool) (form: System.Windows.Forms.Form) =
    form.ResumeLayout(perform)

  let inline run (form: System.Windows.Forms.Form) =
    try
      System.Windows.Forms.Application.Run form; 0 
    with
      _ -> -1

  let inline onLoad ([<InlineIfLambda>] callback: System.EventArgs -> 'T) (form: System.Windows.Forms.Form) =
    form.Load.Add (callback >> ignore); form

  let inline add (control: System.Windows.Forms.Control) (form: System.Windows.Forms.Form) =
    form.Controls.Add control; form

  let inline setTitle (title: string) (form: System.Windows.Forms.Form) =
    form.Text <- title; form

module WebView2 =
  let inline beginInit (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    (wv2 :> System.ComponentModel.ISupportInitialize).BeginInit(); wv2

  let inline endInit (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    (wv2 :> System.ComponentModel.ISupportInitialize).EndInit()

  let inline initializationCompleted ([<InlineIfLambda>] callback: Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs -> 'T) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.CoreWebView2InitializationCompleted.Add (callback >> ignore); wv2

  let inline domcontentLoaded ([<InlineIfLambda>] callback: Microsoft.Web.WebView2.Core.CoreWebView2DOMContentLoadedEventArgs -> 'T) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.CoreWebView2.DOMContentLoaded.Add (callback >> ignore); wv2

  let inline navigationCompleted ([<InlineIfLambda>] callback: Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs -> 'T) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.NavigationCompleted.Add (callback >> ignore); wv2

  let inline ensureCoreWebView2Async (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.EnsureCoreWebView2Async(null)

  let inline setSource (uri: string) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    match System.Uri.TryCreate(uri, System.UriKind.Absolute) with
      | true, uri -> wv2.Source <- uri
      | _ -> failwith "Invalid URI"
    wv2

  let inline setDock (dock: System.Windows.Forms.DockStyle) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.Dock <- dock; wv2

  let inline getTitle (webview2: Microsoft.Web.WebView2.WinForms.WebView2) =
    task {
      let! settings = document.getHtaxSettings webview2
      return match settings with
              | { HtaxSettings.applicationname = Some name; } -> name
              | _ -> webview2.CoreWebView2.DocumentTitle
    }