/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 26.02.2013
 * Zeit: 09:15
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FilmInfo
{
   /// <summary>
   /// Description of FilmInfo.
   /// </summary>
   [Serializable()]   
   public class FilmInfo
   {
      // Daten aus Internet (Imdb)
      public decimal RatingImdb;      
      public int RatingImdbCount;
      
      public string ratingMetaCriticString;         
      public decimal RatingMetaCritic {
         get { 
            if (string.IsNullOrEmpty(ratingMetaCriticString)
                || ratingMetaCriticString == "-")
               return 0;
            
            decimal rating = 0;
            if (decimal.TryParse(ratingMetaCriticString.Replace("/100", ""), out rating))
            {
               return rating / 10;
            }
            
            return -1; 
         }
      }
      
      private string m_Title = "";
      
      public string Title 
      {
         get { 
            string Title = "";
            // falls in IMDB nix gefunden wurde -> ersatzweise Query anzeigen
            if (string.IsNullOrEmpty(m_Title))
            {
               Title = QueryOriginal;
            }
            else
            {
               Title = m_Title + " (" + QueryOriginal.Replace(".", " ") + ")";
            }
            return Title;
         }
         set 
         {
            m_Title = value;
         }
      }
      
      // IMDB-Infos
      public string TitleEnglish = "";
      public string Runtime;
      public string PosterUrl;
         
      public string PosterUrlLarge {
         get {
            if (string.IsNullOrEmpty(PosterUrl))
               return "";
            
            int PosterWidth = 600;
            
            string regex = @"(http://ia.media-imdb.com/images/M/\w+@+)\.";
            Regex r = new Regex(regex, 
                                RegexOptions.Compiled
                                | RegexOptions.IgnoreCase 
                                | RegexOptions.Singleline);
            
            Match m = r.Match(PosterUrl);       
            
            if (m.Success)
            {
               return m.Groups[1].Value + "._SX" + PosterWidth.ToString() + ".jpg";
            }            
            
            // Fallback, wenn RegEx nicht anschlägt:
            return PosterUrl;
         }
      }
      public string ImdbUrl;
      public string ImdbId = "";
      public string PlotSimple;
      public string Type;
      public string ContentRating; // Altersbeschränkung
      public int Year;
      public int ReleaseDate;
      public string ReleaseDate2;
      public string Country;
      public string Query;
      public string QueryOriginal;
      public long QueryDuration;
      public long ProcessDuration;
      public DateTime DownloadTimestamp;
      public List<string> Actors = new List<string>();
      public List<string> Genres = new List<string>();
      public List<string> Directors = new List<string>();
      public List<string> Creators = new List<string>();
      public string RetrieveState;
      public int MyImdbRating;
      
      // für HTML-Export-Zufallssortierung:
      public int RandomSequenceId;
      
      // Dateibasiert
      public string Filename;
      public string VolumeLabel;
      public long Filesize = 0;
      public DateTime FileLastWriteDate;
      public string Duration = "";
      public string VideoBitrate;
      public string VideoFormat;
      public string VideoDimension;
      public string VideoFramerate;
      public string AudioFormatList;
      public string AudioLanguageList;
      public string AudioChannels;
      
      // Nutzerbasiert
      public DateTime DateTimeAdded;
      public DateTime Watched;
      public bool Favourite = false;      
      public bool Archived = false;
      public int MovieId = 0;
      private SortedDictionary<string, bool> _TagList;
         
      public SortedDictionary<string, bool> TagList {
         get { return _TagList; }
      }
      
      public FilmInfo()
      {
         
      }
      
      public int GetVideoWidth()
      {
         if (!string.IsNullOrEmpty(VideoDimension))
         {
            string[] dimension = VideoDimension.Replace(" ", "").Split('x');
            if (dimension.Length == 2)
            {
               int videoWidth = 0;
               if (int.TryParse(dimension[0], out videoWidth))
               {
                  return videoWidth;
               }
            }
         }
         
         return 0;
      }
         
      public string GetVideoInfo()
      {
         if (string.IsNullOrEmpty(Duration))
            return "-";
         
         if (string.IsNullOrEmpty(VideoBitrate))
            VideoBitrate = "0";
         
         return VideoFormat.Replace("Visual", "") + " (" 
                + VideoDimension + ", "
                + Math.Round(int.Parse(VideoBitrate) / 1024.0 / 1024.0, 1).ToString("0.0") 
                + " MB/s, " + VideoFramerate + " fps)";         
      }
      
      public string GetAudioInfo()
      {
         if (string.IsNullOrEmpty(AudioLanguageList))
            AudioLanguageList = "?";
               
         if (string.IsNullOrEmpty(AudioFormatList))
            return "";
         
         StringBuilder sb = new StringBuilder();
         sb.Append(AudioFormatList.Replace(" Audio", ""));
         if (!string.IsNullOrEmpty(AudioLanguageList))
             sb.Append(", " + AudioLanguageList);
         if (!string.IsNullOrEmpty(AudioChannels))
             sb.Append(", " + AudioChannels + " Ch");

         return sb.ToString();
      }
      
      public string GetFileDetailInfo(bool UseHtmlLineBreaks)
      {
         StringBuilder sb = new StringBuilder();
         
         if (!string.IsNullOrEmpty(Duration))
         {                 
            // sb.AppendLine("Dateiinformationen");
            sb.AppendLine("Laufzeit: " + GetVideoDurationAsString());
            sb.AppendLine("Video: " + GetVideoInfo());            
            sb.Append("Audio: " + GetAudioInfo());
            
            sb.AppendLine();
         }
         if (Filesize > 0)
         {
            sb.AppendLine("Größe: " 
                          + GetFilesizeInGb() 
                          + ", Datum: " + GetFileDate());
         }

         
         if (UseHtmlLineBreaks)
            return sb.ToString().Replace(System.Environment.NewLine, "<br>");
         else
            return sb.ToString();
      }
      
      public string GetFilesizeInGb()
      {
         return Math.Round(Filesize / 1024.0 / 1024.0 / 1024.0, 1).ToString("0.0") + " GB";
      }
      
      public string GetFileDate()
      {
         return FileLastWriteDate.ToShortDateString();
      }

      public string GetAddDate()
      {
         return DateTimeAdded.ToShortDateString();
      }      
      
      public bool ExistsTag(string Tag)
      {
         if (_TagList == null)
            return false;
         return _TagList.ContainsKey(Tag);
      }
      
      public void SetTagList(string Tags)
      {
         if (_TagList == null)
            _TagList = new SortedDictionary<string, bool>();
         
         _TagList.Clear();
         // String-Liste immer auf "Komma" normalisieren)
         Tags = Tags.Replace(" ", ",").Replace(";", ",").Replace("-", "").Replace("\n", "");
         foreach(string tag in Tags.Split(','))
         {
            if (string.IsNullOrEmpty(tag))
               continue;
            
            if (!_TagList.ContainsKey(tag))
               _TagList.Add(tag, true);
         }
      }
      
      public string GetTagStringList()
      {
         if (_TagList == null || _TagList.Count == 0)
            return "-";
         
         StringBuilder result = new StringBuilder();
         foreach(KeyValuePair<string, bool> kvp in _TagList)
         {
            if (!string.IsNullOrEmpty(kvp.Key) 
                && kvp.Key != "\n")
            {
               result.Append(kvp.Key + ", ");               
            }
            
         }
         
         return result.ToString().Substring(0, result.ToString().Length - 2);
      }
      
      public string GetGermanAudioFormat()
      {
         if (string.IsNullOrEmpty(Duration))
            return "";
         
         // komische Formatierung der AudioChannels korrigieren
         string tmpAudioChannels = AudioChannels.Replace(" ", "").Replace("7/6", "6");
         
         string[] languages = AudioLanguageList.Replace(" ", "").Split('/');
         string[] formats = AudioFormatList.Replace(" ", "").Split('/');
         string[] channels = tmpAudioChannels.Replace(" ", "").Split(',');
         
         
         int idxGerman = 0;
         for(int i = 0; i < languages.Length; i++)
         {
            if (languages[i].ToLower().IndexOf("german") != -1)
            {
               idxGerman = i;
               break;
            }
         }
         
         if (idxGerman + 1 > channels.Length
             || idxGerman + 1 > formats.Length)
         {
            return "???";
         }
         
         string result = "";
         int channel = -1;
         
         if (int.TryParse(channels[idxGerman], out channel))
         {
            if (channel <= 2)
               result = channel.ToString() + ".0";
            else if (channel == 6)
               result = formats[idxGerman].Replace("-", "") + " 5.1";
            else
               result = channel.ToString() + "Ch-" + formats[idxGerman].Replace("-", "") + ")";
         }
         else
            result = "???";
         
         // formats[idxGerman] + "(" + channels[idxGerman] + " Ch)";
         return result;
      }
      
      public string GetVideoDurationAsString()
      {
         return GetVideoDurationInMinutes().ToString() + " min";
      }
      
      public int GetVideoDurationInMinutes()
      {
         long duration = 0;
         if (!long.TryParse(Duration, out duration))
            return 0;
         
         TimeSpan ts = TimeSpan.FromMilliseconds(duration);
         
         // Ergebnis in der Form "mm min" ermitteln
         int result = (int)ts.TotalMinutes;
        
         return result;         
      }
      
      public bool IsExistingAsFile()
      {
         if (File.Exists(Filename) || Directory.Exists(Filename))
            return true;
         else
            return false;
      }
      
      public bool Is3D()
      {
         string filenameToCheck = Path.GetFileNameWithoutExtension(Filename);
         int idx = Filename.ToLower().IndexOf("3d");
         if (idx != -1)
         {
            int idx2 = Filename.ToLower().IndexOf("ac3d");
            // ac3d kommt nicht vor:
            if (idx2 == -1)
               return true;
            
            // ac3d kommt vor, beinhaltet aber nicht das "3d"
            // (z.B. (Avatar.AC3D.3D.x264.1080p.mkv)
            if (idx2 + 2 != idx)
            {
               return true;
            }
         }
         return false;
      }
      
      public string GetGenresStringList()
      {         
         return string.Join(", ", Genres.ToArray());
      }
      
      public string GetActorsStringList()
      {
         return string.Join(", ", Actors.ToArray());
      }
      
      public string GetDirectorsStringList()
      {
         return string.Join(", ", Directors.ToArray());
      }      
      
      public string GetTitleOnly()
      {
         return m_Title;
      }
      
      public bool IsMoviePosterAvailable()
      {
         return !string.IsNullOrEmpty(ImdbId) 
                && !string.IsNullOrEmpty(PosterUrl);
      }
      
      public string GetHtmlTitle()
      {
         string Title = "";
         // falls in IMDB nix gefunden wurde -> ersatzweise Query anzeigen
         if (string.IsNullOrEmpty(m_Title))
         {
            Title = QueryOriginal.Replace(".", " ");
         }
         else
         {
            Title = m_Title;
         }
         return Title;
      }
      
      public bool IsVisible(SearchInfo searchInfo)
      {
         bool visible = true;
         int parseResult;

         if (!string.IsNullOrEmpty(searchInfo.Title))
         {
            if (this.Query.ToLower().IndexOf(searchInfo.Title.ToLower()) == -1
                && this.Title.ToLower().IndexOf(searchInfo.Title.ToLower()) == -1
                && this.QueryOriginal.ToLower().Replace(".", " ").IndexOf(searchInfo.Title.ToLower().Replace(".", " ")) == -1)
                return false;
         }

         if (!string.IsNullOrEmpty(searchInfo.Tag))
         {
            if (!this.ExistsTag(searchInfo.Tag))
               return false;
         }
         
         if (!string.IsNullOrEmpty(searchInfo.Rating)
             && int.TryParse(searchInfo.Rating, out parseResult))
         {
            if (this.RatingImdb < parseResult)
                return false;
         }
         
         if (!string.IsNullOrEmpty(searchInfo.MinYear)
             && int.TryParse(searchInfo.MinYear, out parseResult))
         {
            if (this.Year < parseResult)
                return false;
         }         
         
         if (!string.IsNullOrEmpty(searchInfo.Genre))
         {
            if (string.Join(",", this.Genres.ToArray()).IndexOf(searchInfo.Genre) == -1)
               return false;
         }
         
         if (!string.IsNullOrEmpty(searchInfo.VolumeLabel))
         {
            
            if (searchInfo.VolumeLabel != this.VolumeLabel)
                return false;
         }         
         
         if (!string.IsNullOrEmpty(searchInfo.Actor))
         {
            string actorList = this.GetActorsStringList().ToLower();
            if (actorList.IndexOf(searchInfo.Actor.ToLower()) == -1)
               return false;
         }
         
         if (!string.IsNullOrEmpty(searchInfo.Director))
         {
            string directorList = this.GetDirectorsStringList().ToLower();
            if (directorList.IndexOf(searchInfo.Director.ToLower()) == -1)
               return false;
         }

         if (!string.IsNullOrEmpty(searchInfo.Is3D))
         {
            if (searchInfo.Is3D == "Checked" && !this.Is3D())
               return false;
            if (searchInfo.Is3D == "Unchecked" && this.Is3D())
               return false;
         }
         
         if (!string.IsNullOrEmpty(searchInfo.OnlyMovies))
         {
            if (searchInfo.OnlyMovies == "Checked" 
                && Path.GetExtension(this.Filename).Length != 4)
               return false;
            if (searchInfo.OnlyMovies == "Unchecked" 
                && Path.GetExtension(this.Filename).Length == 4)
               return false;
         }         

         if (!string.IsNullOrEmpty(searchInfo.Channels))
         {
            if (string.IsNullOrEmpty(this.AudioFormatList))
            {
               if (searchInfo.Channels == "Checked")
                  return false;
            }
            else
            {
               if (searchInfo.Channels == "Checked"
                   && !this.AudioChannels.Contains("6"))
                  return false;
               
               if (searchInfo.Channels == "Unchecked"
                   && this.AudioChannels.Contains("6"))
                  return false;                    
            }
         }            
         
         if (!string.IsNullOrEmpty(searchInfo.Watched))
         {
            if (searchInfo.Watched == "Checked" && this.Watched == new DateTime(0))
               return false;
            if (searchInfo.Watched == "Unchecked" && this.Watched != new DateTime(0))
               return false;                  
         }            
         
         if (!string.IsNullOrEmpty(searchInfo.Favourite))
         {
            if (searchInfo.Favourite == "Checked" && !this.Favourite)
               return false;
            if (searchInfo.Favourite == "Unchecked" && this.Favourite)
               return false;                  
         }
         
         if (!string.IsNullOrEmpty(searchInfo.Archieved))
         {
            if (searchInfo.Archieved == "Checked" && !this.Archived)
               return false;
            if (searchInfo.Archieved == "Unchecked" && this.Archived)
               return false;                  
         }     
         
         if (!string.IsNullOrEmpty(searchInfo.OnlyWithMyRating))
         {
            if (searchInfo.OnlyWithMyRating == "Checked" && this.MyImdbRating <= 0)
               return false;
            if (searchInfo.OnlyWithMyRating == "Unchecked" && this.MyImdbRating > 0)
               return false;                  
         }                  
         
         
         if (!string.IsNullOrEmpty(searchInfo.Unknown))
         {
            if (searchInfo.Unknown == "Checked" && !string.IsNullOrEmpty(this.GetTitleOnly()))
               return false;
            if (searchInfo.Unknown == "Unchecked" && string.IsNullOrEmpty(this.GetTitleOnly()))
               return false;                  
         }
         
         if (!string.IsNullOrEmpty(searchInfo.SdHd))
         {
            if (searchInfo.SdHd == "1080p" && this.GetVideoWidth() < 1920)
               return false;
            if (searchInfo.SdHd == "720p" && this.GetVideoWidth() < 1280)
               return false;
            if (searchInfo.SdHd == "SD" && this.GetVideoWidth() >= 1280)
               return false;            
         }    
         
         int result;
         if (!int.TryParse(searchInfo.MaxDuration, out result))
             result = 0;
         if (result > 0)
         {
            if (this.GetVideoDurationInMinutes() == 0
                || this.GetVideoDurationInMinutes() > result)
               return false;
         }             
         
         return visible;
      }
      
      public string GetResolution()
      {
         if (this.Is3D())
            return "3D";
         if (this.GetVideoWidth() >= 1920)
            return "1080p";
         if (this.GetVideoWidth() >= 1280)
            return "720p";               

         return "SD";
      }
      
      public static string GetCsvStringHeader()
      {
         return   "Title;TitleEnglish;Query;QueryOrig;DateTimeAdded;Archived;Watched;"
                + "ImdbId;ImdbUrl;AudioChannels;AudioFormatList;AudioLangList;DownloadTimestamp;"
                + "Favourite;Filename;FileLastWriteDate;Filesize;Genres;Resolution;Duration;"
                + "VideoWidth;Is3D;PosterUrl;Rating;RatingCount;RatingMetaCritic;ReleaseDate;"
                + "Runtime;Type;VideoBitrate;VideoDimension;VideoFormat;VideoFramerate;VolumeLabel;"
                + "Year;MyImdbRating";
      }

      public string GetCsvString()
      {
         string separator = ";";
         StringBuilder sb = new StringBuilder();
         sb.Append(this.Title); sb.Append(separator);
         sb.Append(this.TitleEnglish); sb.Append(separator);
         sb.Append(this.Query); sb.Append(separator);
         sb.Append(this.QueryOriginal); sb.Append(separator);
         sb.Append(this.DateTimeAdded); sb.Append(separator);         
         sb.Append(this.Archived); sb.Append(separator);
         sb.Append(this.Watched.ToString()); sb.Append(separator);
         sb.Append(this.ImdbId); sb.Append(separator);
         sb.Append(this.ImdbUrl); sb.Append(separator);
         sb.Append(this.AudioChannels); sb.Append(separator);
         sb.Append(this.AudioFormatList); sb.Append(separator);
         sb.Append(this.AudioLanguageList); sb.Append(separator);
         sb.Append(this.DownloadTimestamp); sb.Append(separator);
         sb.Append(this.Favourite); sb.Append(separator);
         sb.Append(this.Filename); sb.Append(separator);
         sb.Append(this.FileLastWriteDate); sb.Append(separator);
         sb.Append(this.Filesize); sb.Append(separator);
         sb.Append(this.GetGenresStringList()); sb.Append(separator);
         sb.Append(this.GetResolution()); sb.Append(separator);
         sb.Append(this.GetVideoDurationAsString()); sb.Append(separator);
         sb.Append(this.GetVideoWidth()); sb.Append(separator);
         sb.Append(this.Is3D()); sb.Append(separator);
         sb.Append(this.PosterUrl); sb.Append(separator);
         sb.Append(this.RatingImdb); sb.Append(separator);    
         sb.Append(this.RatingImdbCount); sb.Append(separator);    
         sb.Append(this.ratingMetaCriticString); sb.Append(separator);    
         sb.Append(this.ReleaseDate2); sb.Append(separator);    
         sb.Append(this.Runtime); sb.Append(separator);    
         sb.Append(this.Type); sb.Append(separator);    
         sb.Append(this.VideoBitrate); sb.Append(separator);    
         sb.Append(this.VideoDimension); sb.Append(separator);    
         sb.Append(this.VideoFormat); sb.Append(separator);    
         sb.Append(this.VideoFramerate); sb.Append(separator);    
         sb.Append(this.VolumeLabel); sb.Append(separator);    
         sb.Append(this.Year); sb.Append(separator);    
         sb.Append(this.MyImdbRating); sb.Append(separator);    
         
         
         return sb.ToString();
      }
   }
}
