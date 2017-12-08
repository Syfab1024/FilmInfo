/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 28.03.2013
 * Zeit: 15:10
 * 
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

using MediaInfoLib;
using MIL.Html;
using Helper;

namespace FilmInfo
{
   /// <summary>
   /// Description of FilmInfoManager.
   /// </summary>
   public class FilmInfoManager
   {
      private MediaInfo _MediaInfo = new MediaInfo();     
      private ExtendedWebClient _extendedWebClient = new ExtendedWebClient();      
      private Dictionary<string, string> _AkaList = new Dictionary<string, string>();
      private string _CachePath;
      private string _MovieThumbnailerPath;
      private string _UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";        
      private string _MoviePosterNotAvailableFilename = "NotAvailable.jpg";
      private DateTime _LastLoginTime = new DateTime(0);
         
      public string MoviePosterNotAvailableFilename {
         get { return _MoviePosterNotAvailableFilename; }
      }      
      
      public FilmInfoManager(string CachePath, 
                             Dictionary<string, string> AkaList,
                             string MovieThumbnailerPath,
                             bool UseProxy)
      {
         if (_MediaInfo.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0").Length == 0)
         {
            MsgBox.ShowCmnError("MediaInfo.dll konnte nicht erfolgreich geladen werden. Abbruch.");
         }
         
         if (UseProxy)
         {
            _extendedWebClient.SetProxy("127.0.0.1", 8888, "", "");
         }
         
         _CachePath = CachePath;
         _MoviePosterNotAvailableFilename = Path.Combine(_CachePath, _MoviePosterNotAvailableFilename);
         _MovieThumbnailerPath = MovieThumbnailerPath;
         _AkaList = AkaList;
      }
      
      
      public Image GetPosterImage(FilmInfo fi, 
                                  int Width, int Height, 
                                  bool OnlyDownload,
                                  bool DownloadLargeImage)
      {
         Image img = null;

         try
         {
            string imgFilename;
            string imgLargeFilename;
            
            if (fi.IsMoviePosterAvailable())
            {               
               imgLargeFilename = Path.Combine(_CachePath, fi.ImdbId + "_large.jpg");               
               if (DownloadLargeImage && !File.Exists(imgLargeFilename))
               {
                  _extendedWebClient.DownloadFile(fi.PosterUrlLarge, imgLargeFilename);  
               }               
               
               imgFilename = Path.Combine(_CachePath, fi.ImdbId + ".jpg");
               if (!DownloadLargeImage && !File.Exists(imgFilename))
               {
                  _extendedWebClient.DownloadFile(fi.PosterUrl, imgFilename);                     
               }
                                                
            }
            else
            {
               imgFilename = _MoviePosterNotAvailableFilename;
            }

            if (!OnlyDownload)
            {
               img = Image.FromFile(imgFilename);
               if (Width > 0)
                  img = img.GetThumbnailImage(Width, Height, ()=>false, IntPtr.Zero);               
            }
         }
         catch(Exception x)
         {
            string s = x.Message;
         }
         
         return img;
      }      
      
      public string GetMovieThumbnailerImage(string Filename)
      {
         string movieFilename = Filename;
         //movieFilename = @"d:\temp\d\salmo-23-psalm-23_1110_480x320.mp4";
         
         if (!File.Exists(movieFilename))
            return null;
         
         string imgFilename = "";
                
         try
         {
            string OutputFilename = Path.GetFileNameWithoutExtension(movieFilename);
            imgFilename = Path.Combine(_CachePath, OutputFilename + "_s.jpg"); 
            
            // wenn Ziel-Datei noch nicht existiert:
            // MovieThumbnailer (mtn) aufrufen
            if (!File.Exists(imgFilename))
            {
               /*
               http://moviethumbnail.sourceforge.net/usage.en.html:
               Params: Höhe: 100px, Blank-Faktor 0,6, OutputPath, keine Pause
               */
               string args = "-h 100 -b 0,6 -O \"" + _CachePath + "\" -P "
                             + "\"" + movieFilename + "\"";
               ProcessStartInfo pInfo = 
                  new ProcessStartInfo(_MovieThumbnailerPath, args);
               pInfo.WindowStyle = ProcessWindowStyle.Minimized;
      
               
               // Start processö
               Process mProcess = new Process();
               mProcess.StartInfo = pInfo;
               mProcess.Start();
               // Warten, bis Bild generiert wurde
               mProcess.WaitForExit();
            }           
         }
         catch(Exception)
         {
            
         }
         
         return imgFilename;
      }            
      
      
      private string GetQueryStringFromAkaList(string Query)
      {
         if (_AkaList.ContainsKey(Query))
            return _AkaList[Query];
         else
            return Query;
      }      
      
      private string NormalizeMovieName(string Movie)
      {
         string akaMovie = GetQueryStringFromAkaList(Movie);
         if (akaMovie != Movie)
            return akaMovie;
         
         // sinnlose Strings ersetzen
         Movie = Movie.Replace(".", " ");
         Movie = Movie.Replace("_", " ");
         Movie = Movie.Replace("-", " ");
         Movie = Regex.Replace(Movie, "BluRay", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "Remastered", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "3D", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "x264", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "HDTV", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "DTS", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "1080p", "", RegexOptions.IgnoreCase);
         Movie = Regex.Replace(Movie, "720p", "", RegexOptions.IgnoreCase);

         // die 2-Buchstabenkombinationen kommen in normalen Wörtern relativ
         // häufig vor --> Groß/Kleinschreibung beachten!
         Movie = Regex.Replace(Movie, "AC", ""); 
         Movie = Regex.Replace(Movie, "ML", "");
         Movie = Regex.Replace(Movie, "DL", "");     
         Movie = Regex.Replace(Movie, "Der", "");
         Movie = Regex.Replace(Movie, "Die", "");
         Movie = Regex.Replace(Movie, "Das", "");         
         
         
         Movie = Regex.Replace(Movie, "ü", "ue");         
         Movie = Regex.Replace(Movie, "ä", "ae");         
         Movie = Regex.Replace(Movie, "ö", "oe");         
         
         // Behandlung von Dateinamen wie 
         // "Lockout.2012.German.DTS.DL.720p.BluRay.x264-LeetHD"
         // -> ab "German" oder Jahreszahl den String abschneiden!
         int cutIndex = -1;
         int germanIndex = -1;
         int yearIndex = -1;
         
         // ggf. Jahreszahl entfernen, wenn zwischen 1970 und 2020..
         string regex = @"(\d\d\d\d)";
         Regex r = new Regex(regex, 
                             RegexOptions.Compiled
                             | RegexOptions.IgnoreCase 
                             | RegexOptions.Singleline);
         
         Match m = r.Match(Movie);       
         
         if (m.Success)
         {
            // aktuelle Jahreszahl?
            int year = int.Parse(m.Groups[1].Value);
            if (year > 1950 && year < 2020)
               yearIndex = m.Groups[1].Index;
               //Movie = Movie.Replace(year.ToString(), "");
         }
         
         // nach "German" suchen
         germanIndex = Movie.ToLower().IndexOf("german");
         
         // Min-Index von "German" und Jahreszahl finden
         if (yearIndex == -1)
            yearIndex = 99999;
         if (germanIndex == -1)
            germanIndex = 99999;         
         if (germanIndex < yearIndex)
            cutIndex = germanIndex;
         else
            cutIndex = yearIndex;
         
         if (cutIndex != -1 && cutIndex < Movie.Length)
            Movie = Movie.Substring(0, cutIndex - 1);        
   
         
         // ggf. Leerzeichen hinzufügen
         // Macht bpsw. aus "TheBigLebowski" -> "The Big Lebowski"
         // aus "KevinAlleinZuHause 2" -> "Kevin Allein Zu Hause 2"         
         string normalized = "";
         char lastChar = ' ';
         foreach(char currentChar in Movie)
         {
            string seperator = "";
            // Leerzeichen dazwischenschieben, wenn akt. Zeichen 
            // ein Großbuchstabe oder eine Zahl ist
            if (char.IsUpper(currentChar) || char.IsDigit(currentChar))
               seperator = " ";                
            
            // wenn bereits das letzte Zeichen 
            // ein Großbuchstabe oder eine Zahl war
            // --> Leerzeichen wieder rückgängig machen
            if (char.IsUpper(lastChar) || char.IsDigit(lastChar) || lastChar == ' ')
               seperator = "";
                
            normalized += seperator + currentChar;
            lastChar = currentChar;
         }
         
         return normalized.TrimStart(' ').TrimEnd(' ');
      }      

      private string GetGroupValueFromRegEx(string RegExp, string Input, int Group)
      {
         Regex r;
         Match m;
         
         r = new Regex(RegExp,
                              RegexOptions.Compiled
                              | RegexOptions.IgnoreCase
                              | RegexOptions.Singleline);
         
         m = r.Match(Input);
         
         if (m.Success)
         {
            return m.Groups[Group].Value;
         }  
         else
         {
            return "";
         }
      }
      
      private string GetImdbDetailLink(string HtmlContent)
      {
         string imdbBaseAdress = @"http://www.imdb.com";
                 
         MIL.Html.HtmlDocument mHtml = MIL.Html.HtmlDocument.Create(HtmlContent, false);
         HtmlNodeCollection hnc;
         
         hnc = mHtml.Nodes.FindByAttributeNameValue("class", "findResult odd");         
         hnc = hnc.FindByName("a");
            
         if (hnc.Count < 2)
            return "";
         
         
         string searchInfo = hnc[1].HTML;
         
         // searchInfo-Beispiel: "<a href="/title/tt0351283/?ref_=fn_al_tt_1" >Madagascar</a>"
         string link = GetGroupValueFromRegEx("<a href=\"(/title/tt\\d+.+)\" >(.+)</a>",
                                              searchInfo, 1);
         
         return imdbBaseAdress + System.Web.HttpUtility.HtmlDecode(link);
      }
      
      private void ProcessImdbDetails(string HtmlContent, FilmInfo fi)
      {         
         //string imdbBaseAdress = @"http://www.imdb.com";
         string searchInfo = "";            
         
         MIL.Html.HtmlDocument mHtml = MIL.Html.HtmlDocument.Create(HtmlContent, false);
         HtmlNodeCollection hnc;
         
         // Poster-Link
         //hnc = mHtml.Nodes.FindByAttributeNameValue("class", "maindetails_center");      
         //hnc = mHtml.Nodes.FindByAttributeNameValue("id", "img_primary");
         hnc = mHtml.Nodes.FindByAttributeNameValue("class", "poster");         
         
         hnc = hnc.FindByName("img");
         if (hnc.Count != 0)
         {
            searchInfo = hnc[0].HTML;
            
            // searchInfo-Beispiel: "<a href="/media/rm2305203456/tt0351283?ref_=tt_ov_i" ><img height="317" width="214" alt="Madagascar (2005) Poster" title="Madagascar (2005) Poster" src="http://ia.media-imdb.com/images/M/MV5BMTY4NDUwMzQxMF5BMl5BanBnXkFtZTcwMDgwNjgyMQ@@._V1_SX214_.jpg" itemprop="image" /></a>"
            fi.PosterUrl = GetGroupValueFromRegEx("src=\"(.+?)\"", searchInfo, 1);                                 
         }

         // movieName
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "name");
         fi.Title = System.Web.HttpUtility.HtmlDecode(hnc[0].FirstChild.HTML);
         hnc = mHtml.Nodes.FindByAttributeNameValue("class", "originalTitle");
         if (hnc.Count != 0)
            fi.TitleEnglish = System.Web.HttpUtility.HtmlDecode(hnc[0].FirstChild.HTML);
         else
            fi.TitleEnglish = fi.Title;
         
         //hnc = mHtml.Nodes.FindByAttributeNameValue("class", "nobr");
         hnc = mHtml.Nodes.FindByAttributeNameValue("id", "titleYear");
         if (hnc.Count != 0)
         {
            int.TryParse(GetGroupValueFromRegEx(">(\\d+)</a>", hnc[0].HTML, 1), out fi.Year);
         }
         
         /*
         // contentRating (Altersbeschränkung)
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "contentRating");      
         fi.ContentRating = hnc[0].FirstChild.HTML;         
         */
        
         /*
         // my Rating --> kann u.U. mehrfach im HTML-Code vorkommen, falls bereits
         // auf der Seite angezeigte Film-Empfehlungen bewertet wurden...
         // daher: erstmal oberes HTML-Stück ausschneiden (bis zur Beschreibung), damit nur dort gesucht wird
         int maxMyRatingIdx = HtmlContent.IndexOf("itemprop=\"actors\"");
         string myRatingContent;
         if (maxMyRatingIdx > 0)
            myRatingContent = HtmlContent.Substring(0, maxMyRatingIdx);
         else
            myRatingContent = HtmlContent;

         MIL.Html.HtmlDocument mHtmlMyRating = MIL.Html.HtmlDocument.Create(myRatingContent, false);
            
         fi.MyImdbRating = -1;
         hnc = mHtmlMyRating.Nodes.FindByAttributeNameValue("class", "rating-rating rating-your");
         if (hnc.Count != 0)
         {
            string myRating = hnc[0].HTML;
            //<span class="rating-rating rating-your"><span class="value">8</span><span class="grey">/</span><span class="grey">10</span></span>
            myRating = GetGroupValueFromRegEx("<span class=\"value\">(\\d+)</span>", myRating, 1);
            fi.MyImdbRating = int.Parse(myRating);
         }
        
         */
         
         // duration
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "duration");   
         if (hnc.Count != 0)
            if (hnc[0].FirstChild != null)
               fi.Runtime = hnc[0].FirstChild.HTML;                  

