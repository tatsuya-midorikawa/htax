using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htax {
  internal static class Html {
    internal static class document {

      public static async ValueTask<string[]> querySelectorAll(Microsoft.Web.WebView2.WinForms.WebView2 wv2, string selector) {
        var script = $@"
          (function() {{
            var icons = document.querySelectorAll('{selector}');
            var iconUrls = [];
            icons.forEach(icon => iconUrls.push(icon.href));
            return iconUrls;
          }})();
        ";

        var result = await wv2.CoreWebView2.ExecuteScriptAsync(script);

        // 結果を処理
        // 結果はJSON形式の文字列として返されるので、パースが必要
        return System.Text.Json.JsonSerializer.Deserialize<string[]>(result);
      }
    }
  }
}
