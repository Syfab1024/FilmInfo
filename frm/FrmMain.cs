/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 26.02.2013
 * Zeit: 08:11
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Syntela.Controls;

using Helper;


namespace FilmInfo
{
   /// <summary>
   /// Description of MainForm.
   /// </summary>
   public partial class FrmMain : System.Windows.Forms.Form
   {
      private FilmInfo _selectedFilmInfo = null;
      private Dictionary<string, string> _AkaList = new Dictionary<string, string>();

      private FilmInfoHttpServer _FilmInfoHttpServer;
      private HtmlExport _HtmlExport;
      private FilmInfoManager _FilmInfoManager;
      private FrmHttpServerStatistic _FrmHttpServerStatistic;
      public SearchInfo GridViewSearchInfo = new SearchInfo();
      public SynSettings MySettings = new SynSettings();
      
      private string _AssemblyExecutionPath;
      private string _CachePath;
      private string _IconPath;
      private string _BackupPath;
      private string _AkaListFilename;
      private string _FilmListFilename;
      private string _HtmlExportListTemplateFilename;
      private string _HtmlExportDetailTemplateFilename;
      private string _MovieThumbnailerExeFilename;
      private string _MassScanDirListFilename;
      private bool _UseProxy = false;
      private string _ExecuteFilename = "";
     
      
      public FrmMain()
      {
         InitializeComponent();
         Initialize();         
      }
      
