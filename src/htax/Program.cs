using System;
using System.Windows.Forms;
using System.IO;

//https://qiita.com/tnakagawa/items/e049fa1bdd8829315a90

namespace htax {
  internal static class Htax {
    public static bool IsHtax(string src) {
      return Path.GetExtension(src) == ".htax";
    }

    public static string ConvertToHiiddenHtml(string src) {
      var htax = Path.GetFullPath(src);
      var html = Path.ChangeExtension(htax, ".html");
      if (File.Exists(html)) {
        File.Delete(html);
      }
      File.Copy(htax, html);
      var attr = File.GetAttributes(html);
      attr |= FileAttributes.Hidden;
      File.SetAttributes(html, attr);
      return html;
    }
  }


  internal static class Program {
    [STAThread]
    static void Main(string[] args) {
#if DEBUG
      Application.Run(new htax(""));
#else
      var path = 0 < args.Length ? Path.GetFullPath(args[0]) : "";
      if (Htax.IsHtax(path) && File.Exists(path)) {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new htax(Htax.ConvertToHiiddenHtml(path)));
      } else {
        MessageBox.Show("This file is not HTAX.");
      }
#endif
    }
  }
}
