/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 28.03.2013
 * Zeit: 14:54
 * 
 */
using System;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace FilmInfo
{
   /// <summary>
   /// Description of HtmlExport.
   /// </summary>
   public class HtmlExport
   {
      //private DataGridView _DataGridView;
      private List<FilmInfo> _FilmInfoList = new List<FilmInfo>();
      private SortedDictionary<int, FilmInfo> _FilmInfoDict 
                     = new SortedDictionary<int, FilmInfo>();
      private string _HtmlExportListTemplateFilename;
      private string _HtmlExportDetailTemplateFilename;
      private FilmInfoManager _FilmInfoManager;

      
      // Icon(Image-Bilder)-Maße
      private const double _Ratio = 0.679012;
      private int _MoviePosterThumbnailWidth;
      private int _MoviePosterThumbnailHeight;
      private const double _MoviePosterHalfSizeFactor = 0.45;
      
      // für Zufallssortierung
      private Random _Random = new Random();
      
      private string[] _SearchGenreArray;
      private string[] _SearchTagArray;
      
      private IComparer<FilmInfo> _filmInfoSorter = new FilmInfoSorter();
      
      public HtmlExport(DataGridView dgv, 
         string HtmlExportListTemplateFilename,
         string HtmlExportDetailTemplateFilename,
         FilmInfoManager filmInfoManager,
         string[] SearchGenreArray,
         string[] SearchTagArray)
      {
         _HtmlExportListTemplateFilename = HtmlExportListTemplateFilename;
         _HtmlExportDetailTemplateFilename = HtmlExportDetailTemplateFilename;
         _FilmInfoManager = filmInfoManager;
         _SearchGenreArray = SearchGenreArray;
         _SearchTagArray = SearchTagArray;
         CopyDataGridViewToFilmInfoList(dgv);
      }
      
      public void SortFilmInfoList(string ColumnName, bool SortAscending)
      {
         FilmInfoSorter fis = _filmInfoSorter as FilmInfoSorter;
         fis.ColumnName = ColumnName;
         fis.Ascending = SortAscending;
         if (ColumnName == "Zufall") {
            // Für Zufallsauswahl vorbereiten 
            // (jedem FilmInfo-Objekt eine zufällige Nummer geben)
            foreach (FilmInfo fi in _FilmInfoList) {
               fi.RandomSequenceId = _Random.Next(1, 100000);
            }
         }
         _FilmInfoList.Sort(_filmInfoSorter);            
      }
      
      private void CopyDataGridViewToFilmInfoList(DataGridView dgv)
      {
         _FilmInfoList.Clear();
         int i = 0;
         foreach (DataGridViewRow dgvr in dgv.Rows) {
            FilmInfo fi = dgvr.Tag as FilmInfo;
            fi.MovieId = i;
            _FilmInfoList.Add(fi);            
            _FilmInfoDict.Add(fi.MovieId, fi);
                              
            i++;
         }
      }
      
      public string GetTagListComplete()
      {
         return string.Join(",", GetTagListCompleteArray());
      }

      public string[] GetVolumeLabelListCompleteArray()
      {
         List<string> VolumeLabelList = new List<string>();
         VolumeLabelList.Add("");
         foreach (FilmInfo fi in _FilmInfoList) {
            if (fi.VolumeLabel == null)
               continue;
         
            if (!VolumeLabelList.Contains(fi.VolumeLabel))
               VolumeLabelList.Add(fi.VolumeLabel);
                           
         }

         return VolumeLabelList.ToArray();
      }
      
      public string[] GetTagListCompleteArray()
      {
         SortedList<string, bool> TagList = new SortedList<string, bool>();
         foreach (FilmInfo fi in _FilmInfoList) {
            if (fi.TagList == null || fi.TagList.Count == 0)
               continue;
            
            foreach (KeyValuePair<string, bool> tagItem in fi.TagList) {
               if (!TagList.ContainsKey(tagItem.Key))
                  TagList.Add(tagItem.Key, true);
            }               
         }
         
         List<string> stringArray = new List<string>();
         
         
         stringArray.Add("");
         
         foreach (KeyValuePair<string, bool> tagItem in TagList) {
            if (!string.IsNullOrEmpty(tagItem.Key)
                && tagItem.Key != "\n") {
               stringArray.Add(tagItem.Key);               
            }
         }
         
         return stringArray.ToArray();
      }

      public FilmInfo GetFilmInfoByMovieId(int MovieId)
      {
         if (_FilmInfoDict.ContainsKey(MovieId)) {
            return _FilmInfoDict[MovieId];
         }
          
         return null;         
      }
      
      public void ToggleFilmInfoArchived(int MovieId)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null)
            fi.Archived = !fi.Archived;
      }
      
      public void ToggleFilmInfoFavourite(int MovieId)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null)
            fi.Favourite = !fi.Favourite;
      }
      
      public void ToggleFilmInfoWatched(int MovieId)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null) {            
            if (fi.Watched == new DateTime(0))
               fi.Watched = DateTime.Now;
            else
               fi.Watched = new DateTime(0);
         }
      }
      
      private string GetSearchForm(SearchInfo searchInfo)
      {
         StringBuilder html = new StringBuilder();
         html.AppendLine("<a id=\"topofsite\"></a>");
         html.AppendLine("<form id=\"SearchForm\" action=\"search.htm\" method=\"get\" accept-charset=\"UTF-8\">");
         
         html.AppendLine("<div class=\"searchform\">");
         html.AppendLine("<table style=\"table-layout:fixed\">");
         html.AppendLine("<tr>");
         
         // Titel
         html.AppendLine(GetHtmlInputTextCode("Titel", "Title", searchInfo.Title, 20));
         
         // Rating
         html.AppendLine(GetHtmlInputTextCode("Min. Rating", "Rating", searchInfo.Rating, 1, true));
         
         // Actor
         html.AppendLine(GetHtmlInputTextCode("Actor", "Actor", searchInfo.Actor, 20));


         // 2. Zeile         
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");              
         
         // Director
         html.AppendLine(GetHtmlInputTextCode("Director", "Director", searchInfo.Director, 20));
          
         
         // SD/HD
         html.AppendLine(GetHtmlOptionCode("SD / HD", "SdHd", searchInfo.SdHd,
            new string[] {
               "",
               "SD",
               "720p",
               "1080p"
            })); 
         
         // Channels
         html.AppendLine(GetHtmlOptionCode("5.1", "Channels", searchInfo.Channels,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));
         
         // 3. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");   
         
         // 3D
         html.AppendLine(GetHtmlOptionCode("3D", "Is3D", searchInfo.Is3D,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));

         // Favourite
         html.AppendLine(GetHtmlOptionCode("Favoriten", "Favourite", searchInfo.Favourite,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));
         
         // Watched
         html.AppendLine(GetHtmlOptionCode("Gesehen", "Watched", searchInfo.Watched,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));

         // 4. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");            
         
         // Genre
         html.AppendLine(GetHtmlOptionCode("Genre", "Genre", searchInfo.Genre, _SearchGenreArray));
         
         // max. Filmlänge
         html.AppendLine(GetHtmlInputTextCode("Länge (min)", "MaxDuration", searchInfo.MaxDuration, 3, true));

         // Archieved
         html.AppendLine(GetHtmlOptionCode("Archiviert", "Archieved", searchInfo.Archieved,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));            
         
         // 5. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");                  
         
         // Only Movies
         html.AppendLine(GetHtmlOptionCode("Nur Filme", "OnlyMovies", searchInfo.OnlyMovies,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));                

         // Jahr
         html.AppendLine(GetHtmlInputTextCode("ab Jahr", "MinYear", searchInfo.MinYear, 4, true));
         
         // ShowIconDetails
         html.AppendLine(GetHtmlOptionCode("Zeige Details", "ShowIconDetails", searchInfo.ShowIconDetails,
            new string[] {
               "Checked",
               "Unchecked"
            }));             
         
         // 6. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");   
         
         
         // SortColumnName
         html.AppendLine(GetHtmlOptionCode("Sortierung", "SortByColumnName", searchInfo.SortByColumnName,
            new string[] {
               "Hinzugefuegt",
               "Titel",
               "Jahr", 
               "Bewertung",
               "Dauer",
               "Gesehen",
               "Zufall"
            }));

         html.AppendLine(GetHtmlOptionCode("Aufsteigend", "SortAscending", searchInfo.SortAscending,
            new string[] {
               "Checked",
               "Unchecked"
            }));                
         
         // Rahmen
         html.AppendLine(GetHtmlOptionCode("Rahmen", "ShowFrame", searchInfo.ShowFrame,
            new string[] { "Ja", "Nein" }));
         
         // 6. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");
         
         // MaxItemCount
         html.AppendLine(GetHtmlInputTextCode("Max. Anzahl Ergebnisse", "MaxItemCount", searchInfo.MaxItemCount, 5, true));
         
         // HalfSize
         html.AppendLine(GetHtmlOptionCode("Halbe Größe", "HalfSize", searchInfo.HalfSize,
            new string[] {
               "Unchecked",
               "Checked"
            }));                
         // OnlyWithMyRating
         html.AppendLine(GetHtmlOptionCode("Nur von mir bewertete Filme", "OnlyWithMyRating", searchInfo.OnlyWithMyRating,
            new string[] {
               "",
               "Checked",
               "Unchecked"
            }));                         
            
            
         
         // 7. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");         
         
         
         html.AppendLine(GetHtmlOptionCode("Tag", "Tag", searchInfo.Tag, GetTagListCompleteArray()));
         html.AppendLine(GetHtmlOptionCode("Disk", "Disk", searchInfo.VolumeLabel, GetVolumeLabelListCompleteArray()));

         // Leer-Suchfeld(er):         
         html.AppendLine("<th></th><td></td>");
         
         // 8. Zeile
         html.AppendLine("</tr>");
         html.AppendLine("<tr>");        
                
         
         // Buttons
         // Ausblenden-Button
         html.AppendLine("<th><input id=\"BtnSearchFormFadeOut\" class=\"btnSearch\" style=\"width:100px\" type=\"button\" value=\"Ausblenden\"></th><td></td>");
         // Leeren-Button
         html.AppendLine("<th><input id=\"BtnReset\" class=\"btnSearch\" style=\"width:100px\" type=\"button\" value=\"Zurücksetzen\"></th><td></td>");
         // Suche-Button
         html.AppendLine("<th><input class=\"btnSearch\" style=\"width:80px\" type=\"submit\" value=\"Suchen\"></th><td></td>");         
         
         /*
         // Buttons
         // Ausblenden-Button
         html.AppendLine("<th></th><td class=\"customTD\"><input id=\"BtnSearchFormFadeOut\" class=\"btnSearch\" style=\"width:100px\" type=\"button\" value=\"Ausblenden\"></td>");
         // Leeren-Button
         html.AppendLine("<th></th><td class=\"customTD\"><input id=\"BtnReset\" class=\"btnSearch\" style=\"width:100px\" type=\"button\" value=\"Zurücksetzen\"></td>");
         // Suche-Button
         html.AppendLine("<th></th><td class=\"customTD\"><input class=\"btnSearch\" style=\"width:80px\" type=\"submit\" value=\"Suchen\"></td>");
         */
         
         html.AppendLine("</tr>");
         html.AppendLine("</table>");
         html.AppendLine("</div>");
         html.AppendLine("</form>");
         
         return html.ToString();
      }
     
      private string GetHtmlInputTextCode(string Label, string Name, string Default, int Size)
      {
         return GetHtmlInputTextCode(Label, Name, Default, Size, false);
      }
      
      private string GetHtmlInputTextCode(string Label, string Name, string Default, int Size, bool IsNumber)
      {
         StringBuilder sb = new StringBuilder();
         
         string inputPattern = "";
         if (IsNumber)
            inputPattern = "pattern=\"[0-9]*\"";
            
         
         sb.Append("<th>" + Label + "</th>");
         sb.Append("<td class=\"customTD\"><input name=\"" + Name
         + "\" type=\"search\" " + inputPattern + " size=\"" + Size.ToString()
         + "\" value=\"" + Default// System.Web.HttpUtility.HtmlEncode(
         + "\" maxlength=\"" + Size.ToString() + "\"></td>");         
         
         return sb.ToString();
      }
      
      /// <summary>
      /// Liefert ein HTML-Option-String (Auswahlbox) mit den angegebenen Parametern zurück
      /// </summary>
      /// <param name="Label"></param>Für den Nutzer sichtbare Auswahlbox-Bezeichnung
      /// <param name="Name"></param>Interner Name (für Parameter-Auswertung)
      /// <param name="Selected"></param>Evtl. vorausgewählter Wert
      /// <param name="Values"></param>Werte-Array, mit den auswählbaren Werten.
      /// <returns></returns>
      private string GetHtmlOptionCode(string Label, string Name, string Selected, 
         string[] Values)
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("<th>" + Label + "</th>");
         sb.AppendLine("<td class=\"customTD\"><select name=\"" + Name + "\" size=\"1\">");
         
         foreach (string val in Values) {
            string TextToShow = val;
            if (TextToShow == "Checked")
               TextToShow = "Ja";
            if (TextToShow == "Unchecked")
               TextToShow = "Nein";
            if (TextToShow == "")
               TextToShow = "&nbsp;";
            
            if (val == Selected)
               sb.AppendLine("<option selected value=\"" + val + "\">" + TextToShow + "</option>");
            else
               sb.AppendLine("<option value=\"" + val + "\">" + TextToShow + "</option>");
         }
         sb.AppendLine("</select></td>");
         return sb.ToString();
      }
      
      private string GetFooter()
      {
         StringBuilder html = new StringBuilder();
         
         html.AppendLine("<a href=\"#result\"><div class=\"footer\">Nach oben</div></a>");
         
         html.AppendLine("<br /><br /><a href=\"execute\" target=\"_blank\">"
         + "<div class=\"executeLink\">Programm ausfuehren</div>"
         + "</a><br />");

         html.AppendLine("<a href=\"#\" >"
         + "<div id=\"shutdownPcLink\" class=\"executeLink\">Computer herunterfahren</div>"
         + "</a><br /><br />");
         
         return html.ToString();
      }
      
      private string GetImgCssClassNameByResolution(FilmInfo fi, string ShowFrame)
      {
         if (!ShowFrame.Equals("Ja"))
            return "imageicon_None";
         
         string resolution = fi.GetResolution();
         string imgCssClassName = "imageicon_SD";
         if (resolution == "3D")
            imgCssClassName = "imageicon_3D";
         if (resolution == "1080p")
            imgCssClassName = "imageicon_1080";
         if (resolution == "720p")
            imgCssClassName = "imageicon_720";    
         
         return imgCssClassName;
      }

      public string AjaxSetArchived(int MovieId, bool Checked)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null) {
            fi.Archived = Checked;
            return "{\"NewCheckedState\": " + fi.Archived.ToString().ToLower() + "}";
         }
         
         return "";
      }
      
      public string AjaxSetFavourite(int MovieId, bool Checked)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null) {
            fi.Favourite = Checked;
            return "{\"NewCheckedState\": " + fi.Favourite.ToString().ToLower() + "}";
         }
         
         return "";
      }
      
      public string AjaxSetTagList(int MovieId, string TagList)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null) {
            fi.SetTagList(TagList);         
            return "{\"NewTagList\": \"" + fi.GetTagStringList() + "\"}";
         }
         
         return "";
      }
      
      public string AjaxSetWatched(int MovieId, bool Checked)
      {
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         if (fi != null) {            
            if (Checked)
               fi.Watched = DateTime.Now;
            else
               fi.Watched = new DateTime(0);
            
            return "{\"Datum\": \"" + fi.Watched.ToShortDateString()
            + "\", \"NewCheckedState\": " + Checked.ToString().ToLower() + "}";            
         }        

         return "";         
      }

      /// <summary>
      /// Liefert den kompletten HTML-Code für die Auslieferung an den Client zurück
      /// </summary>
      /// <param name="searchInfo"></param>Suchparameter, gemäß denen die Anzeige eingeschränkt werden soll
      /// <returns></returns>HTML-Code
      public string GetHtmlExportDetail(int MovieId)
      {  
         if (!File.Exists(_HtmlExportDetailTemplateFilename))
            return "Das Template " + _HtmlExportDetailTemplateFilename + " konnte nicht gefunden werden.";
         
         string htmlContent = File.ReadAllText(_HtmlExportDetailTemplateFilename);
         FilmInfo fi = GetFilmInfoByMovieId(MovieId);
         
         if (fi == null) {
            return "Die MovieId " + MovieId.ToString() + " konnte nicht gefunden werden.";
         }

         // Umgebungsvariablen für JavaScript initialisieren
         htmlContent = htmlContent.Replace("%MovieId%", fi.MovieId.ToString());
         htmlContent = htmlContent.Replace("%WatchedDate%", fi.Watched.ToShortDateString());
         htmlContent = htmlContent.Replace("%Watched%", (fi.Watched != new DateTime(0)).ToString().ToLower());         
         htmlContent = htmlContent.Replace("%Archived%", fi.Archived.ToString().ToLower());
         htmlContent = htmlContent.Replace("%Favourite%", fi.Favourite.ToString().ToLower());
         
         // Page "Infos"
         htmlContent = htmlContent.Replace("%Title%", fi.GetHtmlTitle());
         string originalTitle = "";
         if (!fi.TitleEnglish.Equals("") && !fi.TitleEnglish.Equals(fi.Title))
            originalTitle = "\"" + fi.TitleEnglish + "\", ";
         htmlContent = htmlContent.Replace("%Year%",  originalTitle + fi.Country + " " + fi.Year.ToString());
         htmlContent = htmlContent.Replace("%Length%", fi.GetVideoDurationInMinutes().ToString());
         htmlContent = htmlContent.Replace("%GenreList%", fi.GetGenresStringList());
         htmlContent = htmlContent.Replace("%TagList%", fi.GetTagStringList());
         htmlContent = htmlContent.Replace("%TagListComplete%", GetTagListComplete());
         
         htmlContent = htmlContent.Replace("%ReleaseDate%", fi.ReleaseDate2);
         htmlContent = htmlContent.Replace("%ImdbUrl%", fi.ImdbUrl);
         
         // IMDB-Link/Rating
         Color ratingColor = GetRatingColor(fi.RatingImdb);
         htmlContent = htmlContent.Replace("%RatingImdbColor%", 
            ratingColor.ToArgb().ToString("X").Substring(2));
         htmlContent = htmlContent.Replace("%RatingImdb%", fi.RatingImdb.ToString());
         htmlContent = htmlContent.Replace("%RatingImdbCount%", fi.RatingImdbCount.ToString());

         // MetaCritic-Link/Rating
         ratingColor = GetRatingColor(fi.RatingMetaCritic);
         htmlContent = htmlContent.Replace("%RatingMetaCriticColor%", 
            ratingColor.ToArgb().ToString("X").Substring(2));         
         htmlContent = htmlContent.Replace("%RatingMetaCritic%", fi.RatingMetaCritic.ToString());
         htmlContent = htmlContent.Replace("%MetaCriticSearchString%", GetMetacriticSearchString(fi));
         
         htmlContent = htmlContent.Replace("%AmazonSearchString%", GetAmazonSearchString(fi));
         htmlContent = htmlContent.Replace("%YoutubeSearchString%", GetYouTubeSearchString(fi));
         htmlContent = htmlContent.Replace("%TheMovieDbSearchString%", GetTheMovieDbSearchString(fi));
         htmlContent = htmlContent.Replace("%RottenTomatoesSearchString%", GetRottenTomatoesSearchString(fi));
         htmlContent = htmlContent.Replace("%GoogleSearchString%", GetGoogleSearchString(fi));
         htmlContent = htmlContent.Replace("%BingSearchString%", GetBingSearchString(fi));
      
         htmlContent = htmlContent.Replace("%PlotSimple%", fi.PlotSimple);
         htmlContent = htmlContent.Replace("%ActorsList%", fi.GetActorsStringList());
         htmlContent = htmlContent.Replace("%DirectorsList%", fi.GetDirectorsStringList());

         
         
         // Page "Poster"         
         if (fi.IsMoviePosterAvailable()) {
            //_FilmInfoManager.GetPosterImage(fi, 0, 0, true, false);
            _FilmInfoManager.GetPosterImage(fi, 0, 0, true, true);
            
            htmlContent = htmlContent.Replace("%PosterImage%", fi.ImdbId + "_large.jpg");
            htmlContent = htmlContent.Replace("%PosterLarge%", fi.ImdbId + "_large.jpg");
         } else {
            htmlContent = htmlContent.Replace("%PosterImage%", "NotAvailable.jpg");
            htmlContent = htmlContent.Replace("%PosterLarge%", "NotAvailable.jpg");
         }         
         
         htmlContent = htmlContent.Replace("%ImageCssClass%", GetImgCssClassNameByResolution(fi, "Nein"));
         
         
         // Page "Datei"
         htmlContent = htmlContent.Replace("%VideoInfo%", fi.GetVideoInfo());
         htmlContent = htmlContent.Replace("%AudioInfo%", fi.GetAudioInfo());
         
         string DtsIcon = "";
         if (fi.GetGermanAudioFormat().ToLower().Contains("dts"))
            DtsIcon = "<li><div align=\"center\"><img alt=\"DTS\" src=\"icons/dts.png\" border=\"0\" height=\"32\"></div></li>";         
         htmlContent = htmlContent.Replace("%DtsIcon%", DtsIcon);
         
         string DolbyIcon = "";
         if (fi.GetGermanAudioFormat().ToLower().Contains("ac3"))
            DolbyIcon = "<li><div align=\"center\"><img alt=\"Dolby Digital 5.1\" src=\"icons/Dolby_Digital_5_1.png\" border=\"0\" height=\"32\"></div></li>";
         htmlContent = htmlContent.Replace("%DolbyIcon%", DolbyIcon);
         
         htmlContent = htmlContent.Replace("%FileSize%", fi.GetFilesizeInGb());
         htmlContent = htmlContent.Replace("%FileDate%", fi.GetFileDate());
         htmlContent = htmlContent.Replace("%AddDate%", fi.GetAddDate());         
         htmlContent = htmlContent.Replace("%FileVideoDuration%", fi.GetVideoDurationAsString());
         htmlContent = htmlContent.Replace("%Filename%", fi.Filename);
         
         if (File.Exists(fi.Filename))
            htmlContent = htmlContent.Replace("%FileExistsIcon%", "<div align=\"center\"><img alt=\"Yes\" src=\"icons/checkmark.png\" border=\"0\" height=\"32\"></div>");
         else
            htmlContent = htmlContent.Replace("%FileExistsIcon%", "<div align=\"center\"><img alt=\"No\" src=\"icons/cross.png\" border=\"0\" height=\"32\"></div>");
         
         
         string volumeLabel = fi.VolumeLabel;
         if (string.IsNullOrEmpty(volumeLabel))
            volumeLabel = "???";   
         htmlContent = htmlContent.Replace("%FileDisk%", volumeLabel);
         
         // Resolution-Icons
         string ResolutionIcons = "";
         string Icon3D = "<img alt=\"(3D)\" src=\"icons/3D.png\" border=\"0\" height=\"32\"> ";
         string IconHd1080p = "<img alt=\"(1080p)\" src=\"icons/hd_1080p.png\" border=\"0\" height=\"32\">";
         string IconHd720p = "<img alt=\"(720p)\" src=\"icons/hd_720p.png\" border=\"0\" height=\"32\">";
         
         if (fi.Is3D())
            ResolutionIcons = ResolutionIcons + Icon3D;
         // Hd-Icon anzeigen
         if (fi.GetVideoWidth() >= 1920)
            ResolutionIcons = ResolutionIcons + IconHd1080p;
         else if (fi.GetVideoWidth() >= 1280)
            ResolutionIcons = ResolutionIcons + IconHd720p; 
         
         if (!string.IsNullOrEmpty(ResolutionIcons))
            ResolutionIcons = "<li><div align=\"center\">" + ResolutionIcons + "</div></li>";
         htmlContent = htmlContent.Replace("%ResolutionIcons%", ResolutionIcons);
         
         
         // MovieThumbnailer
         string ThumbnailLink = "";
         
         if (File.Exists(fi.Filename))
            ThumbnailLink = "<li><a href=\"mtn.htm?file=" + fi.Filename + "\" target=\"_blank\"><img src=\"icons/Preview.png\" alt=\"preview\" class=\"ui-li-icon\">Thumbnails</a></li>";         
         htmlContent = htmlContent.Replace("%MovieThumbnails%", ThumbnailLink);
         
         
         return htmlContent;         
      }
      
            
      /// <summary>
      /// Liefert den kompletten HTML-Code für die Auslieferung an den Client zurück
      /// </summary>
      /// <param name="searchInfo"></param>Suchparameter, gemäß denen die Anzeige eingeschränkt werden soll
      /// <returns></returns>HTML-Code
      public string GetHtmlExportIconList(SearchInfo searchInfo)
      {
         
         
         if (!File.Exists(_HtmlExportListTemplateFilename))
            return "Das Template " + _HtmlExportListTemplateFilename + " konnte nicht gefunden werden.";
         
         string htmlContent = File.ReadAllText(_HtmlExportListTemplateFilename);
         
         if (searchInfo.ShowFrame.Equals("Ja")) {
            _MoviePosterThumbnailWidth = 110; // bis 04.03.2016: 110                
         } else {
            // ohne Rahmen (3px links, 3px rechts) -> 6px mehr Platz für Breite!
            _MoviePosterThumbnailWidth = 116;
         }
         
         _MoviePosterThumbnailHeight = (int)Math.Round(_MoviePosterThumbnailWidth / _Ratio, 0);
         
         // Halbe Größe anzeigen?
         double MoviePosterFactor = 1;
         if (!string.IsNullOrEmpty(searchInfo.HalfSize)) {
            if (searchInfo.HalfSize == "Checked") {
               MoviePosterFactor = _MoviePosterHalfSizeFactor;
               searchInfo.ShowIconDetails = "Unchecked";
            }
         }                
         
         htmlContent = htmlContent.Replace("%FormData%", GetSearchForm(searchInfo));
         
         //Ansichts-Breite anhand der darzustellenden Spalten berechnen
         htmlContent = htmlContent.Replace("%ViewPort%", "<meta name=\"viewport\" content=\"user-scalable=yes, width=device-width\">");
         /*
         htmlContent = htmlContent.Replace("%TD_Width%", ((int)(MoviePosterFactor * _MoviePosterThumbnailWidth + m_ImageOffsetPixel)).ToString());
         htmlContent = htmlContent.Replace("%TD_Height%", ((int)(MoviePosterFactor * _MoviePosterThumbnailHeight)).ToString());
         */
         
         // für CSS-Angabe:   
         int topIconDetails = _MoviePosterThumbnailHeight - 19;
         if (!searchInfo.ShowFrame.Equals("Ja"))
            topIconDetails = topIconDetails - 6;         
         htmlContent = htmlContent.Replace("%TopIconDetails%", topIconDetails.ToString());
         
         
         StringBuilder htmlResultTable = new StringBuilder();
              
         int itemNumber = 0;
         
         // FilmInfo-Items durchlaufen
         foreach (FilmInfo fi in _FilmInfoList) {
            if (!fi.IsVisible(searchInfo))
               continue;
            
            // Poster-Bilder
            string imgLinkHtml;
            if (fi.IsMoviePosterAvailable()) {
               _FilmInfoManager.GetPosterImage(fi, 0, 0, true, false);
               imgLinkHtml = fi.ImdbId + ".jpg";
            } else
               imgLinkHtml = "NotAvailable.jpg";
              
      
            // ggf. IconDetails ermitteln (so dass sie "schön" aussehen)
            string iconDetails = "";
            if (searchInfo.ShowIconDetails == "Checked") {
               // Details am unteren Rand des Movie-Posters
               string bottomImgDetails = "<br />" + GetBottonImgDetails(fi);
               
               // Jahr muss hier ausgewertet werden, weil es zwischen FilmName und Jahr
               // keinen Zeilenumbruch geben soll
               if (fi.Year != 0) {
                  bottomImgDetails = " (" + fi.Year.ToString() + ")"
                  + bottomImgDetails;
               }
               bottomImgDetails = "\n               <span class=\"iconTextSmall\">" + bottomImgDetails + "</span>";
               
               // Details am oberen Rand des Movie-Posters
               string topImgDetails = GetTopImgDetails(fi);
               string topImgRating = GetTopImgRatingHtml(fi);
               if (!string.IsNullOrEmpty(topImgDetails))
                  topImgDetails = topImgRating
                  + "\n            <span class=\"h3span\">"
                  + topImgDetails
                  + "&nbsp;</span>\n";    
               
                  
               iconDetails = "         <div class=\"h3\">\n            "
               + topImgDetails
               + "         </div>\n"
               + "         <div class=\"h2\">\n"
               + "            <span class=\"h2span\">" + fi.GetHtmlTitle()/*+ "&nbsp;"*/
               + bottomImgDetails + "\n"
               + "            </span>\n"
               + "         </div>\n";
            }
  
            imgLinkHtml = "<a href=\"detail.html?MovieId=" + fi.MovieId.ToString() + "\" target=\"_blank\">\n"
            + "   <div class=\"image\">\n"
            + "      <img class=\""
                           // farbiger MoviePoster-Rahmen:
            + GetImgCssClassNameByResolution(fi, searchInfo.ShowFrame) + "\" width=\""
            + ((int)(MoviePosterFactor * _MoviePosterThumbnailWidth)).ToString()
            + "\" height=\"" + ((int)(MoviePosterFactor * _MoviePosterThumbnailHeight)).ToString()
            + "\" src=\"" + imgLinkHtml + "\" alt=\"\" />\n"
            + iconDetails
            + "   </div>\n</a>\n";            
            
            htmlResultTable.AppendLine(imgLinkHtml);
            
            itemNumber++;
            
            if (!string.IsNullOrEmpty(searchInfo.MaxItemCount)) {
               if (itemNumber >= int.Parse(searchInfo.MaxItemCount))
                  break;
            }            
         }
         
         htmlResultTable.AppendLine("<br>");
         htmlResultTable.AppendLine("<div class=\"iconText\">");
         htmlResultTable.AppendLine("Es wurden " + itemNumber.ToString() + " Einträge gefunden.");
         htmlResultTable.AppendLine("</div>");
         
         htmlContent = htmlContent.Replace("%ResultTable%", htmlResultTable.ToString());
         htmlContent = htmlContent.Replace("%Footer%", GetFooter());
         
         return htmlContent;
      }
      
      private string GetTopImgRatingHtml(FilmInfo fi)
      {
         string details = "";
         
         if (fi.RatingImdb > 0)
            details = "<div class=\"rating_bar\"><div style=\"width:"
            + (fi.RatingImdb * 10).ToString("0")
            + "%\"></div></div>";
         
         return details;
      }
      
      private string GetTopImgDetails(FilmInfo fi)
      {
         string details = "";
         bool moreThanOneItem = false;         
         
         if (!string.IsNullOrEmpty(fi.GetResolution())) {
            details += fi.GetResolution();
            moreThanOneItem = true;
         }    

         string germanAudioFormat = fi.GetGermanAudioFormat();
         if (!string.IsNullOrEmpty(germanAudioFormat)) {
            if (moreThanOneItem) {
               details += "/";
            }
            
            details += germanAudioFormat;
            moreThanOneItem = true;
         }             
         
         /*
         if (fi.Rating > 0)
         {
            if (moreThanOneItem)
            {
               details += " ";
            }
            details += "I:" + fi.Rating.ToString().Replace(",", ".") ;   
            moreThanOneItem = true;
         }  
         
         if (!string.IsNullOrEmpty(fi.RatingMetaCritic) 
             && fi.RatingMetaCritic != "-")
         {
            if (moreThanOneItem)
            {
               details += " ";
            }
            details += "M:" + fi.RatingMetaCritic.Replace("/100", "");
            moreThanOneItem = true;
         }
         
         
         if (fi.MyImdbRating > 0)
         {
            if (moreThanOneItem)
            {
               details += " ";
            }
            details += "P:" + fi.MyImdbRating.ToString();
            moreThanOneItem = true;
         }         
         */
         
         return details;
      }
      
      private string GetBottonImgDetails(FilmInfo fi)
      {
         string details = "";
         bool moreThanOneItem = false;

         int dur = fi.GetVideoDurationInMinutes();
         if (dur != 0) {
            if (moreThanOneItem) {
               details += ", ";
            }
            details += dur.ToString() + "min";  
            moreThanOneItem = true;
         }   
         string genres = fi.GetGenresStringList();
         if (!string.IsNullOrEmpty(genres)) {
            if (moreThanOneItem) {
               details += ", ";
            }
            details += genres;  
            moreThanOneItem = true;
         }               
         
         return details;
      }
      
      
      public string GetBingSearchString(FilmInfo fi)
      {
         string searchString = @"https://www.bing.com/search?q={QUERY}+Film";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.GetTitleOnly()));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
      	
         return searchString;
      }
      
      public string GetGoogleSearchString(FilmInfo fi)
      {
         string searchString = @"https://www.google.de/search?q={QUERY}+Film";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.GetTitleOnly()));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
      	
         return searchString;
      }
      
      public string GetAmazonSearchString(FilmInfo fi)
      {
         string searchString = @"http://www.amazon.de/s/ref=nb_sb_noss_2?__mk_de_DE=%C3%85M%C3%85Z%C3%95%C3%91&url=search-alias%3Daps&field-keywords={QUERY}";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.GetTitleOnly()));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
      	
         return searchString;
      }
      
      public string GetYouTubeSearchString(FilmInfo fi)
      {
         string searchString = @"http://www.youtube.com/results?search_query={QUERY}&amp;oq={QUERY}";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.GetTitleOnly() + " trailer"));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query + " trailer"));
      	
         return searchString;
      }
      
      public string GetMetacriticSearchString(FilmInfo fi)
      {  
         string searchString = @"http://www.metacritic.com/search/movie/{QUERY}/results";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.TitleEnglish));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
      	
         return searchString;
      }     

      public string GetTheMovieDbSearchString(FilmInfo fi)
      {  
         string searchString = @"https://www.themoviedb.org/search?query={QUERY}&language=de";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.GetTitleOnly()));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
         
         return searchString;
      }      
      
      
      public string GetRottenTomatoesSearchString(FilmInfo fi)
      {  
         string searchString = @"https://www.rottentomatoes.com/search/?search={QUERY}";
         if (!string.IsNullOrEmpty(fi.GetTitleOnly()))
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.TitleEnglish));
         else
            searchString = searchString.Replace("{QUERY}", System.Web.HttpUtility.UrlEncode(fi.Query));
      	
         return searchString;
      }
      
      private string GetRatingHtml(FilmInfo fi)
      {  
         Color ratingColor = GetRatingColor(fi.RatingImdb);
         string htmlColor = ratingColor.ToArgb().ToString("X").Substring(2);
         
         return 
            "<span style=\"background-color:#" + htmlColor + ";\"><b>"
         + fi.RatingImdb.ToString() + "</b> bei "
         + fi.RatingImdbCount.ToString()
         + " Votes</span><br>"
         + "<b>" + fi.ratingMetaCriticString + "</b> bei MetaCritic";
      }
      
      private Color GetRatingColor(decimal Rating)
      {  
         if (Rating <= 0)
            return Color.White;
         
         double relevance = (double)Rating * 10;
         
         // farbig markieren (Farbverlauf von grün nach rot)
         // berechnet mit LGS: a + b * x = y ; x = relevance, y = Wunschwert (für rel. 0 und 100)
         int red = (int)(255.0 - 0.82 * relevance);
         int green = (int)(99.0 + 1.56 * relevance);
         int blue = (int)(71.0 - 0.24 * relevance);
         return Color.FromArgb(red, green, blue);         
      }
   }
   
   
   
   public class FilmInfoSorter : IComparer<FilmInfo>
   {
      private int _Factor = -1;
      
      public bool Ascending {
         get { 
            return _Factor == 1;
         }
         set { 
            if (value)
               _Factor = 1;
            else
               _Factor = -1;
         }
      }
      
      private int _ColNr;
      public string ColumnName {
         get { return _ColNr.ToString(); }
         set { 
            _ColNr = 1;
            if (value.Equals("Hinzugefuegt"))
               _ColNr = 1;
            if (value.Equals("Titel"))
               _ColNr = 2;
            if (value.Equals("Jahr"))
               _ColNr = 3;
            if (value.Equals("Bewertung"))
               _ColNr = 4;          
            if (value.Equals("Dauer"))
               _ColNr = 5;                
            if (value.Equals("Gesehen"))
               _ColNr = 6;                            
            if (value.Equals("Zufall")) {
               _ColNr = 7;                    
            }
               
         }
      }
         
      public int Compare(FilmInfo x, FilmInfo y)
      {         
         int result = 0;
         
         if (_ColNr == 1) {
            result = _Factor * x.DateTimeAdded.CompareTo(y.DateTimeAdded);
            if (result == 0)
               result = _Factor * x.FileLastWriteDate.CompareTo(y.FileLastWriteDate);
         } else if (_ColNr == 2)
            result = _Factor * x.Title.CompareTo(y.Title);
         else if (_ColNr == 3) {
            result = _Factor * x.Year.CompareTo(y.Year);
            if (result == 0
                && !string.IsNullOrEmpty(x.ReleaseDate2)
                && !string.IsNullOrEmpty(y.ReleaseDate2))
               result = _Factor * x.ReleaseDate2.CompareTo(y.ReleaseDate2);
         } else if (_ColNr == 4)
            result = _Factor * x.RatingImdb.CompareTo(y.RatingImdb);
         else if (_ColNr == 5) {
            string DurationX = "0";
            string DurationY = "0";
            
            // Runtime (lt. IMDB) NULL?
            if (!string.IsNullOrEmpty(x.Runtime))
               DurationX = x.Runtime.Replace("min", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(y.Runtime))
               DurationY = y.Runtime.Replace("min", "").Replace(" ", "");
            
            // Duration NULL?
            if (DurationX.Equals("0") && !string.IsNullOrEmpty(x.Duration))
               DurationX = x.Duration;
            if (DurationY.Equals("0") && !string.IsNullOrEmpty(y.Duration))
               DurationY = y.Duration;
      
            int durX = 0;
            int durY = 0;
            int.TryParse(DurationX, out durX);
            int.TryParse(DurationY, out durY);      
            
            result = durX.CompareTo(durY);
            if (result == 0)
               result = _Factor * x.DateTimeAdded.CompareTo(y.DateTimeAdded);               
         } else if (_ColNr == 6) {
            result = _Factor * x.Watched.CompareTo(y.Watched);
            if (result == 0
                && !string.IsNullOrEmpty(x.ReleaseDate2)
                && !string.IsNullOrEmpty(y.ReleaseDate2)) {
               result = _Factor * x.ReleaseDate2.CompareTo(y.ReleaseDate2);
            }
         } else if (_ColNr == 7) {
            result = x.RandomSequenceId.CompareTo(y.RandomSequenceId);
         }
            
         
         return result;
      }
   }
}

