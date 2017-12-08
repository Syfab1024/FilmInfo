/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 28.03.2013
 * Zeit: 14:27
 * 
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Helper;
using Org.Mentalis.Utilities;

namespace FilmInfo
{
   /// <summary>
   /// Description of FilmInfoHttpServer.
   /// </summary>
   public class FilmInfoHttpServer
   {
      private bool m_FirstRun = true;
      
      private int m_Port = 80; //80: HTTP-Standard-Port
      public int Port {
         get { return m_Port; }
      }
      private int m_AccessCounter = 0;
      public int AccessCounter {
         get { return m_AccessCounter; }
      }
      private DateTime m_LastAccessTime;
      public DateTime LastAccessTime {
         get { return m_LastAccessTime; }
      }
      
      private string m_Prefix = "http://*:<Port>/";

      private string m_CachePath;
      private string m_MainPath;
      private HttpListener m_Listener;
      private HtmlExport m_HtmlExport;
      private FilmInfoManager m_filmInfoManager;
      private SearchInfo m_SearchInfo = new SearchInfo();         
      private string _ExecuteFilename;
      
      public FilmInfoHttpServer(string CachePath, 
                                string MainPath, 
                                HtmlExport htmlExport, 
                                FilmInfoManager filmInfoManager,
                                string ExecuteFilename)
      {
         if (!HttpListener.IsSupported)
         {
            Console.WriteLine ("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
         }
         
         m_filmInfoManager = filmInfoManager;
         m_CachePath = CachePath;
         m_MainPath = MainPath;
         m_HtmlExport = htmlExport;
         m_Prefix = m_Prefix.Replace("<Port>", m_Port.ToString());  
         _ExecuteFilename = ExecuteFilename;
      }
      
      public void UpdateMovieList(HtmlExport htmlExport)
      {
         m_HtmlExport = htmlExport;
      }
      
      public void StartServer()
      {
         if (m_FirstRun)
         {
            m_Listener = new HttpListener();
            m_Listener.Prefixes.Add(m_Prefix);
         }
         try
         {
            m_Listener.Start();
            m_FirstRun = false;
            ReceiveRequest();
         }
         catch(System.Net.HttpListenerException x)
         {
            MsgBox.ShowCmnError("Der Http-Server konnte nicht gestartet werden, weil das Programm nicht mit Administratorrechten läuft oder die IP-Adresse bereits besetzt ist.\nFehlermeldung: " + x.Message);
         }
      }
      
      private void ReceiveRequest()
      {
         if (!m_Listener.IsListening)
            return;
         IAsyncResult result = m_Listener.BeginGetContext(new AsyncCallback(ListenerCallback), m_Listener);
      }
      
      public bool IsRunning()
      {
         return m_Listener.IsListening;
      }
      
      private void StartProcess()
      {
         if (string.IsNullOrEmpty(_ExecuteFilename))
            return;
         
         string args = "";
         ProcessStartInfo pInfo = new ProcessStartInfo(_ExecuteFilename, args);
         
         // Start process
         Process mProcess = new Process();
         mProcess.StartInfo = pInfo;
         mProcess.Start();         
      }
      
      private void Shutdown()
      {
         WindowsController.ExitWindows(RestartOptions.ShutDown, false);
      }
      
      
      private byte[] ProcessMovieThumbnailerRequest(HttpListenerResponse response,
                                                    string MovieFilename)
      {
         string fileToThumbnail = Uri.UnescapeDataString(MovieFilename.Replace("mtn.htm?file=", ""));
         string outputThumbnail = m_filmInfoManager.GetMovieThumbnailerImage(fileToThumbnail);
         
         if (File.Exists(outputThumbnail))
         {
            response.ContentType = "image/jpeg";
            return File.ReadAllBytes(outputThumbnail);            
         }
         else
         {
            string responseString = "<HTML><BODY> Datei " + fileToThumbnail + " wurde nicht gefunden.</BODY></HTML>";
            return System.Text.Encoding.UTF8.GetBytes(responseString);
         }         
      }
      
      
      private byte[] ProcessFileRequest(HttpListenerRequest request,
                                       string file, out string ContentType)
      {            
         // MIME-Typen: http://de.selfhtml.org/diverses/mimetypen.htm
         ContentType = "text/html";
         string filename = "";
         
         
         if (Path.GetExtension(file) == ".jpg")
         {
            filename = Path.Combine(m_CachePath, file);
            ContentType = "image/jpeg";              
         }
         else if (Path.GetExtension(file) == ".png")
         {
            filename = Path.Combine(m_MainPath, file);      
            ContentType = "image/png";            
         }         
         else if (Path.GetExtension(file) == ".ico")
         {
            filename = Path.Combine(m_MainPath, file);         
            ContentType = "image/x-icon";            
         }                     
         else if (Path.GetExtension(file) == ".js")
         {
            filename = Path.Combine(m_MainPath, file);
            ContentType = "text/javascript";
         }                         
         else if (Path.GetExtension(file) == ".css")
         {
            filename = Path.Combine(m_MainPath, file);
            ContentType = "text/css";
         }    
         else if (Path.GetExtension(file) == ".gif")
         {
            filename = Path.Combine(m_MainPath, file);
            ContentType = "image/gif";
         }                 

         // wenn Datei existiert --> zurückgeben, sonst Fehlerseite
         if (File.Exists(filename))
         {
            return File.ReadAllBytes(filename);                        
         }
         else
         {
            string responseString = "<HTML><BODY> Datei " + request.RawUrl + " wurde nicht gefunden.</BODY></HTML>";
            return System.Text.Encoding.UTF8.GetBytes(responseString);                        
         }
      }
                  
      
      private byte[] ProcessSearchRequest(HttpListenerRequest request,
                                          HttpListenerResponse response)
      {           
         // Filterung vornehmen, falls Parameter gesetzt wurden
         if (request.QueryString.AllKeys.Length != 0)
         {            
            System.Collections.Specialized.NameValueCollection nvc 
               = System.Web.HttpUtility.ParseQueryString(request.RawUrl.Substring(request.RawUrl.IndexOf('?')), request.ContentEncoding);
            
            m_SearchInfo.Title = nvc["Title"];
            m_SearchInfo.Rating = nvc["Rating"];
            m_SearchInfo.Actor = nvc["Actor"];
            m_SearchInfo.Director = nvc["Director"];
            m_SearchInfo.SdHd = nvc["SdHd"];
            m_SearchInfo.Is3D = nvc["Is3D"];
            m_SearchInfo.Channels = nvc["Channels"];
            m_SearchInfo.Favourite = nvc["Favourite"];
            m_SearchInfo.Genre = nvc["Genre"];
            m_SearchInfo.Tag = nvc["Tag"];
            m_SearchInfo.VolumeLabel = nvc["Disk"];
            m_SearchInfo.Unknown = nvc["Unknown"];
            m_SearchInfo.Watched = nvc["Watched"];                  
            m_SearchInfo.MaxDuration = nvc["MaxDuration"];
            m_SearchInfo.ShowFrame = nvc["ShowFrame"];
            m_SearchInfo.Archieved = nvc["Archieved"];
            m_SearchInfo.OnlyMovies = nvc["OnlyMovies"];
            m_SearchInfo.MinYear = nvc["MinYear"];
            m_SearchInfo.ShowIconDetails = nvc["ShowIconDetails"];
            m_SearchInfo.MaxItemCount = nvc["MaxItemCount"];
            m_SearchInfo.HalfSize = nvc["HalfSize"];
            m_SearchInfo.OnlyWithMyRating = nvc["OnlyWithMyRating"];
            
            m_SearchInfo.SortByColumnName = nvc["SortByColumnName"];
            m_SearchInfo.SortAscending = nvc["SortAscending"];
                        
            
            int movieId = -1;                     
            string setWatchedId = request.QueryString.Get("SetWatched");
            if (!string.IsNullOrEmpty(setWatchedId)
                && int.TryParse(setWatchedId, out movieId))
            {
               m_HtmlExport.ToggleFilmInfoWatched(movieId);
            }
            
            string setFavouriteId = request.QueryString.Get("SetFavourite");
            if (!string.IsNullOrEmpty(setFavouriteId)
                && int.TryParse(setFavouriteId, out movieId))
            {
               m_HtmlExport.ToggleFilmInfoFavourite(movieId);
            }   

            string setArchivedId = request.QueryString.Get("SetArchived");
            if (!string.IsNullOrEmpty(setArchivedId)
                && int.TryParse(setArchivedId, out movieId))
            {
               m_HtmlExport.ToggleFilmInfoArchived(movieId);
            }
         }
         else
         {
            // wenn Seite neu betreten wird -> SuchInfos zurücksetzen
            // (damit nicht die vorhergehende Suche benutzt wird)
            
            m_SearchInfo = new SearchInfo();
            // Defaultwerte für Suchmaske
            m_SearchInfo.Watched = "Unchecked";
            m_SearchInfo.Archieved = "Unchecked";
            m_SearchInfo.OnlyMovies = "Checked";
            m_SearchInfo.ShowIconDetails = "Checked";
            m_SearchInfo.ShowFrame = "Nein";
            m_SearchInfo.MaxItemCount = "";
            m_SearchInfo.SortAscending = "Unchecked";
         }
         
         if (string.IsNullOrEmpty(m_SearchInfo.SortByColumnName))
         {
            m_SearchInfo.SortByColumnName = "Hinzugefuegt";
            m_SearchInfo.SortAscending = "Unchecked";
         }
         
         bool sortAscending = m_SearchInfo.SortAscending == "Checked";
         m_HtmlExport.SortFilmInfoList(m_SearchInfo.SortByColumnName, sortAscending);
         
         // HTML ausgeben
         string content = "";
         //if (m_SearchInfo.ViewStyle.IndexOf("Icons") != -1)
         {
            content = m_HtmlExport.GetHtmlExportIconList(m_SearchInfo);
         }
         /*
         else
         {
            content = m_HtmlExport.GetHtmlExportDetailList(m_SearchInfo);
         }
         */
            
            
         return System.Text.Encoding.UTF8.GetBytes(content);         
      }
      
      private byte[] ProcessDetailRequest(HttpListenerRequest request,
                                          HttpListenerResponse response)
      {     
         string content = "";          
         int movieId = -1;                     
         string MovieIdRaw = request.QueryString.Get("MovieId");
         if (!string.IsNullOrEmpty(MovieIdRaw)
             && int.TryParse(MovieIdRaw, out movieId))
         {
            content = m_HtmlExport.GetHtmlExportDetail(movieId);            
         }
         return System.Text.Encoding.UTF8.GetBytes(content);         
      }
      
      private byte[] ProcessActionRequest(HttpListenerRequest request,
                                          HttpListenerResponse response)
      {            
         string requestContent = GetRequestPostData(request);
         
         System.Collections.Specialized.NameValueCollection nvc 
            = System.Web.HttpUtility.ParseQueryString(requestContent, request.ContentEncoding);   
         
         int MovieId = -1;
         int.TryParse(nvc["movie_id"], out MovieId);
         bool Checked = false;
         bool.TryParse(nvc["checked"], out Checked);
         string TagList = nvc["taglist"];
         
         // Aktionen durchführen und entsprechendes JSON zurückliefern
         if (nvc["action"] == "watched")
         {
            string result = m_HtmlExport.AjaxSetWatched(MovieId, Checked);
            return System.Text.Encoding.UTF8.GetBytes(result);
         }
         if (nvc["action"] == "archived")
         {
            return System.Text.Encoding.UTF8.GetBytes(m_HtmlExport.AjaxSetArchived(MovieId, Checked));
         }
         if (nvc["action"] == "favourite")
         {
            return System.Text.Encoding.UTF8.GetBytes(m_HtmlExport.AjaxSetFavourite(MovieId, Checked));
         }     

         if (nvc["action"] == "taglist")
         {
            return System.Text.Encoding.UTF8.GetBytes(m_HtmlExport.AjaxSetTagList(MovieId, TagList));
         }               
         
         
         return System.Text.Encoding.UTF8.GetBytes("");         
      }
      
      public string GetRequestPostData(HttpListenerRequest request)
      {
         if (!request.HasEntityBody)
         {
            return null;
         }
         using (System.IO.Stream body = request.InputStream)
         {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
            {
               return reader.ReadToEnd();
            }
         }
      }

      
      /// <summary>
      /// Hauptfunktion, die bei Aufruf einer Webseite vom Browser aus gestartet wird
      /// und für alle weiteren Aktionen verantwortlich ist.
      /// </summary>
      /// <param name="result"></param>
      private void ListenerCallback(IAsyncResult result)
      {
         string file = "";
         
         try
         {         
            if (!m_Listener.IsListening 
                || result == null)
               return;
            
            
            HttpListenerContext context = m_Listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;            
            HttpListenerResponse response = context.Response;
            
            // Workaround for Socket-Timeout-"Bug" in Chrome 
            // https://code.google.com/p/chromium/issues/detail?id=131246         
            response.KeepAlive = false;                     
            
            // führendes "/"-Zeichen entfernen:
            file = request.RawUrl.Substring(1);
            
            byte[] buffer = null;
            
            bool shutdown = false;
            // Standard-Content-Type festlegen (kann sich je nach gesendeter Response ändern)
            string ContentType = "text/html; charset=UTF-8";
            string ClientUserAgent = request.UserAgent;         

            /*
            // Info in Logdatei schreiben...
            string errorMessage = DateTime.Now.ToString()
                                  + " " + request.RawUrl + "\n";
            string logFilename = Path.Combine(m_CachePath, "HttpServerLog.log");
            File.AppendAllText(logFilename, errorMessage);               
            */
                          
            // PC herunterfahren
            if (file.Equals("shutdown"))
            {
               // Shutdown erst nach Ausgabe des Textes ausführen, daher Umweg über Variable
               shutdown = true;
               buffer = System.Text.Encoding.UTF8.GetBytes("<HTML><BODY>PC wird heruntergefahren...</BODY></HTML>");
            }               
            
            // Datei ausführen
            else if (file.Equals("execute"))
            {
               StartProcess();
               buffer = System.Text.Encoding.UTF8.GetBytes("<HTML><BODY>Datei " 
                                                           + _ExecuteFilename 
                                                           + " wurde ausgefuehrt.</BODY></HTML>");
            }
            
            // Movie-Thumbnailer
            else if (file.StartsWith("mtn.htm"))
            {
               buffer = ProcessMovieThumbnailerRequest(response, file);
               ContentType = "image/jpeg";
            }
                          
            // Movie-Details
            else if (file.StartsWith("detail.htm"))
            {
               buffer = ProcessDetailRequest(request, response);
            }
                                         
            // Aktion durchführen
            else if (file.StartsWith("action.htm"))
            {
               buffer = ProcessActionRequest(request, response);
               ContentType = "application/json";
            }      
            
            // Suchparameter auswerten
            else if (string.IsNullOrEmpty(file)
                || file.StartsWith("search.htm"))
            {
               buffer = ProcessSearchRequest(request, response);
            }               
            
            // Ansonsten: Datei unverändert zurückliefern
            else
            {
               buffer = ProcessFileRequest(request, file, out ContentType);              
            }
            
            // Get a response stream and write the response to it.
            response.ContentType = ContentType;
            response.ContentLength64 = buffer.Length;
            
            // Workaround for Socket-Timeout-"Bug" in Chrome 
            // https://code.google.com/p/chromium/issues/detail?id=131246         
            response.KeepAlive = false;            
            System.IO.Stream output = response.OutputStream;
            
               
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            
            // Shutdown erst jetzt ausführen, da erst jetzt der HTML-Text beim 
            // Client angekommen ist.
            if (shutdown)
               Shutdown();
            
            // zu Statistikzwecken
            m_LastAccessTime = DateTime.Now;
            m_AccessCounter++;
         }
         catch(Exception ex)
         {
            // Fehler in Logdatei schreiben...
            string errorMessage = DateTime.Now.ToString()
                                  + ": Bei der Verarbeitung der Anfrage '"
                                  + file + "' trat ein Fehler auf: '" 
                                  + ex.Message + "'\n";
            string logFilename = Path.Combine(m_CachePath, "HttpServerLog.log");
            File.AppendAllText(logFilename, errorMessage);
         }   
         finally
         {
            ReceiveRequest();
         }
      }
               
      
      public void CloseServer()
      {
         if (m_Listener.IsListening)
            m_Listener.Close();
      }      
      
      public void StopServer()
      {
         if (m_Listener.IsListening)
            m_Listener.Stop();
      }
      
      public string ServerAdress
      {
         get {
            return "http://" + GetNetworkIpAdress() + ":" + m_Port.ToString();
         }
      }
      
      public string GetNetworkIpAdress()
      {
         System.Net.IPAddress HostIP = null;
         foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
         {
            // IPv4-Adresse ermitteln
            if (address.AddressFamily != AddressFamily.InterNetworkV6)
            {
               HostIP = address;
               break;
            }
         }
         
         return HostIP.ToString();
      }
   }
}
