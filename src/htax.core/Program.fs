module Htax.Program

let inline initialize () =
  System.Windows.Forms.Application.EnableVisualStyles()
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
  System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware) |> ignore

let inline initalizeComponents (
  form: System.Windows.Forms.Form, 
  webview2: Microsoft.Web.WebView2.WinForms.WebView2
) =
  let updateTitle _ =
    form |> Form.setTitle webview2.CoreWebView2.DocumentTitle

  webview2
    |> WebView2.initializationCompleted (fun _ ->
        webview2
          |> WebView2.domcontentLoaded updateTitle
          |> WebView2.navigationCompleted updateTitle
      )
    |> ignore

  form
    |> Form.onLoad (fun _ -> task {
        do! webview2 |> WebView2.ensureCoreWebView2Async
        return webview2 
          |> WebView2.setDock System.Windows.Forms.DockStyle.Fill
          |> WebView2.setSource "https://www.microsoft.com"
      })
    |> Form.add webview2
    |> ignore

[<EntryPoint; System.STAThread>]
let main args =
  initialize ()  
  
  use form = new System.Windows.Forms.Form()
  use wv2 = new Microsoft.Web.WebView2.WinForms.WebView2()
  initalizeComponents (form, wv2)

  form |> Form.run