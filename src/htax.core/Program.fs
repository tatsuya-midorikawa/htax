module Htax.Program

let inline debug (msg: string) =
  System.Diagnostics.Debug.WriteLine msg

let inline initialize () =
  System.Windows.Forms.Application.EnableVisualStyles()
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
#if NET8_0_OR_GREATER
  System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware) |> ignore
#endif

let initalizeComponents (
  form: System.Windows.Forms.Form, 
  webview2: Microsoft.Web.WebView2.WinForms.WebView2,
  html: Html.Html
) =
  let updateTitle args =
    task {
      let! title = WebView2.getTitle webview2
      form |> Form.setTitle title |> ignore
    }

  let updateIcon args =
    task {
      let! settings = document.getHtaxSettings webview2
      match settings with
      | { HtaxSettings.icon = Some icon; } ->
          // HTAX ファイルはローカルに配置されている前提なので、相対パスでアイコンを指定している場合は、すべてローカルにあるものとして扱う
          // ただし、アイコンが URL で指定されている場合は、その URL からアイコンを取得する
          if icon.StartsWith "https://" || icon.StartsWith "http://" then
            let! raw = http.GetByteArrayAsync(icon)
            use stream = new System.IO.MemoryStream(raw)
            form.Icon <- new System.Drawing.Icon(stream)
          else
            form.Icon <- new System.Drawing.Icon(icon)
      | _ -> ()
    }

  let onDomContentLoaded args =
    task {
      do! updateTitle args
      do! updateIcon args
    }

  webview2
    |> WebView2.beginInit
    |> WebView2.initializationCompleted (fun e ->
        debug $"Initialization completed: {e.IsSuccess}"
        if not e.IsSuccess then debug e.InitializationException.Message
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