         // genres
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "genre");
         List<string> genres = new List<string>();
         for(int i = 0; i < hnc.Count - 1; i++)
         {
            genres.Add(hnc[i].FirstChild.HTML);
         }
         fi.Genres = genres;
         
         // datePublished
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "datePublished");     
         if (hnc.Count != 0)
         {
            searchInfo = hnc[0].HTML;
            string datePublished = GetGroupValueFromRegEx("content=\"(.+)\"", searchInfo, 1);         
            //string datePublishedCountry = hnc[0].Next.HTML;
            fi.ReleaseDate2 = datePublished;            
         }

         hnc = mHtml.Nodes.FindByAttributeNameValue("id", "titleDetails");
         string titleDetails = hnc[0].HTML;
         fi.Country = GetGroupValueFromRegEx("<a href=\"/search/title\\?country_of_origin=.+?>([\\w\\s]+)</a>", titleDetails, 1);
         //File.WriteAllText(@"C:\temp\debug.txt", titleDetails);
         

         // rating
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "ratingValue");
         if (hnc.Count != 0)
            fi.RatingImdb = decimal.Parse(hnc[0].FirstChild.HTML.Replace(".", ","));
         
         // ratingCount
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "ratingCount");      
         if (hnc.Count != 0)
            fi.RatingImdbCount = int.Parse(hnc[0].FirstChild.HTML.Replace(".", "").Replace(",", ""));
         
         // metaCriticRating
         hnc = mHtml.Nodes.FindByAttributeNameValue("href", "criticreviews?ref_=tt_ov_rt");
         if (hnc.Count != 0)
         {
            fi.ratingMetaCriticString = hnc[0].FirstChild.FirstChild.FirstChild.HTML;
         }            
         else
            fi.ratingMetaCriticString = "-";
         
         // description
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "description");  
         if (hnc.Count != 0)
            fi.PlotSimple = hnc[0].FirstChild.HTML;
         
         // directors
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "director");      
         hnc = hnc.FindByAttributeNameValue("itemprop", "name");      
         List<string> directors = new List<string>();
         for(int i = 0; i < hnc.Count; i++)
         {
            directors.Add(hnc[i].FirstChild.HTML);
         }         
         fi.Directors = directors;
         
         
         // creators
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "creator");      
         hnc = hnc.FindByAttributeNameValue("itemprop", "name");      
         List<string> creators = new List<string>();
         for(int i = 0; i < hnc.Count; i++)
         {
            creators.Add(hnc[i].FirstChild.HTML);
         }  
         fi.Creators = creators;
         
         // actors
         hnc = mHtml.Nodes.FindByAttributeNameValue("itemprop", "actor");      
         hnc = hnc.FindByAttributeNameValue("itemprop", "name");      
         List<string> actors = new List<string>();
         for(int i = 0; i < hnc.Count; i++)
         {
            actors.Add(hnc[i].FirstChild.HTML);
         }   
         fi.Actors = actors;
      }
      
      private void AddHttpHeaders(System.Net.WebHeaderCollection WebHeaders,
                                  bool UseCompression)
      {
         WebHeaders.Clear();
         WebHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
         if (UseCompression)
            WebHeaders.Add("Accept-Encoding", "gzip, deflate");
         WebHeaders.Add("Accept-Language", "de-de,de;q=0.8,en-us;q=0.5,en;q=0.3");
         WebHeaders.Add("User-Agent", _UserAgent);                     
      }
      
      public bool ImdbLogin(string Username, string Password)
      {
         return true;
         
         // weil Login seit 2016 deutlich komplizierter + mit Captcha --> nicht mehr machen...
         
         /*
         
         // wenn letzter Login weniger als 1 Stunde zurückliegt -> kein erneutes Login
         if ((DateTime.Now - _LastLoginTime).TotalHours < 1
             || string.IsNullOrEmpty(Username))
            return true;
         
         bool _IsLoggedIn = false;
         
         // Erstmal die Login-Hauptseite laden (dort wo man Name/Passwort eingibt)
         AddHttpHeaders(_extendedWebClient.Headers, false);
         
         string content1 = _extendedWebClient.Get("https://secure.imdb.com/register-imdb/login?ref_=nv_usr_lgin_1");
         
         // auf der Login-Seite ist ein Hidden-Text-Field eingebaut. Diese Informationen
         // müssen wieder per Post beim eigentlichen Login an die Seite übergeben werden
         string regex = "<h3>Sign in with IMDb</h3>\\s+<input type=\"hidden\" name=\"(.+)\" value=\"(.+)\" />";
         Regex r = new Regex(regex, 
                             RegexOptions.Compiled
                             | RegexOptions.IgnoreCase 
                             | RegexOptions.Singleline);
         
         Match m = r.Match(content1);       
         
         string name = "";
         string value = "";
         if (m.Success)
         {
            name = m.Groups[1].Value;
            value = m.Groups[2].Value;
         }
         
         // eigentliches Login vorbereiten
         Dictionary<string, string> parameters = new Dictionary<string, string>();    
         parameters.Add(name, value);
         parameters.Add("login", Username);
         parameters.Add("password", Password);                  
         
         AddHttpHeaders(_extendedWebClient.Headers, false); 

         //System.Net.ServicePointManager.Expect100Continue = false;
         string htmlContent = _extendedWebClient.Post("https://secure.imdb.com" +
                                                      "/register-imdb/login?ref_=nv_usr_lgin_1",
                                                      parameters);    
         

         if (htmlContent.IndexOf("invalid login information") != -1)
         {
            _IsLoggedIn = false;
         }
         else
         {
            _IsLoggedIn = true;
            _LastLoginTime = DateTime.Now;
         }         
         
         return _IsLoggedIn;
         
         */
      }
      
      public FilmInfo GetMovieInfo(string Query, bool ForceUpdate, string ImdbId)
      {  
         FilmInfo fi = new FilmInfo();

         if (!ImdbLogin("geheimerNutzer", "geheimesPasswort"))
         {
            MsgBox.ShowCmnError("Das Login bei IMDB war leider nicht erfolgreich. Abbruch");
            fi.RetrieveState = "IMDB-Login-Error";
            return fi;
         }

         fi.QueryOriginal = Query;
         Query = NormalizeMovieName(Query);
         fi.Query = Query;
         
         // absichtlich unbekannter Film -> nicht suchen!
         if (Query == "-")
         {
            return fi;
         }
                  
         
         try
         {            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            if (string.IsNullOrEmpty(Query))
            {
               fi.RetrieveState = "Query is empty";
               return fi;
            }
                
            // Suche nach IMDB-ID oder SuchString?
            string searchQueryLink = "";
            string detailLink = "";
            string searchContentHtml = "";
            string detailContentHtml = "";
            
            if (!Query.StartsWith("tt") && string.IsNullOrEmpty(ImdbId))   
            {               
               searchQueryLink = "http://www.imdb.com/find?q={Query}&s=tt";
               searchQueryLink = searchQueryLink.Replace("{Query}", 
                                                 System.Web.HttpUtility.UrlEncode(Query));
               
               _extendedWebClient.Headers.Add("User-Agent", _UserAgent);
               
               searchContentHtml = _extendedWebClient.Get(searchQueryLink);
               searchContentHtml = searchContentHtml.Replace("\r", " ").Replace("\n", " ");
               detailLink = GetImdbDetailLink(searchContentHtml);
            }
            else
            {
               if (Query.StartsWith("tt"))
                  detailLink = "http://www.imdb.com/title/" + Query;
               else
                  detailLink = "http://www.imdb.com/title/" + ImdbId;
            }

            if (string.IsNullOrEmpty(detailLink))
            {
               fi.RetrieveState = "Movie not found in IMDB.";
               
               return fi;
            }
            
            // Download der Detailinfos
            AddHttpHeaders(_extendedWebClient.Headers, true);
            detailContentHtml = _extendedWebClient.Get(detailLink);
            stopwatch.Stop();
            fi.QueryDuration = stopwatch.ElapsedMilliseconds;
            detailContentHtml = detailContentHtml.Replace("\r", " ").Replace("\n", " ");
            
            // verarbeiten der heruntergeladenen Infos
            stopwatch.Reset();
            stopwatch.Start();
            ProcessImdbDetails(detailContentHtml, fi);
            stopwatch.Stop();
            fi.ProcessDuration = stopwatch.ElapsedMilliseconds;            
            
            fi.ImdbId = GetGroupValueFromRegEx("/title/(tt\\d+)/?", detailLink, 1);
            fi.ImdbUrl = detailLink;

            fi.DownloadTimestamp = DateTime.Now;
         }
         catch(Exception ex)
         {
            fi.RetrieveState = ex.Message;
         }
         
         return fi;
      }            
      
      public void UpdateFilmInfoByFile(FilmInfo fi)
      {
         try 
         {  
            if (Directory.Exists(fi.Filename))
            {
               fi.FileLastWriteDate = Directory.GetCreationTime(fi.Filename);
               
               DriveInfo di = new DriveInfo(Path.GetPathRoot(fi.Filename));
               fi.VolumeLabel = di.VolumeLabel;                 
            }
            
            // wenn Datei existiert -> aktualisieren
            if (File.Exists(fi.Filename))
            {
               fi.Filesize = new FileInfo(fi.Filename).Length;
               fi.FileLastWriteDate = File.GetLastWriteTime(fi.Filename);
               
               DriveInfo di = new DriveInfo(Path.GetPathRoot(fi.Filename));
               fi.VolumeLabel = di.VolumeLabel;               
               
               _MediaInfo.Open(fi.Filename);
               
               // Video
               fi.Duration = _MediaInfo.Get(StreamKind.Video, 0, "Duration");
               //fi.VideoFormat = _MediaInfo.Get(StreamKind.Video, 0, "Codec ID");
               fi.VideoFormat = _MediaInfo.Get(StreamKind.General, 0, "Video_Format_WithHint_List");               
               fi.VideoBitrate = _MediaInfo.Get(StreamKind.Video, 0, "BitRate");
               fi.VideoDimension = _MediaInfo.Get(StreamKind.Video, 0, "Width");
               fi.VideoDimension = fi.VideoDimension + " x "
                                   + _MediaInfo.Get(StreamKind.Video, 0, "Height");
               fi.VideoFramerate = _MediaInfo.Get(StreamKind.Video, 0, "FrameRate");
               double videoFramerate = 0;
               if (double.TryParse(fi.VideoFramerate.Replace(".", ","), out videoFramerate))
               {
                  fi.VideoFramerate = Math.Round(videoFramerate, 0).ToString("0");
               }
               else
               {
                  fi.VideoFramerate = "0";
               }
               
               // Audio
               //fi.AudioFormat = _MediaInfo.Get(StreamKind.Audio, 1, "Format");
               fi.AudioFormatList = _MediaInfo.Get(StreamKind.General, 0, "Audio_Format_WithHint_List");
               fi.AudioLanguageList = _MediaInfo.Get(StreamKind.General, 0, "Audio_Language_List");
               
               string tmpAudioChannels = "";
               fi.AudioChannels = _MediaInfo.Get(StreamKind.Audio, 0, "Channels");
               tmpAudioChannels = _MediaInfo.Get(StreamKind.Audio, 1, "Channels");
               if (!string.IsNullOrEmpty(tmpAudioChannels))
                  fi.AudioChannels = fi.AudioChannels + ", " + tmpAudioChannels;
               tmpAudioChannels = _MediaInfo.Get(StreamKind.Audio, 2, "Channels");
               if (!string.IsNullOrEmpty(tmpAudioChannels))
                  fi.AudioChannels = fi.AudioChannels + ", " + tmpAudioChannels;
               
               /*
               fi.AudioBitrate = _MediaInfo.Get(StreamKind.Audio, 1, "BitRate");               
               */
            }            
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError("Fehler beim Lesen der Datei: " + ex.Message);
         }
      }      
   }
}
