/*
 * Syntela GmbH Leipzig (2006).
 * User: schulz
 * Date: 30.10.2006
 * Time: 16:31
 * 
 */

using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Helper
{
	/// <summary>
	/// Stellt einige nützliche Methoden zur Anzeige einer MessageBox bereit
	/// </summary>
	public static class MsgBox
	{
    	/// <summary>
    	/// Zeigt einen allgemeinen Fehler an und speichert den Fehlertext in einer Datei
    	/// </summary>	    
	    public static void ShowCmnError(string Text)
	    {
	        MessageBox.Show(
	            Text, 
	            "Allgemeiner Fehler",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1);
	        
	        //FileHelper.WriteLog("Error: " + Text);
	    }
	    
	    public static DialogResult ShowYesNoQuestion(string Text)
	    {
	        return MessageBox.Show(
	            Text, 
	            "Sicherheitsabfrage",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button2);
	    }	    
	    
	    public static void ShowHint(string Text)
	    {
	        MessageBox.Show(
	            Text, 
	            "Hinweis",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button1);
	        
	        //FileHelper.WriteLog("Hint: " + Text);
	    }	
	    
	    public static void ShowInfo(string Text)
	    {
	        MessageBox.Show(
	            Text, 
	            "Information",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1);	        
	    }		  

		public static void ShowSqlError(SqlException e)
		{
		    // "einfache" Fehlermeldung für Nutzer
	        MessageBox.Show(
	            e.Message, 
	            "Allgemeiner Fehler",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1);			

		    // umfangreiche Fehlermeldung für LogFile
			string msg = e.Message + "\n\n" +
						 "Fehler in Prozedur [" + e.Procedure + "] " +
						 "Zeile " + e.LineNumber.ToString() + "\n" +						 
						 "Server: " + e.Server  + ", " +
						 "Status: " + e.State  + "\n\n" +
						 "Stack: \n" + e.StackTrace;		    
		    
			//FileHelper.WriteLog("Sql: " + msg);		
		}		    
	}
}
