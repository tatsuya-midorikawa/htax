module htax.prgram

let initialize () =
  System.Windows.Forms.Application.EnableVisualStyles()
  System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
  System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware) |> ignore


[<EntryPoint; System.STAThread>]
let main args =
  initialize()  
  
  use form = new System.Windows.Forms.Form()
  use wv2 = new Microsoft.Web.WebView2.WinForms.WebView2()
  wv2.Dock <- System.Windows.Forms.DockStyle.Fill
  form.Load.Add(fun _ -> wv2.Source <- new System.Uri("https://www.google.com"))
  form.Controls.Add(wv2)
  System.Windows.Forms.Application.Run form
  
  0