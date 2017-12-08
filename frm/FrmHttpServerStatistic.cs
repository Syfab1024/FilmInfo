/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 02.04.2013
 * Zeit: 08:51
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FilmInfo
{
   /// <summary>
   /// Description of FrmHttpServerStatistic.
   /// </summary>
   public partial class FrmHttpServerStatistic : Form
   {
      private FilmInfoHttpServer m_FilmInfoHttpServer;
      
      public FrmHttpServerStatistic(FilmInfoHttpServer Server)
      {
         InitializeComponent();
         m_FilmInfoHttpServer = Server;
      }
      
      void TStatisticUpdateTick(object sender, EventArgs e)
      {
         tbInfo.Text = 
            "Aktuelle Zeit: " + DateTime.Now.ToString() + "\r\n"
            + "Letzte Zugriffszeit: " + m_FilmInfoHttpServer.LastAccessTime.ToString() + "\r\n"
            + "Objektanzahl: " + m_FilmInfoHttpServer.AccessCounter.ToString();
      }
      
      void FrmHttpServerStatisticFormClosing(object sender, FormClosingEventArgs e)
      {
      	e.Cancel = true;
      	tStatisticUpdate.Enabled = false;
      	this.Hide();
      }
      
      public void ShowStats()
      {
      	tStatisticUpdate.Enabled = true;
      	
      	this.Show();         
      	this.TopMost = true;
      	this.TopMost = false;
      }
   }
}
