/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 08.11.2010
 * Zeit: 15:05
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
namespace FilmInfo
{
    partial class frmInputBox
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
        	this.btnCancel = new System.Windows.Forms.Button();
        	this.btnOk = new System.Windows.Forms.Button();
        	this.tbInput = new System.Windows.Forms.TextBox();
        	this.lInfo = new System.Windows.Forms.Label();
        	this.lLabel = new System.Windows.Forms.Label();
        	this.pictureBox1 = new System.Windows.Forms.PictureBox();
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// btnCancel
        	// 
        	this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnCancel.Location = new System.Drawing.Point(276, 123);
        	this.btnCancel.Name = "btnCancel";
        	this.btnCancel.Size = new System.Drawing.Size(73, 23);
        	this.btnCancel.TabIndex = 11;
        	this.btnCancel.Text = "Abbrechen";
        	this.btnCancel.UseVisualStyleBackColor = true;
        	// 
        	// btnOk
        	// 
        	this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.btnOk.Location = new System.Drawing.Point(197, 123);
        	this.btnOk.Name = "btnOk";
        	this.btnOk.Size = new System.Drawing.Size(73, 23);
        	this.btnOk.TabIndex = 10;
        	this.btnOk.Text = "OK";
        	this.btnOk.UseVisualStyleBackColor = true;
        	this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
        	// 
        	// tbInput
        	// 
        	this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tbInput.Location = new System.Drawing.Point(113, 97);
        	this.tbInput.MaxLength = 300;
        	this.tbInput.Name = "tbInput";
        	this.tbInput.Size = new System.Drawing.Size(327, 20);
        	this.tbInput.TabIndex = 9;
        	// 
        	// lInfo
        	// 
        	this.lInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.lInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lInfo.Location = new System.Drawing.Point(112, 20);
        	this.lInfo.Name = "lInfo";
        	this.lInfo.Size = new System.Drawing.Size(328, 74);
        	this.lInfo.TabIndex = 8;
        	this.lInfo.Text = "Der von Ihnen angegebene Proxyserver verlangt eventuell ein Passwort:";
        	// 
        	// lLabel
        	// 
        	this.lLabel.Location = new System.Drawing.Point(13, 100);
        	this.lLabel.Name = "lLabel";
        	this.lLabel.Size = new System.Drawing.Size(94, 16);
        	this.lLabel.TabIndex = 12;
        	this.lLabel.Text = "label1";
        	this.lLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
        	// 
        	// pictureBox1
        	// 
        	this.pictureBox1.Location = new System.Drawing.Point(12, 18);
        	this.pictureBox1.Name = "pictureBox1";
        	this.pictureBox1.Size = new System.Drawing.Size(42, 38);
        	this.pictureBox1.TabIndex = 13;
        	this.pictureBox1.TabStop = false;
        	// 
        	// frmInputBox
        	// 
        	this.AcceptButton = this.btnOk;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.btnCancel;
        	this.ClientSize = new System.Drawing.Size(454, 158);
        	this.Controls.Add(this.pictureBox1);
        	this.Controls.Add(this.lLabel);
        	this.Controls.Add(this.btnCancel);
        	this.Controls.Add(this.btnOk);
        	this.Controls.Add(this.tbInput);
        	this.Controls.Add(this.lInfo);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.Name = "frmInputBox";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        	this.Text = "Abfrage";
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lLabel;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lInfo;
    }
}