      private void Initialize()
      {
         try
         {
            CreatePathNames();
            LoadAkaList();
            ReadSettings();
            _FilmInfoManager = new FilmInfoManager(_CachePath, 
                                                   _AkaList,
                                                   _MovieThumbnailerExeFilename,
                                                   _UseProxy);

            CreateGridViewColumns();

            cbSearchGenre.Items.Add("");  

            CreateBackup();

            LoadFilmInfoList();
            FillSearchInfo();
            FilterGridViewBySearchInfo();
            ShowDetails();

            _HtmlExport = new HtmlExport(dgvMain, 
                                         _HtmlExportListTemplateFilename, 
                                         _HtmlExportDetailTemplateFilename,
                                         _FilmInfoManager,
                                         GetGenreStringArray(),
                                         GetTagStringArray());
            
            _FilmInfoHttpServer = new FilmInfoHttpServer(_CachePath, 
                                                         _AssemblyExecutionPath,
                                                         _HtmlExport, 
                                                         _FilmInfoManager,
                                                         _ExecuteFilename);
            _FilmInfoHttpServer.StartServer();
            btnHttpServerOpen.Text = "Seite im Browser öffnen ("
                                     + _FilmInfoHttpServer.ServerAdress + ")";
            _FrmHttpServerStatistic = new FrmHttpServerStatistic(_FilmInfoHttpServer);
            
            
            if (!IsMovieThumbnailerAvailable())
            {
               MsgBox.ShowCmnError("MovieThumbnailer wurde nicht gefunden ("
                                   + _MovieThumbnailerExeFilename 
                                   + "). Daher können keine Vorschaubilder angezeigt werden.\n"
                                   + "Download: http://moviethumbnail.sourceforge.net/");
            }
            btnScanDirBulk.Enabled = File.Exists(_MassScanDirListFilename);
            
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }
      
      private void WriteSettings()
      {
         MySettings.Write("UseProxy", _UseProxy);
         MySettings.Write("ExecuteFilename", _ExecuteFilename);
         
      }
      
      private void ReadSettings()
      {
         _UseProxy = MySettings.Read("UseProxy", false);
         _ExecuteFilename = MySettings.Read("ExecuteFilename", "");
      }
      
      private bool IsMovieThumbnailerAvailable()
      {
         return File.Exists(_MovieThumbnailerExeFilename);
      }
      
      private void CreatePathNames()
      {
         // Pfadnamen erstellen
         _AssemblyExecutionPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
         _CachePath = Path.Combine(_AssemblyExecutionPath, "Cache");
         _IconPath = Path.Combine(_AssemblyExecutionPath, "icons");
         
         if (!Directory.Exists(_CachePath))
             Directory.CreateDirectory(_CachePath);
         
         _BackupPath = Path.Combine(_AssemblyExecutionPath, "Backup");
         if (!Directory.Exists(_BackupPath))
             Directory.CreateDirectory(_BackupPath);
         
         _AkaListFilename = Path.Combine(_AssemblyExecutionPath, "AkaList.csv");
         _MassScanDirListFilename = Path.Combine(_AssemblyExecutionPath, "directories.txt");
         _FilmListFilename = Path.Combine(_AssemblyExecutionPath, "Filmlist.dat");
         _HtmlExportListTemplateFilename = Path.Combine(_AssemblyExecutionPath, "HtmlTemplate_List.html");
         _HtmlExportDetailTemplateFilename = Path.Combine(_AssemblyExecutionPath, "HtmlTemplate_Detail.html");
         _MovieThumbnailerExeFilename = Path.Combine(_AssemblyExecutionPath, "mtn-200808a-win32");
         _MovieThumbnailerExeFilename = Path.Combine(_MovieThumbnailerExeFilename, "mtn.exe");
      }
      
      private void CreateBackup()
      {
         string backupFile = Path.Combine(_BackupPath, 
                                          Path.GetFileName(_FilmListFilename)
                                          + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak");
         File.Copy(_FilmListFilename, backupFile);
         CleanupBackup();
      }
      
      /// <summary>
      /// Löscht alle Backups bis auf die 20 aktuellsten Dateien
      /// </summary>
      private void CleanupBackup()
      {
         string[] filenames = System.IO.Directory.GetFiles(_BackupPath);
         
         if (filenames.Length <= 20)
            return;
         
         // CreationTimes ermitteln
         DateTime[] creationTimes = new DateTime[filenames.Length];
         for(int i = 0; i < filenames.Length; i++)
         {
            creationTimes[i] = new FileInfo(filenames[i]).CreationTime;
         }
         
         // Filename-Array nach CreationTime aufsteigend sortieren
         Array.Sort(creationTimes, filenames);
         
         // alle bis auf die 20 neuesten Dateien löschen
         for(int i = 0; i < filenames.Length - 20; i++)
         {
            File.Delete(filenames[i]);
         }
      }
      
      private void LoadFilmInfoList()
      {
         List<FilmInfo> l = LoadFilmInfoListFromFile();
         if (l == null)
            l = LoadFilmInfoListFromCompressedFile();
         
         foreach(FilmInfo fi in l)
         {
            AddFilmInfoToGridView(fi);
         }
      }    
      
      private List<FilmInfo> LoadFilmInfoListFromFile()
      {
         List<FilmInfo> l = null;// = new List<FilmInfo>();
         
         if (!File.Exists(_FilmListFilename))
            return l;
         
         Stream stream = File.Open(_FilmListFilename, FileMode.Open, FileAccess.Read);
         BinaryFormatter bf = new BinaryFormatter();
         
         try
         {  
            l = bf.Deserialize(stream) as List<FilmInfo>;
         }         
         catch(Exception ex)
         {
            // GZip-Compression-Fehler ignorieren:
            if (!ex.Message.Contains("kein gültiges binäres Format"))               
               MsgBox.ShowCmnError(ex.Message);
         }
         finally
         {
            stream.Close();
         }
         
         return l;
      }
      
      private List<FilmInfo> LoadFilmInfoListFromCompressedFile()
      {
         List<FilmInfo> l = new List<FilmInfo>();
         
         if (!File.Exists(_FilmListFilename))
            return l;
         
         Stream stream = File.Open(_FilmListFilename, FileMode.Open, FileAccess.Read);
         GZipStream zip = new GZipStream(stream, CompressionMode.Decompress);

         BinaryFormatter bf = new BinaryFormatter();
         
         try
         {  
            l = bf.Deserialize(zip) as List<FilmInfo>;
         }
         
         catch(Exception ex)
         {            
            MsgBox.ShowCmnError(ex.Message);
         }
         finally
         {
            stream.Close();
            zip.Close();
         }
         
         return l;
      }
      
      private void SaveFilmInfoListToCompressedFile()
      {
         try
         {
            List<FilmInfo> l = new List<FilmInfo>();
            foreach(DataGridViewRow dgvr in dgvMain.Rows)
            {
               l.Add(dgvr.Tag as FilmInfo);
            }
            
            Stream stream = File.Open(_FilmListFilename, FileMode.Create);
            GZipStream zip = new GZipStream(stream, CompressionMode.Compress);
            BinaryFormatter bf = new BinaryFormatter();
                        
            bf.Serialize(zip, l);
            zip.Close();
            stream.Close();            
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }
      
      private void LoadAkaList()
      {
         _AkaList.Clear();
         if (!File.Exists(_AkaListFilename))
            return;
         
         string[] lines = File.ReadAllLines(_AkaListFilename);
         if (!lines[0].Equals("Filename;Query"))
         {
            MsgBox.ShowCmnError("Die AkaList " + _AkaListFilename 
                                + " beginnt nicht mit 'Filename;Query'. Abbruch" );
            return;
         }
         
         foreach(string s in lines)
         {
            if (!s.Equals("Filename;Query"))
            {
               string[] splittedLine = s.Split(';');
               if (splittedLine.Length != 2)
               {
                  MsgBox.ShowCmnError("Die AkaList " + _AkaListFilename 
                                      + " enthält eine ungültige Zeile: "
                                      + s + ". Abbruch" );
                  return;                  
               }
               string key = splittedLine[0];
               string val = splittedLine[1];
               
               if (!_AkaList.ContainsKey(key))
                  _AkaList.Add(key, val);
            }
         }
      }
      
      private void SaveAkaList()
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("Filename;Query");
         foreach(KeyValuePair<string, string> kvp in _AkaList)
         {
            sb.AppendLine(kvp.Key + ";" + kvp.Value);
         }
         
         File.WriteAllText(_AkaListFilename, sb.ToString());
      }      
      
      private void ClearJSonCache()
      {
         try
         {
            foreach(string filename in Directory.GetFiles(_CachePath, "*.json"))
            {
               File.Delete(filename);
            }            
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }
      
      private void ClearImageCache()
      {
         try
         {
            foreach(string filename in Directory.GetFiles(_CachePath, "*.jpg"))
            {
               if (filename.ToLower().IndexOf("notavailable") == -1)
                  File.Delete(filename);
            }            
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }      
      
      void BtnFindClick(object sender, EventArgs e)
      {
         FilmInfo fi = _FilmInfoManager.GetMovieInfo(tbQuery.Text, false, null);
         if (!string.IsNullOrEmpty(fi.RetrieveState))
         {
            MsgBox.ShowCmnError(fi.RetrieveState);
            return;
         }
         
         AddFilmInfoToGridView(fi);
      }
      
      private void FillFilmList(List<string> FilenameList)
      {
         pbMain.Maximum = FilenameList.Count;
         pbMain.Value = 0;
         
         foreach(string filename in FilenameList)
         {
            GetFilmInfoAndAddToGridView(filename);

            Application.DoEvents();
            pbMain.Value++;
            lStatus.Text = pbMain.Value.ToString() + " / " + FilenameList.Count.ToString();
         }
      }
      
      private void GetFilmInfoAndAddToGridView(string Filename)
      {
         if (!IsValidFilename(Filename))
            return;
         
         string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Filename);
         FilmInfo fiFound = GetFilmInfoWithSameFilename(fileNameWithoutExtension);
         if (fiFound == null)
         {
            FilmInfo fi = _FilmInfoManager.GetMovieInfo(fileNameWithoutExtension, false, null);
            fi.Filename = Filename;
            fi.DateTimeAdded = DateTime.Now;
            _FilmInfoManager.UpdateFilmInfoByFile(fi);
            AddFilmInfoToGridView(fi);               
         }
         else
         {
            // FilmInfo-Objekt existiert bereits
            // --> nur Dateinamen aktualisieren, restliche Informationen bleiben bestehen
            fiFound.Filename = Filename;
            _FilmInfoManager.UpdateFilmInfoByFile(fiFound);
         }
      }
      
      private bool IsValidFilename(string Filename)
      {
         string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Filename);
         if (fileNameWithoutExtension.ToLower() == "thumbs")
            return false;
         
         string extension = Path.GetExtension(Filename);
         if (extension.ToLower() != ".avi"
             && extension.ToLower() != ".mkv"
             && extension.ToLower() != ".mp4"
             && extension.ToLower() != ".mpg"
             && extension.ToLower() != "" // für VerzeichnisScan
             && extension.ToLower() != ".flv")
            return false;
         
         return true;         
      }
      
      private FilmInfo GetFilmInfoWithSameFilename(string FileNameWithoutExtension)
      {
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {
            FilmInfo fi = dgvr.Tag as FilmInfo;
            if (!string.IsNullOrEmpty(fi.Filename)
                && Path.GetFileNameWithoutExtension(fi.Filename) == FileNameWithoutExtension)
                return fi;
         }
         
         return null;
      }
      
      private void ShowDetails()
      {
         _selectedFilmInfo = null;
         if (dgvMain.SelectedRows.Count > 0 
            && dgvMain.CurrentCell != null)
            _selectedFilmInfo = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Tag as FilmInfo;
         
         if (_selectedFilmInfo == null)
            _selectedFilmInfo = new FilmInfo();
         
         
         pbPoster.Image = _FilmInfoManager.GetPosterImage(_selectedFilmInfo, -1, -1, false, false);
         lRating.Text = _selectedFilmInfo.RatingImdb.ToString() 
                        + " Punkte (" + _selectedFilmInfo.RatingImdbCount.ToString() 
                        + " Nutzer), MetaCritic: " + _selectedFilmInfo.ratingMetaCriticString;
         lYearRuntime.Text = "VÖ: " + _selectedFilmInfo.ReleaseDate2
                             + ", " + _selectedFilmInfo.Runtime + ", " + _selectedFilmInfo.Country;
         lPlot.Text = _selectedFilmInfo.PlotSimple;
         lActors.Text = string.Join(", ", _selectedFilmInfo.Actors.ToArray());
         lDirectors.Text = string.Join(", ", _selectedFilmInfo.Directors.ToArray());
         lGenres.Text = string.Join(", ", _selectedFilmInfo.Genres.ToArray());
         llImdbUrl.Text = _selectedFilmInfo.GetTitleOnly();
         
         if (_selectedFilmInfo.IsExistingAsFile())
         {
            lFilename.Text = "Datei/Ordner anzeigen (" + _selectedFilmInfo.Filename + ")" ;
            lFilename.Enabled = true;
            lShowMovieThumbnail.Enabled = true;
         }
         else
         {
            lFilename.Text = "[Datei/Ordner nicht verfügbar]";
            lFilename.Enabled = false;
            lShowMovieThumbnail.Enabled = false;
         }
            
         
         if (_selectedFilmInfo.Query != "")
         {
            lImdbSearch.Text = "Imdb-Filmsuche";
            lAmazonSearch.Text = "Amazon-Filmsuche";
         }            
         else
         {
            lImdbSearch.Text = "";
            lAmazonSearch.Text = "";
         }
            
         
         if (!string.IsNullOrEmpty(_selectedFilmInfo.VolumeLabel))
            lDriveLabel.Text = "Datenträger: " + _selectedFilmInfo.VolumeLabel;
         else
            lDriveLabel.Text = "Datenträger: ???";
         
         
         if (_selectedFilmInfo.Watched == new DateTime(0))
         {
            cbWatched.Checked = false;
            cbWatched.Text = "Film gesehen";
         }
         else
         {
            cbWatched.Checked = true;
            cbWatched.Text = "Film gesehen: " 
                             + _selectedFilmInfo.Watched.ToShortDateString();
         }
         
         if (_selectedFilmInfo.MyImdbRating <= 0)
            myRating.Text = "Sie haben den Film noch nicht bei IMDB bewertet.";
         else
            myRating.Text = "Ihre IMDB-Bewertung: " + _selectedFilmInfo.MyImdbRating.ToString();
               
          
         cbFavourite.Checked = _selectedFilmInfo.Favourite;
         cbArchieved.Checked = _selectedFilmInfo.Archived;
         
         
         lFileDetails.Text = _selectedFilmInfo.GetFileDetailInfo(false);
         tbTags.Text = _selectedFilmInfo.GetTagStringList();
      }

      void BtnSearchPathClick(object sender, EventArgs e)
      {
         List<string> l = new List<string>();
         if (folderDialog.ShowDialog() != DialogResult.OK)
            return;
         
         l.AddRange(Directory.GetFiles(folderDialog.SelectedPath));

         FillFilmList(l);      	
      }
      
      void BtnClearListClick(object sender, EventArgs e)
      {
      	dgvMain.Rows.Clear();
      }
      
      void DgvMainSelectionChanged(object sender, EventArgs e)
      {
      	ShowDetails();
      }
      
      void LlImdbUrlLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            Process.Start(_selectedFilmInfo.ImdbUrl);
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }           
      }
      
