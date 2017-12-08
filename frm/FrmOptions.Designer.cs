/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 26.06.2013
 * Zeit: 14:12
 * 
 */
namespace FilmInfo
{
   partial class FrmOptions
   {
      /// <summary>
      /// Designer variable used to keep track of non-visual components.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      
      /// <summary>
      /// Disposes resources used by the form.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing) {
            if (components != null) {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }
      
      /// <summary>
      /// This method is required for Windows Forms designer support.
      /// Do not change the method contents inside the source code editor. The Forms designer might
      /// not be able to load this method if it was changed manually.
      /// </summary>
      private void InitializeComponent()
      {
      	this.btnSave = new System.Windows.Forms.Button();
      	this.label1 = new System.Windows.Forms.Label();
      	this.cbUseProxy = new System.Windows.Forms.CheckBox();
      	this.label2 = new System.Windows.Forms.Label();
      	this.tbExecuteFilename = new System.Windows.Forms.TextBox();
      	this.btnCancel = new System.Windows.Forms.Button();
      	this.SuspendLayout();
      	// 
      	// btnSave
      	// 
      	this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      	this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
      	this.btnSave.Location = new System.Drawing.Point(266, 60);
      	this.btnSave.Name = "btnSave";
      	this.btnSave.Size = new System.Drawing.Size(75, 23);
      	this.btnSave.TabIndex = 0;
      	this.btnSave.Text = "OK";
      	this.btnSave.UseVisualStyleBackColor = true;
      	// 
      	// label1
      	// 
      	this.label1.Location = new System.Drawing.Point(12, 9);
      	this.label1.Name = "label1";
      	this.label1.Size = new System.Drawing.Size(82, 17);
      	this.label1.TabIndex = 1;
      	this.label1.Text = "Proxy benutzen";
      	this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
      	// 
      	// cbUseProxy
      	// 
      	this.cbUseProxy.Location = new System.Drawing.Point(100, 4);
      	this.cbUseProxy.Name = "cbUseProxy";
      	this.cbUseProxy.Size = new System.Drawing.Size(23, 24);
      	this.cbUseProxy.TabIndex = 2;
      	this.cbUseProxy.UseVisualStyleBackColor = true;
      	// 
      	// label2
      	// 
      	this.label2.Location = new System.Drawing.Point(12, 36);
      	this.label2.Name = "label2";
      	this.label2.Size = new System.Drawing.Size(82, 17);
      	this.label2.TabIndex = 3;
      	this.label2.Text = "Ausführen von";
      	this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
      	// 
      	// tbExecuteFilename
      	// 
      	this.tbExecuteFilename.Location = new System.Drawing.Point(100, 33);
      	this.tbExecuteFilename.Name = "tbExecuteFilename";
      	this.tbExecuteFilename.Size = new System.Drawing.Size(322, 20);
      	this.tbExecuteFilename.TabIndex = 4;
      	// 
      	// btnCancel
      	// 
      	this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      	this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      	this.btnCancel.Location = new System.Drawing.Point(347, 60);
      	this.btnCancel.Name = "btnCancel";
      	this.btnCancel.Size = new System.Drawing.Size(75, 23);
      	this.btnCancel.TabIndex = 5;
      	this.btnCancel.Text = "Abbrechen";
      	this.btnCancel.UseVisualStyleBackColor = true;
      	// 
      	// FrmOptions
      	// 
      	this.AcceptButton = this.btnSave;
      	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      	this.CancelButton = this.btnCancel;
      	this.ClientSize = new System.Drawing.Size(434, 95);
      	this.Controls.Add(this.btnCancel);
      	this.Controls.Add(this.tbExecuteFilename);
      	this.Controls.Add(this.label2);
      	this.Controls.Add(this.cbUseProxy);
      	this.Controls.Add(this.label1);
      	this.Controls.Add(this.btnSave);
      	this.Name = "FrmOptions";
      	this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      	this.Text = "Einstellungen";
      	this.ResumeLayout(false);
      	this.PerformLayout();
      }
      public System.Windows.Forms.TextBox tbExecuteFilename;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Label label2;
      public System.Windows.Forms.CheckBox cbUseProxy;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button btnSave;
   }
}
