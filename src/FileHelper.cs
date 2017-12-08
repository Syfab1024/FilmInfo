/*
 * Syntela GmbH Leipzig (2006).
 * User: schulz
 * Date: 07.12.2006
 * Time: 12:35
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Helper
{
	/// <summary>
	/// Stellt nützliche Methoden zur Arbeit mit dem FileSystem bereit
	/// </summary>
	public static class FileHelper
	{	    
    	/// <summary>
    	/// Schreibt den übergebenen Text in eine LogDatei 
    	/// (Application.ExecutablePath\SynVertrag.log)
    	/// </summary>	    
	    public static void WriteLog(string Text)
	    {
	        string LogFile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
            string Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // Dateigröße ermitteln,
            // falls bestimmte Größe überschritten wird, nur die letzten 100
            // Zeilen der Datei aufheben
            if (File.Exists(LogFile))
            {
                System.IO.FileInfo fi = new FileInfo(LogFile);
                if (fi.Length > 1000000) // größer als ca. 1 MB?
                {
                    // nur die letzten 100 Zeilen aufheben
                    string[] sOld = File.ReadAllLines(LogFile);
                    string[] sNew = new string[100];
                    int j = 0;
                    for (int i = sOld.Length - 100; i < sOld.Length; i++)
                    {
                        sNew[j++] = sOld[i];
                    }
                    File.WriteAllLines(LogFile, sNew, System.Text.Encoding.UTF8);                    
                }                
            }
            
            // Vorarbeit erledigt,
            // Text in Datei schreiben
            using (StreamWriter sw = new StreamWriter(LogFile, true, System.Text.Encoding.UTF8)) 
            {
                try 
                {
                    sw.WriteLine("\n" +
                                 "[" + Time + "]" + " " + Text);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n" +
                                    "Fehler beim Schreiben in die Datei '" + LogFile + "'.\r\n " +
                                    "Evtl.",
                                    "Fehler",
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error, 
                                    MessageBoxDefaultButton.Button1);
                }
                finally
                {
                    sw.Close();
                }                
            }	        
	    }
		
	}
}
