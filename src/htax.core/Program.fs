module Htax.Program

let inline debug (msg: string) =
  System.Diagnostics.Debug.WriteLine msg

let inline initialize () =
  System.Windows.Forms.Application.EnableVisualStyles()
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
  System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware) |> ignore

let initalizeComponents (
  form: System.Windows.Forms.Form, 
  webview2: Microsoft.Web.WebView2.WinForms.WebView2,
  html: Html.Html
) =
  let updateTitle args =
    form |> Form.setTitle webview2.CoreWebView2.DocumentTitle

  let onDomContentLoaded args =
    updateTitle args |> ignore
    task {
      let! settings = document.getHtaxSettings webview2
      debug (sprintf "HtaxSettings: %A" settings)
    }
    |> ignore

  webview2
    |> WebView2.beginInit
    |> WebView2.initializationCompleted (fun _ ->
        webview2
          |> WebView2.domcontentLoaded onDomContentLoaded
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
          |> WebView2.setSource (Html.path html)
          |> WebView2.endInit
      })
    |> Form.add webview2
    |> Form.resumeLayout false

let inline parseArgs (args: string array) =
  if args.Length = 1 then Some args[0] else None
    |> Option.bind (fun arg ->
        if File.exists arg && File.isHtax arg then Some (System.IO.Path.GetFullPath arg) else None)
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
        initalizeComponents (form, wv2, html)
    
        form |> Form.run
    )
    |> Option.defaultValue -1