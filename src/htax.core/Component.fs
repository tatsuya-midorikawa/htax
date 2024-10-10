namespace Htax

module Form =
  let inline onLoad ([<InlineIfLambda>] callback: System.EventArgs -> 'T) (form: System.Windows.Forms.Form) =
    form.Load.Add (callback >> ignore); form

  let inline add (control: System.Windows.Forms.Control) (form: System.Windows.Forms.Form) =
    form.Controls.Add control; form

  let inline setTitle (title: string) (form: System.Windows.Forms.Form) =
    form.Text <- title; form

module WebView2 =
  let inline ensureCoreWebView2Async (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.EnsureCoreWebView2Async()

  let inline setSource (uri: string) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    match System.Uri.TryCreate(uri, System.UriKind.Absolute) with
      | true, uri -> wv2.Source <- uri
      | _ -> failwith "Invalid URI"
    wv2

  let inline setDock (dock: System.Windows.Forms.DockStyle) (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    wv2.Dock <- dock
    wv2