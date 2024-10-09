module Htax.Program

module Form =
  let inline update_title (form: System.Windows.Forms.Form, title: string) =
    form.Text <- title

let inline initialize () =
  System.Windows.Forms.Application.EnableVisualStyles()
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
  System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware) |> ignore

let inline initalize_components (
  form: System.Windows.Forms.Form, 
  wv2: Microsoft.Web.WebView2.WinForms.WebView2
) =
  form.Load.Add(fun _ ->
    task {
      do! wv2.EnsureCoreWebView2Async()
      wv2.Source <- new System.Uri("https://www.microsoft.com")
    } |> ignore)

  wv2.CoreWebView2InitializationCompleted.Add(fun _ ->
    wv2.CoreWebView2.DOMContentLoaded.Add(fun _ ->
      form.Text <- wv2.CoreWebView2.DocumentTitle
      
    )
    
    wv2.NavigationCompleted.Add(fun _ ->
      form.Text <- wv2.CoreWebView2.DocumentTitle
    )
  )
  wv2.Dock <- System.Windows.Forms.DockStyle.Fill
  form.Controls.Add wv2

[<EntryPoint; System.STAThread>]
let main args =
  initialize ()  
  
  use form = new System.Windows.Forms.Form()
  use wv2 = new Microsoft.Web.WebView2.WinForms.WebView2()
  initalize_components (form, wv2)

  System.Windows.Forms.Application.Run form
  
  0