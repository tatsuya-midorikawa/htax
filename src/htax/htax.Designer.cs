namespace htax {
  partial class htax {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.wv2 = new Microsoft.Web.WebView2.WinForms.WebView2();
      ((System.ComponentModel.ISupportInitialize)(this.wv2)).BeginInit();
      this.SuspendLayout();
      // 
      // wv2
      // 
      this.wv2.AllowExternalDrop = true;
      this.wv2.CreationProperties = null;
      this.wv2.DefaultBackgroundColor = System.Drawing.Color.White;
      this.wv2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.wv2.Location = new System.Drawing.Point(0, 0);
      this.wv2.Name = "wv2";
      this.wv2.Size = new System.Drawing.Size(800, 450);
      this.wv2.TabIndex = 0;
      this.wv2.ZoomFactor = 1D;
      // 
      // htax
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.wv2);
      this.Name = "htax";
      ((System.ComponentModel.ISupportInitialize)(this.wv2)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    internal Microsoft.Web.WebView2.WinForms.WebView2 wv2;
  }
}

