/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 26.02.2013
 * Zeit: 08:11
 * 
 */
using System;
using System.Windows.Forms;

namespace FilmInfo
{
   /// <summary>
   /// Class with program entry point.
   /// </summary>
   internal sealed class Program
   {
      /// <summary>
      /// Program entry point.
      /// </summary>
      [STAThread]
      private static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new FrmMain());
      }
      
   }
}
