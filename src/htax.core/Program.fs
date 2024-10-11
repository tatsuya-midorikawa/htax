module Htax.Program

let inline debug (msg: string) =
  System.Diagnostics.Debug.WriteLine msg

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
    |> WebView2.beginInit
    |> WebView2.initializationCompleted (fun _ ->
        webview2
          |> WebView2.domcontentLoaded updateTitle
          |> WebView2.navigationCompleted updateTitle
      )
    |> WebView2.endInit

  form
    |> Form.suspendLayout
    |> Form.onLoad (fun _ -> task {
        do! webview2 |> WebView2.ensureCoreWebView2Async
        webview2
          |> WebView2.beginInit
          |> WebView2.setDock System.Windows.Forms.DockStyle.Fill
          |> WebView2.setSource "https://www.microsoft.com"
          |> WebView2.endInit
      })
    |> Form.add webview2
    |> Form.resumeLayout false

let inline parseArgs (args: string array) =
  if args.Length = 1 then Some args[0] else None
    |> Option.bind (fun arg ->
        if File.isHtax arg then Some (System.IO.Path.GetFullPath arg) else None)
    |> Option.map Htax.create
    |> Option.map Htax.copyToHiddenHtml

[<EntryPoint; System.STAThread>]
let main args =
  args
    |> parseArgs
    |> Option.map (fun html -> 
        initialize ()  
      
        use form = new System.Windows.Forms.Form()
        use wv2 = new Microsoft.Web.WebView2.WinForms.WebView2()
        initalizeComponents (form, wv2)
    
        form |> Form.run
    )
    |> Option.defaultValue -1