      void LFilenameLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         if (!_selectedFilmInfo.IsExistingAsFile())
            return;
         
         try
         {
            Process.Start("explorer.exe", "/select, " + _selectedFilmInfo.Filename);
         }
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }      	
      }
      
      void LQueryLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
         	string imdbSearchString = @"http://www.imdb.com/find?q={QUERY}&s=tt";
         	imdbSearchString = imdbSearchString.Replace("{QUERY}", _selectedFilmInfo.Query);
         	imdbSearchString = imdbSearchString.Replace("ae", "ä");
         	imdbSearchString = imdbSearchString.Replace("oe", "ö");
         	imdbSearchString = imdbSearchString.Replace("ue", "ü");
         	Process.Start(imdbSearchString);
      	}
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }    
      }
      
      void BtnAkaChangeClick(object sender, EventArgs e)
      {
         frmInputBox f = new frmInputBox();
         string oldAka = _selectedFilmInfo.Query;
         if (_AkaList.ContainsKey(_selectedFilmInfo.QueryOriginal))
         {
            oldAka = _AkaList[_selectedFilmInfo.QueryOriginal];
         }            
            
         string newAka = f.ShowForm("Neuen Namen festlegen",
                                    "Unter welchen Namen (oder IMDB-ID) soll der Film in der IMDB gesucht werden?\n" +
                                    "Hinweis: Wenn es den Film nicht in der IMDB-Datenbank gibt, geben sie hier '-' ein.",
                                    oldAka, "Name/IMDB-ID: ", false);
         if (_selectedFilmInfo.Query == newAka || newAka == "")
            return;

         if (_AkaList.ContainsKey(_selectedFilmInfo.QueryOriginal))
         {
            _AkaList[_selectedFilmInfo.QueryOriginal] = newAka;
         }
         else
         {
            _AkaList.Add(_selectedFilmInfo.QueryOriginal, newAka);
         }

         this.Cursor = Cursors.WaitCursor;
         SaveAkaList();

         FilmInfo newFilmInfo = _FilmInfoManager.GetMovieInfo(_selectedFilmInfo.QueryOriginal, true, null);
         CopyFilmInfoFileDetails(_selectedFilmInfo, newFilmInfo);
         
         _selectedFilmInfo = newFilmInfo;
         UpdateDataGridViewByFilmInfo(dgvMain.SelectedRows[0], _selectedFilmInfo);
         ShowDetails();
         RefreshWebServerData();
         
         this.Cursor = Cursors.Default;
         MsgBox.ShowInfo("Der Aka-Name wurde erfolgreich gespeichert.");
      }
      
      private void CopyFilmInfoFileDetails(FilmInfo Src, FilmInfo Dest)
      {
         // Dateiinformationen
         Dest.Filename = Src.Filename;
         Dest.VolumeLabel = Src.VolumeLabel;
         Dest.Filesize = Src.Filesize;
         Dest.FileLastWriteDate = Src.FileLastWriteDate;
         Dest.Duration = Src.Duration;
         Dest.VideoBitrate = Src.VideoBitrate;
         Dest.VideoFormat = Src.VideoFormat;
         Dest.VideoDimension = Src.VideoDimension;
         Dest.VideoFramerate = Src.VideoFramerate;
         Dest.AudioFormatList = Src.AudioFormatList;
         Dest.AudioChannels = Src.AudioChannels;
         Dest.AudioLanguageList = Src.AudioLanguageList;
         
         // Nutzerinformationen
         Dest.DateTimeAdded = Src.DateTimeAdded;
         Dest.Watched = Src.Watched;
         Dest.Favourite = Src.Favourite;
         Dest.Archived = Src.Archived;
      }
      
      /// <summary>
      /// Aktualisierung der DataGridView-Zeile anhand der FilmInfo-Instanz
      /// Wird für jede in der DataGridView angezeigten Zeile ausgeführt
      /// </summary>
      /// <param name="Row"></param>
      /// <param name="filmInfo"></param>
      private void UpdateDataGridViewByFilmInfo(DataGridViewRow Row, FilmInfo filmInfo)
      {
         Row.Cells["Titel und Dateiname"].Value = filmInfo.Title;
         Row.Cells["Rating"].Value = filmInfo.RatingImdb;
         
         if (!string.IsNullOrEmpty(filmInfo.Duration) && filmInfo.Duration != "0")
            Row.Cells["Laufzeit"].Value = filmInfo.GetVideoDurationAsString();
         else
            Row.Cells["Laufzeit"].Value = filmInfo.Runtime;
         Row.Cells["Jahr"].Value = filmInfo.Year;
         
         // Datum je nach Auswahl setzen
         if (cbGridViewDateByRelease.Checked)
            Row.Cells["Datum"].Value = filmInfo.ReleaseDate;
         if (cbGridViewDateByFile.Checked)
            Row.Cells["Datum"].Value = filmInfo.FileLastWriteDate;
         if (cbGridViewDateByAdd.Checked)
         {
            if (filmInfo.DateTimeAdded != new DateTime(0))
               Row.Cells["Datum"].Value = filmInfo.DateTimeAdded;
            else
               Row.Cells["Datum"].Value = filmInfo.FileLastWriteDate;
         }
            
         
         Row.Tag = filmInfo;
         
         UpdateGenreComboBox(filmInfo);
         UpdateTagComboBox(filmInfo);
         UpdateDiskComboBox(filmInfo);
         Row.Selected = true;
      }      

      private void CreateGridViewColumns()
      {
         // Infos zu DataGridView hier:
         // http://csharp.net-informations.com/datagridview/csharp-datagridview-tutorial.htm
         dgvMain.Columns.Clear();
         dgvMain.Rows.Clear();         
         
         dgvMain.ColumnCount = 5;
         dgvMain.Columns[0].Name = "Titel und Dateiname";
         dgvMain.Columns[0].Width = 250;
         dgvMain.Columns[1].Name = "Rating";
         dgvMain.Columns[1].Width = 50;
         dgvMain.Columns[2].Name = "Laufzeit";
         dgvMain.Columns[2].Width = 60;
         dgvMain.Columns[3].Name = "Jahr";
         dgvMain.Columns[3].Width = 50;
         dgvMain.Columns[4].Name = "Datum";
         dgvMain.Columns[4].Width = 75;
         
         /*
         DataGridViewLinkColumn lnk = new DataGridViewLinkColumn();
         dgvMain.Columns.Add(lnk);
         lnk.HeaderText = "IMDB-Link";
         lnk.Name = "IMDB";
         lnk.UseColumnTextForLinkValue = false;
         m_ColIndex_ImdbUrl = dgvMain.Columns.Count - 1;         
        
         DataGridViewImageColumn img = new DataGridViewImageColumn();
         dgvMain.Columns.Add(img);
         img.HeaderText = "Poster";
         img.Name = "img";
         m_ColIndex_Poster = dgvMain.Columns.Count - 1;  
         img.Visible = false;
         */
      }      
      
      private void AddFilmInfoToGridView(FilmInfo fi)
      {         
         int idx = dgvMain.Rows.Add();
         UpdateDataGridViewByFilmInfo(dgvMain.Rows[idx], fi);
         
         // Thumbnail-Poster-Bild einbinden
         //dgvMain.Rows[idx].Cells[m_ColIndex_Poster].Value = GetPosterImage(fi, 50, 50);
      }      
      
      void BtnCloseClick(object sender, EventArgs e)
      {
         this.Close();
      }
      
      void BtnScanDirClick(object sender, EventArgs e)
      {
         List<string> l = new List<string>();
         if (folderDialog.ShowDialog() != DialogResult.OK)
            return;
         
         l.AddRange(Directory.GetFiles(folderDialog.SelectedPath));

         FillFilmList(l);     
         RefreshWebServerData();
      }
      
      void BtnClearFilmListClick(object sender, EventArgs e)
      {
         if (MsgBox.ShowYesNoQuestion("Wollen Sie wirklich die komplette Liste löschen?")
             == DialogResult.Yes)
         {
            dgvMain.Rows.Clear();
         }
      }
      
      void BtnRemoveSelectedClick(object sender, EventArgs e)
      {
         if (MsgBox.ShowYesNoQuestion("Wollen Sie wirklich die markierten Einträge löschen?")
             == DialogResult.No)
            return;
            
         foreach(DataGridViewRow dgvr in dgvMain.SelectedRows)
         {
            dgvMain.Rows.Remove(dgvr);
         }
         _selectedFilmInfo = null;            
      }
      
      void BtnClearImageCacheClick(object sender, EventArgs e)
      {
         ClearImageCache();
      }
      
      void BtnClearDataCacheClick(object sender, EventArgs e)
      {
         ClearJSonCache();
      }
      
      void FrmMainFormClosing(object sender, FormClosingEventArgs e)
      {
         WriteSettings();
      	SaveFilmInfoListToCompressedFile();
      	_FilmInfoHttpServer.CloseServer();
      }
      
      void SearchFieldChanged(object sender, EventArgs e)
      {
         FillSearchInfo();
         FilterGridViewBySearchInfo();
      }
      
      private void FillSearchInfo()
      {       
         GridViewSearchInfo.Title = tbSearchTitle.Text.ToLower();
         GridViewSearchInfo.Rating = tbSearchRating.Text;
         if (cbSearchGenre.SelectedIndex > 0)
            GridViewSearchInfo.Genre = cbSearchGenre.SelectedItem.ToString();
         else
            GridViewSearchInfo.Genre = null;
         
         if (cbSearchTag.SelectedIndex > 0)
            GridViewSearchInfo.Tag = cbSearchTag.SelectedItem.ToString();
         else
            GridViewSearchInfo.Tag = null;         
         
         if (cbSearchVolumeLabel.SelectedIndex > 0)
            GridViewSearchInfo.VolumeLabel = cbSearchVolumeLabel.SelectedItem.ToString();
         else
            GridViewSearchInfo.VolumeLabel = null;         
         
         GridViewSearchInfo.Actor = tbSearchActor.Text.ToLower();         
         GridViewSearchInfo.Director = tbSearchDirector.Text.ToLower();
         
         if (cbSearch3D.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Is3D = cbSearch3D.CheckState.ToString();
         else
            GridViewSearchInfo.Is3D = null;
         if (cbSearchChannels.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Channels = cbSearchChannels.CheckState.ToString();
         else
            GridViewSearchInfo.Channels = null;
         
         if (cbSearchWatched.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Watched = cbSearchWatched.CheckState.ToString();
         else
            GridViewSearchInfo.Watched = null;

         if (cbSearchArchieved.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Archieved = cbSearchArchieved.CheckState.ToString();
         else
            GridViewSearchInfo.Archieved = null;         
         
         if (cbSearchFavourite.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Favourite = cbSearchFavourite.CheckState.ToString();
         else
            GridViewSearchInfo.Favourite = null;
         
         if (cbSearchUnknown.CheckState != CheckState.Indeterminate)
            GridViewSearchInfo.Unknown = cbSearchUnknown.CheckState.ToString();
         else
            GridViewSearchInfo.Unknown = null;
         
         if (cbSearchSdHd.SelectedIndex > 0)
            GridViewSearchInfo.SdHd = cbSearchSdHd.SelectedItem.ToString();         
         else
            GridViewSearchInfo.SdHd = null;
      }
      
      public void FilterGridViewBySearchInfo()
      {
         if (GridViewSearchInfo == null)
            return;
         
         int visibleCount = 0;
         
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {            
            FilmInfo fi = dgvr.Tag as FilmInfo;
            dgvr.Visible = fi.IsVisible(GridViewSearchInfo);
            if (dgvr.Visible)
               visibleCount++;
         }
         
         lStatus.Text = visibleCount.ToString() + " Einträge gefunden.";
      }
      
      
      /// <summary>
      /// Trägt ggf. neue Disks (aus Filmliste) in die zugehörige ComboBox ein
      /// </summary>
      /// <param name="fi"></param>
      private void UpdateDiskComboBox(FilmInfo fi)
      {
         Dictionary<string, bool> dictItems = new Dictionary<string, bool>();
         
         if (!cbSearchVolumeLabel.Items.Contains(""))
         {
            dictItems.Add("", true);
            cbSearchVolumeLabel.Items.Add("");
         }                        
         
         // erstmal alle bereits vorhandenen Disks ermitteln
         foreach (var v in cbSearchVolumeLabel.Items)
         {
            if (!dictItems.ContainsKey(v.ToString()))
            {
               dictItems.Add(v.ToString(), true);
            }
         }


         // jetzt prüfen, ob die Disks der aktuellen FilmInfo
         // schon ex. und ggf. nachtragen
         if (!dictItems.ContainsKey(fi.VolumeLabel))
         {
            dictItems.Add(fi.VolumeLabel, true);
            cbSearchVolumeLabel.Items.Add(fi.VolumeLabel);
         }             

         cbSearchVolumeLabel.Sorted = true;
      }
      
      
      
      /// <summary>
      /// Trägt ggf. neue Genres (aus Filmliste) in die zugehörige ComboBox ein
      /// </summary>
      /// <param name="fi"></param>
      private void UpdateGenreComboBox(FilmInfo fi)
      {
         Dictionary<string, bool> dictItems = new Dictionary<string, bool>();
                  
         // erstmal alle bereits vorhandenen Genres ermitteln
         foreach (var v in cbSearchGenre.Items)
         {
            if (!dictItems.ContainsKey(v.ToString()))
            {
               dictItems.Add(v.ToString(), true);
            }
         }


         // jetzt prüfen, ob die Genres der aktuellen FilmInfo
         // schon ex. und ggf. nachtragen
         foreach (string genre in fi.Genres)
         {
            if (!dictItems.ContainsKey(genre))
            {
               dictItems.Add(genre, true);
               cbSearchGenre.Items.Add(genre);
            } 
         }
         cbSearchGenre.Sorted = true;
      }
      
      /// <summary>
      /// Trägt ggf. neue Genres (aus Filmliste) in die zugehörige ComboBox ein
      /// </summary>
      /// <param name="fi"></param>
      private void UpdateTagComboBox(FilmInfo fi)
      {
         Dictionary<string, bool> dictItems = new Dictionary<string, bool>();
      
         if (!cbSearchTag.Items.Contains(""))
         {
            dictItems.Add("", true);
            cbSearchTag.Items.Add("");
         }          
         
         // erstmal alle bereits vorhandenen Genres ermitteln
         foreach (var v in cbSearchTag.Items)
         {
            if (!dictItems.ContainsKey(v.ToString()))
            {
               dictItems.Add(v.ToString(), true);
            }
         }


         // jetzt prüfen, ob die Genres der aktuellen FilmInfo
         // schon ex. und ggf. nachtragen
         if (fi.TagList == null)
            return; 
         
         foreach (KeyValuePair<string, bool> kvp in fi.TagList)
         {
            string tag = kvp.Key;
            if (!dictItems.ContainsKey(tag))
            {
               dictItems.Add(tag, true);
               cbSearchTag.Items.Add(tag);
            } 
         }
         cbSearchTag.Sorted = true;
      }
            
      
      private string[] GetGenreStringArray()
      {
         List<string> genres = new List<string>();
         foreach (var v in cbSearchGenre.Items)
         {
            genres.Add(v.ToString());
         }     
         return genres.ToArray();
      }       
      
      private string[] GetTagStringArray()
      {
         List<string> tags = new List<string>();
         foreach (var v in cbSearchTag.Items)
         {
            tags.Add(v.ToString());
         }     
         return tags.ToArray();
      }
              
      
      void LAmazonSearchLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
         	Process.Start(_HtmlExport.GetAmazonSearchString(_selectedFilmInfo));
      	}
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }
      
      
      void BtnAddDirClick(object sender, EventArgs e)
      {
         List<string> l = new List<string>();
         if (folderDialog.ShowDialog() != DialogResult.OK)
            return;
         
         l.AddRange(Directory.GetDirectories(folderDialog.SelectedPath));

         FillFilmList(l);
         RefreshWebServerData();
      }
      
      void CbWatchedCheckedChanged(object sender, EventArgs e)
      {
         if (cbWatched.Checked)
      	   _selectedFilmInfo.Watched = DateTime.Now;
         else
            _selectedFilmInfo.Watched = new DateTime(0);
         
         ShowDetails();
      }
      
      void CbFavouriteCheckedChanged(object sender, EventArgs e)
      {
      	_selectedFilmInfo.Favourite = cbFavourite.Checked;
      	ShowDetails();
      }
      
      private void UpdateFilmInfoListByInternet()
      {         
         this.Cursor = Cursors.WaitCursor;
         dgvMain.Enabled = false;
         
         // Workaround, weil auch unsichtbare Rows selektiert sein können:
         // Wenn nicht sichtbar, dann auch nicht selektiert!
         foreach(DataGridViewRow row in dgvMain.SelectedRows)
         {
            if (!row.Visible)
               row.Selected = false;
         }
         
         // Init
         pbMain.Maximum = dgvMain.SelectedRows.Count;
         pbMain.Value = 0;         
         lStatus.Text = pbMain.Value.ToString() + " / " + dgvMain.SelectedRows.Count.ToString();
         Application.DoEvents();
         StringBuilder sbErrors = new StringBuilder();
         
         // selektierte Einträge durchlaufen
         foreach(DataGridViewRow row in dgvMain.SelectedRows)
         {
            FilmInfo fi = row.Tag as FilmInfo;
            FilmInfo fiNew = new FilmInfo();
            
            // IMDB-Infos ermitteln
            fiNew = _FilmInfoManager.GetMovieInfo(
                        Path.GetFileNameWithoutExtension(fi.Filename), true, fi.ImdbId);
            // Fehler ggf. speichern
            if (!string.IsNullOrEmpty(fiNew.RetrieveState))
            {
               sbErrors.AppendLine(fiNew.Query + ": " + fiNew.RetrieveState);
            }
            
            
            // nicht per IMDB ermittelte Informationen auf das neue 
            // FilmInfo-Objekt übernehmen
            CopyFilmInfoFileDetails(fi, fiNew);
            
            Application.DoEvents();
            
            // in GridView vorhandenes FilmInfo-Objekt durch neues ersetzen
            UpdateDataGridViewByFilmInfo(row, fiNew);
            
            pbMain.Value++;
            lStatus.Text = pbMain.Value.ToString() + " / " + dgvMain.SelectedRows.Count.ToString();
            Application.DoEvents();
         }
         this.Cursor = Cursors.Default;
         
         // Fehler?
         if (sbErrors.Length > 0)
         {
            MsgBox.ShowCmnError("Beim Aktualisieren traten Fehler auf:\n"
                                + sbErrors.ToString());
         }
         
         dgvMain.Enabled = true;
      }
      
      void BtnRefreshByFileClick(object sender, EventArgs e)
      {
         UpdateFilmInfoListByFile();
      }
      
      private void UpdateFilmInfoListByFile()
      {
         this.Cursor = Cursors.WaitCursor;
         pbMain.Maximum = dgvMain.SelectedRows.Count;
         pbMain.Value = 0;         
         
         foreach(DataGridViewRow row in dgvMain.SelectedRows)
         {
            FilmInfo fi = row.Tag as FilmInfo;
            
            _FilmInfoManager.UpdateFilmInfoByFile(fi);
            
            pbMain.Value++;
            lStatus.Text = pbMain.Value.ToString() + " / " + dgvMain.SelectedRows.Count.ToString();
            Application.DoEvents();
         }
         this.Cursor = Cursors.Default;
      }      
      
      void CbGridViewDateByFileClick(object sender, EventArgs e)
      {
      	//cbGridViewDateByFile.Checked = true;
      	cbGridViewDateByRelease.Checked = false;
      	cbGridViewDateByAdd.Checked = false;      	
      	
      	RefreshDataGridView();      	
      }
      
      void CbGridViewDateByReleaseClick(object sender, EventArgs e)
      {
      	cbGridViewDateByFile.Checked = false;
      	//cbGridViewDateByRelease.Checked = true;
      	cbGridViewDateByAdd.Checked = false;    
      	
      	RefreshDataGridView();
      }
      
      void CbGridViewDateByAddClick(object sender, EventArgs e)
      {
      	cbGridViewDateByFile.Checked = false;
      	cbGridViewDateByRelease.Checked = false;
      	//cbGridViewDateByAdd.Checked = true;
      	
      	RefreshDataGridView();         	
      }
      
      private void RefreshDataGridView()
      {
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {
            UpdateDataGridViewByFilmInfo(dgvr, dgvr.Tag as FilmInfo);
         }
      }
      
      void BtnStartStopHttpServerClick(object sender, EventArgs e)
      {
         if (btnStartStopHttpServer.Text == "Stop")
         {
            _FilmInfoHttpServer.StopServer();
            btnStartStopHttpServer.Text = "Start";
         }
         else
         {
            _FilmInfoHttpServer.StartServer();
            btnStartStopHttpServer.Text = "Stop";
         }
      }
      
      void BtnHttpServerOpenClick(object sender, EventArgs e)
      {
         System.Diagnostics.Process.Start(_FilmInfoHttpServer.ServerAdress);
      }
      
      void BtnShowHttpServerStatsClick(object sender, EventArgs e)
      {
         _FrmHttpServerStatistic.ShowStats();
      }
      
      void LMetacriticSearchLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
      	try
         {
         	Process.Start(_HtmlExport.GetMetacriticSearchString(_selectedFilmInfo));
      	}
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }
      }
      
      void LYoutubeSearchLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
      	try
         {
         	Process.Start(_HtmlExport.GetYouTubeSearchString(_selectedFilmInfo));
      	}
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }      	
      }
      
      void LMovieThumbnailerLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
      	try
         {
      	   Process.Start(_FilmInfoManager.GetMovieThumbnailerImage(_selectedFilmInfo.Filename));
      	}
         catch(Exception ex)
         {
            MsgBox.ShowCmnError(ex.Message);
         }      	
      }
      
      void BtnScanDirBulkClick(object sender, EventArgs e)
      {
         if (!File.Exists(_MassScanDirListFilename))
         {
            MsgBox.ShowCmnError("Datei " + _MassScanDirListFilename + " existiert nicht. Abbruch.");
            return;
         }

         List<string> l = new List<string>();
         foreach(string path in File.ReadAllLines(_MassScanDirListFilename))
         {
            if (Directory.Exists(path))
               l.AddRange(Directory.GetFiles(path));
         }

         FillFilmList(l);
         RefreshWebServerData();         
      }
      
      void BtnRefreshItemsClick(object sender, EventArgs e)
      {
      	UpdateFilmInfoListByInternet();
      	ShowDetails();      	
         RefreshWebServerData();      	
      }
      
      void BtnShowDoubleEntriesClick(object sender, EventArgs e)
      {
         Dictionary<string, int> dictImdbId = new Dictionary<string, int>();
         Dictionary<string, int> dictQuery = new Dictionary<string, int>();
         
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {            
            FilmInfo fi = dgvr.Tag as FilmInfo;
            
            // zählen, wie oft bestimmte IMDB-Id's vorkommen
            if (fi.ImdbId.StartsWith("tt"))
            {
               if (!dictImdbId.ContainsKey(fi.ImdbId))
               {
                  dictImdbId.Add(fi.ImdbId, 1);
               }
               else
               {
                  dictImdbId[fi.ImdbId] = dictImdbId[fi.ImdbId] + 1;
               }  
            }
            
            // zählen, wie oft bestimmte Queries vorkommen         
            if (!dictQuery.ContainsKey(fi.Query))
            {
               dictQuery.Add(fi.Query, 1);
            }
            else
            {
               dictQuery[fi.Query] = dictQuery[fi.Query] + 1;
            }            
         }      	
         
         // Filme erneut durchlaufen, jetzt aber nur noch die 
         // mind. 2x vorhandenen Filme sichtbar lassen
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {            
            bool isVisible = false;
            
            FilmInfo fi = dgvr.Tag as FilmInfo;
            
            // Imdb-Id
            if (dictImdbId.ContainsKey(fi.ImdbId))
            {
               isVisible = dictImdbId[fi.ImdbId] > 1;
            }
            
            // Query
            if (!isVisible && dictQuery.ContainsKey(fi.Query))
            {
               isVisible = dictQuery[fi.Query] > 1;
            }
            
            dgvr.Visible = isVisible;
         }
      }
      
      void CbArchievedCheckedChanged(object sender, EventArgs e)
      {
      	_selectedFilmInfo.Archived = cbArchieved.Checked;
      	ShowDetails();      	
      }
      
      void BtnOptionsClick(object sender, EventArgs e)
      {
         FrmOptions f = new FrmOptions();
         f.cbUseProxy.Checked = _UseProxy;
         f.tbExecuteFilename.Text = _ExecuteFilename;
         if (f.ShowDialog() == DialogResult.OK)
         {
            _UseProxy = f.cbUseProxy.Checked;
            _ExecuteFilename = f.tbExecuteFilename.Text;
            WriteSettings();
         }
      }
      
      void BtnExportCsvClick(object sender, EventArgs e)
      {	
         SaveFileDialog saveFileMain = new SaveFileDialog();
         saveFileMain.Filter = "CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
         saveFileMain.FileName = "filminfo.csv";
         saveFileMain.DefaultExt = ".csv";
         
         if (saveFileMain.ShowDialog() != DialogResult.OK)
            return;
         
         try 
         {
            ExportCsv(saveFileMain.FileName);
            MsgBox.ShowInfo("Die CSV-Datei wurde erfolgreich erstellt.");            
         }
         catch(Exception x)
         {
            MsgBox.ShowCmnError(x.Message);
         }  
      }
      
      private void ExportCsv(string Filename)
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine(FilmInfo.GetCsvStringHeader());
         
         foreach(DataGridViewRow dgvr in dgvMain.Rows)
         {
            FilmInfo fi = dgvr.Tag as FilmInfo;
            sb.AppendLine(fi.GetCsvString());
         }

         File.WriteAllText(Filename, sb.ToString(), Encoding.Default);
      
      }
      
      void BtnRefreshWebServerClick(object sender, EventArgs e)
      {
         RefreshWebServerData();
      }
      
      private void RefreshWebServerData()
      {
         _HtmlExport = new HtmlExport(dgvMain, 
                                      _HtmlExportListTemplateFilename,
                                      _HtmlExportDetailTemplateFilename,                                      
                                      _FilmInfoManager,
                                      GetGenreStringArray(),
                                      GetTagStringArray());
         _FilmInfoHttpServer.UpdateMovieList(_HtmlExport);         
      }
         
      
      void BtnSaveClick(object sender, EventArgs e)
      {
      	SaveFilmInfoListToCompressedFile();
      	MsgBox.ShowInfo("Die Liste wurde erfolgreich gespeichert.");
      }
      
      void BtnSaveTagListClick(object sender, EventArgs e)
      {
         _selectedFilmInfo.SetTagList(tbTags.Text);
         UpdateTagComboBox(_selectedFilmInfo);
      }
      void BtnUpdateFilmInfoByFileClick(object sender, EventArgs e)
      {
    
      }
   }
}
