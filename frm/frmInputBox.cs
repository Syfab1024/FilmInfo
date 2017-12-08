/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 08.11.2010
 * Zeit: 15:05
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FilmInfo
{
    /// <summary>
    /// Description of frmInputBox.
    /// </summary>
    public partial class frmInputBox : Form
    {
        public frmInputBox()
        {
            InitializeComponent();
            this.ActiveControl = tbInput;
            this.AcceptButton = btnOk;
        }
        
        public string ShowForm(string FormTitle, string InfoText, 
                               string DefaultValueText, string Label, bool IsPassword)
        {
            this.Text = FormTitle;
            lInfo.Text = InfoText;
            lLabel.Text = Label;
            tbInput.Text = DefaultValueText;
            if (IsPassword)
                tbInput.PasswordChar = '*';
            
            if (this.ShowDialog() == DialogResult.OK)
                return tbInput.Text;
            else
                return "";
        }
        
        void BtnOkClick(object sender, EventArgs e)
        {
        	DialogResult = DialogResult.OK;
        }
    }
}
