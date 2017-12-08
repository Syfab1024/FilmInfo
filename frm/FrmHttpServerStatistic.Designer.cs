/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 02.04.2013
 * Zeit: 08:51
 * 
 */
namespace FilmInfo
{
   partial class FrmHttpServerStatistic
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
      	this.components = new System.ComponentModel.Container();
      	this.tbInfo = new System.Windows.Forms.TextBox();
      	this.tStatisticUpdate = new System.Windows.Forms.Timer(this.components);
      	this.SuspendLayout();
      	// 
      	// tbInfo
      	// 
      	this.tbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      	      	      	| System.Windows.Forms.AnchorStyles.Left) 
      	      	      	| System.Windows.Forms.AnchorStyles.Right)));
      	this.tbInfo.Location = new System.Drawing.Point(12, 12);
      	this.tbInfo.Multiline = true;
      	this.tbInfo.Name = "tbInfo";
      	this.tbInfo.ReadOnly = true;
      	this.tbInfo.Size = new System.Drawing.Size(527, 249);
      	this.tbInfo.TabIndex = 0;
      	// 
      	// tStatisticUpdate
      	// 
      	this.tStatisticUpdate.Interval = 1000;
      	this.tStatisticUpdate.Tick += new System.EventHandler(this.TStatisticUpdateTick);
      	// 
      	// FrmHttpServerStatistic
      	// 
      	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      	this.ClientSize = new System.Drawing.Size(551, 273);
      	this.Controls.Add(this.tbInfo);
      	this.Name = "FrmHttpServerStatistic";
      	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      	this.Text = "HttpServer-Statistik";
      	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmHttpServerStatisticFormClosing);
      	this.ResumeLayout(false);
      	this.PerformLayout();
      }
      private System.Windows.Forms.Timer tStatisticUpdate;
      private System.Windows.Forms.TextBox tbInfo;
   }
}
