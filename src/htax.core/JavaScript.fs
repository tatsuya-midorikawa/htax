namespace Htax

[<Struct>]
type HtaxSettings = {
  id: string option
  applicationname: string option
  border: string option
  borderstyle: string option
  caption: string option
  contextmenu: string option
  icon: string option
  innerborder: string option
  maximizebutton: string option
  minimizebutton: string option
  navigable: string option
  scroll: string option
  scrollflat: string option
  selection: string option
  showintaskbar: string option
  singleinstance: string option
  sysmenu: string option
  version: string option
  windowstate: string option
}
 with
  static member empty = {
    id             = None; applicationname = None; border      = None; borderstyle    = None; caption        = None;
    contextmenu    = None; icon            = None; innerborder = None; maximizebutton = None; minimizebutton = None;
    navigable      = None; scroll          = None; scrollflat  = None; selection      = None; showintaskbar  = None;
    singleinstance = None; sysmenu         = None; version     = None; windowstate    = None;
  }
  static member val cache = HtaxSettings.empty with get, set

module document =
  let inline getHtaxSettings (wv2: Microsoft.Web.WebView2.WinForms.WebView2) =
    let script = $"""
      (() => {{
        let settings = document.getElementsByTagName('hta:application')[0];
        let attrs = settings.getAttributeNames();
        let ret = {{}};
        for (let attr of attrs) {{
          ret[attr] = settings.getAttribute(attr);
        }}
        return ret;
      }})()
    """

    task {
      let! result = wv2.ExecuteScriptAsync(script)
      if result <> null && result <> "null" then
        HtaxSettings.cache <- System.Text.Json.JsonSerializer.Deserialize<HtaxSettings>(result)
      return HtaxSettings.cache
    }