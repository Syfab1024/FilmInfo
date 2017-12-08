/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 26.02.2013
 * Zeit: 08:11
 * 
 */
namespace FilmInfo
{
   partial class FrmMain
   {
      /// <summary>
      /// Designer variable used to keep track of non-visual components.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      
      /// <summary>
      /// Disposes resources used by the form.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing) {
            if (components != null) {
               components.Dispose();
            }
         }
         base.Dispose(disposing);
      }
      
      /// <summary>
      /// This method is required for Windows Forms designer support.
      /// Do not change the method contents inside the source code editor. The Forms designer might
      /// not be able to load this method if it was changed manually.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
         this.ssMain = new System.Windows.Forms.StatusStrip();
         this.lStatus = new System.Windows.Forms.ToolStripStatusLabel();
         this.pbMain = new System.Windows.Forms.ToolStripProgressBar();
         this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.btnSaveTagList = new System.Windows.Forms.Button();
         this.tbTags = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.myRating = new System.Windows.Forms.Label();
         this.cbArchieved = new System.Windows.Forms.CheckBox();
         this.lShowMovieThumbnail = new System.Windows.Forms.LinkLabel();
         this.lYoutubeSearch = new System.Windows.Forms.LinkLabel();
         this.lMetacriticSearch = new System.Windows.Forms.LinkLabel();
         this.lFileDetails = new System.Windows.Forms.Label();
         this.cbFavourite = new System.Windows.Forms.CheckBox();
         this.cbWatched = new System.Windows.Forms.CheckBox();
         this.lAmazonSearch = new System.Windows.Forms.LinkLabel();
         this.lDriveLabel = new System.Windows.Forms.Label();
         this.lYearRuntime = new System.Windows.Forms.Label();
         this.lRating = new System.Windows.Forms.Label();
         this.btnAkaChange = new System.Windows.Forms.Button();
         this.lImdbSearch = new System.Windows.Forms.LinkLabel();
         this.lFilename = new System.Windows.Forms.LinkLabel();
         this.lGenres = new System.Windows.Forms.Label();
         this.lDirectors = new System.Windows.Forms.Label();
         this.llImdbUrl = new System.Windows.Forms.LinkLabel();
         this.lActors = new System.Windows.Forms.Label();
         this.lPlot = new System.Windows.Forms.Label();
         this.pbPoster = new System.Windows.Forms.PictureBox();
         this.btnFind = new System.Windows.Forms.Button();
         this.tbQuery = new System.Windows.Forms.TextBox();
         this.menuStripMain = new System.Windows.Forms.MenuStrip();
         this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.btnOptions = new System.Windows.Forms.ToolStripMenuItem();
         this.btnExportCsv = new System.Windows.Forms.ToolStripMenuItem();
         this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
         this.btnClose = new System.Windows.Forms.ToolStripMenuItem();
         this.filmlisteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.btnScanDir = new System.Windows.Forms.ToolStripMenuItem();
         this.btnAddDir = new System.Windows.Forms.ToolStripMenuItem();
         this.btnScanDirBulk = new System.Windows.Forms.ToolStripMenuItem();
         this.btnRefreshItems = new System.Windows.Forms.ToolStripMenuItem();
         this.btnRemoveSelected = new System.Windows.Forms.ToolStripMenuItem();
         this.btnShowDoubleEntries = new System.Windows.Forms.ToolStripMenuItem();
         this.datumInFilmlisteFestlegenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.cbGridViewDateByFile = new System.Windows.Forms.ToolStripMenuItem();
         this.cbGridViewDateByRelease = new System.Windows.Forms.ToolStripMenuItem();
         this.cbGridViewDateByAdd = new System.Windows.Forms.ToolStripMenuItem();
         this.cacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.btnClearImageCache = new System.Windows.Forms.ToolStripMenuItem();
         this.webserverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.btnHttpServerOpen = new System.Windows.Forms.ToolStripMenuItem();
         this.btnShowHttpServerStats = new System.Windows.Forms.ToolStripMenuItem();
         this.btnRefreshWebServer = new System.Windows.Forms.ToolStripMenuItem();
         this.btnStartStopHttpServer = new System.Windows.Forms.ToolStripMenuItem();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.cbSearchVolumeLabel = new System.Windows.Forms.ComboBox();
         this.label9 = new System.Windows.Forms.Label();
         this.cbSearchTag = new System.Windows.Forms.ComboBox();
         this.label8 = new System.Windows.Forms.Label();
         this.cbSearchArchieved = new System.Windows.Forms.CheckBox();
         this.cbSearchChannels = new System.Windows.Forms.CheckBox();
         this.cbSearchSdHd = new System.Windows.Forms.ComboBox();
         this.label7 = new System.Windows.Forms.Label();
         this.cbSearchUnknown = new System.Windows.Forms.CheckBox();
         this.cbSearchFavourite = new System.Windows.Forms.CheckBox();
         this.cbSearchWatched = new System.Windows.Forms.CheckBox();
         this.cbSearch3D = new System.Windows.Forms.CheckBox();
         this.cbSearchGenre = new System.Windows.Forms.ComboBox();
         this.label6 = new System.Windows.Forms.Label();
         this.tbSearchDirector = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.tbSearchActor = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.dgvMain = new System.Windows.Forms.DataGridView();
         this.tbSearchRating = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.tbSearchTitle = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ssMain.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbPoster)).BeginInit();
         this.menuStripMain.SuspendLayout();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // ssMain
         // 
         this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.lStatus,
         this.pbMain});
         this.ssMain.Location = new System.Drawing.Point(0, 787);
         this.ssMain.Name = "ssMain";
         this.ssMain.Size = new System.Drawing.Size(1092, 22);
         this.ssMain.TabIndex = 4;
         this.ssMain.Text = "statusStrip1";
         // 
         // lStatus
         // 
         this.lStatus.Name = "lStatus";
         this.lStatus.Size = new System.Drawing.Size(0, 17);
         // 
         // pbMain
         // 
         this.pbMain.Name = "pbMain";
         this.pbMain.Size = new System.Drawing.Size(200, 16);
         this.pbMain.Step = 1;
         this.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
         // 
         // folderDialog
         // 
         this.folderDialog.SelectedPath = "c:\\";
         this.folderDialog.ShowNewFolderButton = false;
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
         | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(this.btnSaveTagList);
         this.groupBox1.Controls.Add(this.tbTags);
         this.groupBox1.Controls.Add(this.label3);
         this.groupBox1.Controls.Add(this.myRating);
         this.groupBox1.Controls.Add(this.cbArchieved);
         this.groupBox1.Controls.Add(this.lShowMovieThumbnail);
         this.groupBox1.Controls.Add(this.lYoutubeSearch);
         this.groupBox1.Controls.Add(this.lMetacriticSearch);
         this.groupBox1.Controls.Add(this.lFileDetails);
         this.groupBox1.Controls.Add(this.cbFavourite);
         this.groupBox1.Controls.Add(this.cbWatched);
         this.groupBox1.Controls.Add(this.lAmazonSearch);
         this.groupBox1.Controls.Add(this.lDriveLabel);
         this.groupBox1.Controls.Add(this.lYearRuntime);
         this.groupBox1.Controls.Add(this.lRating);
         this.groupBox1.Controls.Add(this.btnAkaChange);
         this.groupBox1.Controls.Add(this.lImdbSearch);
         this.groupBox1.Controls.Add(this.lFilename);
         this.groupBox1.Controls.Add(this.lGenres);
         this.groupBox1.Controls.Add(this.lDirectors);
         this.groupBox1.Controls.Add(this.llImdbUrl);
         this.groupBox1.Controls.Add(this.lActors);
         this.groupBox1.Controls.Add(this.lPlot);
         this.groupBox1.Controls.Add(this.pbPoster);
         this.groupBox1.Location = new System.Drawing.Point(568, 41);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(512, 743);
         this.groupBox1.TabIndex = 8;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Details";
         // 
         // btnSaveTagList
         // 
         this.btnSaveTagList.Location = new System.Drawing.Point(424, 461);
         this.btnSaveTagList.Name = "btnSaveTagList";
         this.btnSaveTagList.Size = new System.Drawing.Size(85, 23);
         this.btnSaveTagList.TabIndex = 34;
         this.btnSaveTagList.Text = "Speichern";
         this.btnSaveTagList.UseVisualStyleBackColor = true;
         this.btnSaveTagList.Click += new System.EventHandler(this.BtnSaveTagListClick);
         // 
         // tbTags
         // 
         this.tbTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
         | System.Windows.Forms.AnchorStyles.Right)));
         this.tbTags.Location = new System.Drawing.Point(48, 461);
         this.tbTags.MaxLength = 100;
         this.tbTags.Name = "tbTags";
         this.tbTags.Size = new System.Drawing.Size(368, 20);
         this.tbTags.TabIndex = 33;
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(8, 464);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(40, 17);
         this.label3.TabIndex = 32;
         this.label3.Text = "Tags:";
         this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // myRating
         // 
         this.myRating.Location = new System.Drawing.Point(10, 628);
         this.myRating.Name = "myRating";
         this.myRating.Size = new System.Drawing.Size(264, 23);
         this.myRating.TabIndex = 31;
         this.myRating.Text = "Rating";
         // 
         // cbArchieved
         // 
         this.cbArchieved.Location = new System.Drawing.Point(310, 376);
         this.cbArchieved.Name = "cbArchieved";
         this.cbArchieved.Size = new System.Drawing.Size(153, 24);
         this.cbArchieved.TabIndex = 30;
         this.cbArchieved.Text = "Archiviert";
         this.cbArchieved.UseVisualStyleBackColor = true;
         this.cbArchieved.CheckedChanged += new System.EventHandler(this.CbArchievedCheckedChanged);
         // 
         // lShowMovieThumbnail
         // 
         this.lShowMovieThumbnail.Location = new System.Drawing.Point(310, 411);
         this.lShowMovieThumbnail.Name = "lShowMovieThumbnail";
         this.lShowMovieThumbnail.Size = new System.Drawing.Size(120, 15);
         this.lShowMovieThumbnail.TabIndex = 29;
         this.lShowMovieThumbnail.TabStop = true;
         this.lShowMovieThumbnail.Text = "Vorschaubild zeigen...";
         this.lShowMovieThumbnail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LMovieThumbnailerLinkClicked);
         // 
         // lYoutubeSearch
         // 
         this.lYoutubeSearch.Location = new System.Drawing.Point(155, 440);
         this.lYoutubeSearch.Name = "lYoutubeSearch";
         this.lYoutubeSearch.Size = new System.Drawing.Size(120, 15);
         this.lYoutubeSearch.TabIndex = 28;
         this.lYoutubeSearch.TabStop = true;
         this.lYoutubeSearch.Text = "Youtube-Filmsuche...";
         this.lYoutubeSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LYoutubeSearchLinkClicked);
         // 
         // lMetacriticSearch
         // 
         this.lMetacriticSearch.Location = new System.Drawing.Point(310, 440);
         this.lMetacriticSearch.Name = "lMetacriticSearch";
         this.lMetacriticSearch.Size = new System.Drawing.Size(120, 15);
         this.lMetacriticSearch.TabIndex = 27;
         this.lMetacriticSearch.TabStop = true;
         this.lMetacriticSearch.Text = "Metacritic-Filmsuche...";
         this.lMetacriticSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LMetacriticSearchLinkClicked);
         // 
         // lFileDetails
         // 
         this.lFileDetails.Location = new System.Drawing.Point(6, 536);
         this.lFileDetails.Name = "lFileDetails";
         this.lFileDetails.Size = new System.Drawing.Size(468, 82);
         this.lFileDetails.TabIndex = 26;
         this.lFileDetails.Text = "DateiDetailInformationen";
         // 
         // cbFavourite
         // 
         this.cbFavourite.Location = new System.Drawing.Point(10, 406);
         this.cbFavourite.Name = "cbFavourite";
         this.cbFavourite.Size = new System.Drawing.Size(139, 24);
         this.cbFavourite.TabIndex = 25;
         this.cbFavourite.Text = "Favourit";
         this.cbFavourite.UseVisualStyleBackColor = true;
         this.cbFavourite.CheckedChanged += new System.EventHandler(this.CbFavouriteCheckedChanged);
         // 
         // cbWatched
         // 
         this.cbWatched.Location = new System.Drawing.Point(155, 406);
         this.cbWatched.Name = "cbWatched";
         this.cbWatched.Size = new System.Drawing.Size(153, 24);
         this.cbWatched.TabIndex = 24;
         this.cbWatched.Text = "Film gesehen";
         this.cbWatched.UseVisualStyleBackColor = true;
         this.cbWatched.CheckedChanged += new System.EventHandler(this.CbWatchedCheckedChanged);
         // 
         // lAmazonSearch
         // 
         this.lAmazonSearch.Location = new System.Drawing.Point(6, 440);
         this.lAmazonSearch.Name = "lAmazonSearch";
         this.lAmazonSearch.Size = new System.Drawing.Size(120, 15);
         this.lAmazonSearch.TabIndex = 23;
         this.lAmazonSearch.TabStop = true;
         this.lAmazonSearch.Text = "Amazon-Filmsuche...";
         this.lAmazonSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LAmazonSearchLinkClicked);
         // 
         // lDriveLabel
         // 
         this.lDriveLabel.Location = new System.Drawing.Point(242, 323);
         this.lDriveLabel.Name = "lDriveLabel";
         this.lDriveLabel.Size = new System.Drawing.Size(264, 19);
         this.lDriveLabel.TabIndex = 22;
         this.lDriveLabel.Text = "DriveLabel";
         // 
         // lYearRuntime
         // 
         this.lYearRuntime.Location = new System.Drawing.Point(242, 72);
         this.lYearRuntime.Name = "lYearRuntime";
         this.lYearRuntime.Size = new System.Drawing.Size(264, 19);
         this.lYearRuntime.TabIndex = 21;
         this.lYearRuntime.Text = "Jahr und Laufzeit";
         // 
         // lRating
         // 
         this.lRating.Location = new System.Drawing.Point(242, 49);
         this.lRating.Name = "lRating";
         this.lRating.Size = new System.Drawing.Size(264, 23);
         this.lRating.TabIndex = 20;
         this.lRating.Text = "Rating";
         // 
         // btnAkaChange
         // 
         this.btnAkaChange.Location = new System.Drawing.Point(155, 376);
         this.btnAkaChange.Name = "btnAkaChange";
         this.btnAkaChange.Size = new System.Drawing.Size(133, 23);
         this.btnAkaChange.TabIndex = 18;
         this.btnAkaChange.Text = "Falscher Film?";
         this.btnAkaChange.UseVisualStyleBackColor = true;
         this.btnAkaChange.Click += new System.EventHandler(this.BtnAkaChangeClick);
         // 
         // lImdbSearch
         // 
         this.lImdbSearch.Location = new System.Drawing.Point(6, 381);
         this.lImdbSearch.Name = "lImdbSearch";
         this.lImdbSearch.Size = new System.Drawing.Size(120, 15);
         this.lImdbSearch.TabIndex = 17;
         this.lImdbSearch.TabStop = true;
         this.lImdbSearch.Text = "Imdb-Filmsuche...";
         this.lImdbSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LQueryLinkClicked);
         // 
         // lFilename
         // 
         this.lFilename.Location = new System.Drawing.Point(6, 352);
         this.lFilename.Name = "lFilename";
         this.lFilename.Size = new System.Drawing.Size(468, 15);
         this.lFilename.TabIndex = 16;
         this.lFilename.TabStop = true;
         this.lFilename.Text = "Datei anzeigen (.....)";
         this.lFilename.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LFilenameLinkClicked);
         // 
         // lGenres
         // 
         this.lGenres.Location = new System.Drawing.Point(242, 293);
         this.lGenres.Name = "lGenres";
         this.lGenres.Size = new System.Drawing.Size(264, 30);
         this.lGenres.TabIndex = 14;
         this.lGenres.Text = "Genres";
         // 
         // lDirectors
         // 
         this.lDirectors.Location = new System.Drawing.Point(242, 263);
         this.lDirectors.Name = "lDirectors";
         this.lDirectors.Size = new System.Drawing.Size(264, 30);
         this.lDirectors.TabIndex = 13;
         this.lDirectors.Text = "Directors";
         // 
         // llImdbUrl
         // 
         this.llImdbUrl.Location = new System.Drawing.Point(242, 16);
         this.llImdbUrl.Name = "llImdbUrl";
         this.llImdbUrl.Size = new System.Drawing.Size(264, 33);
         this.llImdbUrl.TabIndex = 12;
         this.llImdbUrl.TabStop = true;
         this.llImdbUrl.Text = "ImdbLink";
         this.llImdbUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LlImdbUrlLinkClicked);
         // 
         // lActors
         // 
         this.lActors.Location = new System.Drawing.Point(242, 186);
         this.lActors.Name = "lActors";
         this.lActors.Size = new System.Drawing.Size(264, 68);
         this.lActors.TabIndex = 11;
         this.lActors.Text = "Schauspieler";
         // 
         // lPlot
         // 
         this.lPlot.Location = new System.Drawing.Point(242, 91);
         this.lPlot.Name = "lPlot";
         this.lPlot.Size = new System.Drawing.Size(264, 95);
         this.lPlot.TabIndex = 10;
         this.lPlot.Text = "Plot";
         // 
         // pbPoster
         // 
         this.pbPoster.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.pbPoster.ImageLocation = "";
         this.pbPoster.Location = new System.Drawing.Point(6, 19);
         this.pbPoster.Name = "pbPoster";
         this.pbPoster.Size = new System.Drawing.Size(224, 320);
         this.pbPoster.TabIndex = 6;
         this.pbPoster.TabStop = false;
         // 
         // btnFind
         // 
         this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnFind.Location = new System.Drawing.Point(476, 755);
         this.btnFind.Name = "btnFind";
         this.btnFind.Size = new System.Drawing.Size(84, 23);
         this.btnFind.TabIndex = 11;
         this.btnFind.Text = "Hinzufügen";
         this.btnFind.UseVisualStyleBackColor = true;
         this.btnFind.Click += new System.EventHandler(this.BtnFindClick);
         // 
         // tbQuery
         // 
         this.tbQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.tbQuery.Location = new System.Drawing.Point(334, 757);
         this.tbQuery.Name = "tbQuery";
         this.tbQuery.Size = new System.Drawing.Size(136, 20);
         this.tbQuery.TabIndex = 10;
         // 
         // menuStripMain
         // 
         this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.dateiToolStripMenuItem,
         this.filmlisteToolStripMenuItem,
         this.cacheToolStripMenuItem,
         this.webserverToolStripMenuItem});
         this.menuStripMain.Location = new System.Drawing.Point(0, 0);
         this.menuStripMain.Name = "menuStripMain";
         this.menuStripMain.Size = new System.Drawing.Size(1092, 24);
         this.menuStripMain.TabIndex = 14;
         this.menuStripMain.Text = "menuStrip1";
         // 
         // dateiToolStripMenuItem
         // 
         this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.btnOptions,
         this.btnExportCsv,
         this.btnSave,
         this.btnClose});
         this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
         this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
         this.dateiToolStripMenuItem.Text = "Datei";
         // 
         // btnOptions
         // 
         this.btnOptions.Name = "btnOptions";
         this.btnOptions.Size = new System.Drawing.Size(194, 22);
         this.btnOptions.Text = "Einstellungen...";
         this.btnOptions.Click += new System.EventHandler(this.BtnOptionsClick);
         // 
         // btnExportCsv
         // 
         this.btnExportCsv.Name = "btnExportCsv";
         this.btnExportCsv.Size = new System.Drawing.Size(194, 22);
         this.btnExportCsv.Text = "CSV-Export...";
         this.btnExportCsv.Click += new System.EventHandler(this.BtnExportCsvClick);
         // 
         // btnSave
         // 
         this.btnSave.Name = "btnSave";
         this.btnSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
         this.btnSave.Size = new System.Drawing.Size(194, 22);
         this.btnSave.Text = "Liste speichern";
         this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
         // 
         // btnClose
         // 
         this.btnClose.Name = "btnClose";
         this.btnClose.Size = new System.Drawing.Size(194, 22);
         this.btnClose.Text = "Beenden";
         this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
         // 
         // filmlisteToolStripMenuItem
         // 
         this.filmlisteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.btnScanDir,
         this.btnAddDir,
         this.btnScanDirBulk,
         this.btnRefreshItems,
         this.btnRemoveSelected,
         this.btnShowDoubleEntries,
         this.datumInFilmlisteFestlegenToolStripMenuItem});
         this.filmlisteToolStripMenuItem.Name = "filmlisteToolStripMenuItem";
         this.filmlisteToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
         this.filmlisteToolStripMenuItem.Text = "Filmliste";
         // 
         // btnScanDir
         // 
         this.btnScanDir.Name = "btnScanDir";
         this.btnScanDir.Size = new System.Drawing.Size(282, 22);
         this.btnScanDir.Text = "Verzeichnisscan (Dateien)...";
         this.btnScanDir.Click += new System.EventHandler(this.BtnScanDirClick);
         // 
         // btnAddDir
         // 
         this.btnAddDir.Name = "btnAddDir";
         this.btnAddDir.Size = new System.Drawing.Size(282, 22);
         this.btnAddDir.Text = "Verzeichnisscan (Verzeichnisse)...";
         this.btnAddDir.Click += new System.EventHandler(this.BtnAddDirClick);
         // 
         // btnScanDirBulk
         // 
         this.btnScanDirBulk.Name = "btnScanDirBulk";
         this.btnScanDirBulk.Size = new System.Drawing.Size(282, 22);
         this.btnScanDirBulk.Text = "Massenscan (Dateien)...";
         this.btnScanDirBulk.Click += new System.EventHandler(this.BtnScanDirBulkClick);
         // 
         // btnRefreshItems
         // 
         this.btnRefreshItems.Name = "btnRefreshItems";
         this.btnRefreshItems.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
         this.btnRefreshItems.Size = new System.Drawing.Size(282, 22);
         this.btnRefreshItems.Text = "Markierte Einträge aktualisieren";
         this.btnRefreshItems.Click += new System.EventHandler(this.BtnRefreshItemsClick);
         // 
         // btnRemoveSelected
         // 
         this.btnRemoveSelected.Name = "btnRemoveSelected";
         this.btnRemoveSelected.ShortcutKeys = System.Windows.Forms.Keys.Delete;
         this.btnRemoveSelected.Size = new System.Drawing.Size(282, 22);
         this.btnRemoveSelected.Text = "Markierte Einträge entfernen";
         this.btnRemoveSelected.Click += new System.EventHandler(this.BtnRemoveSelectedClick);
         // 
         // btnShowDoubleEntries
         // 
         this.btnShowDoubleEntries.Name = "btnShowDoubleEntries";
         this.btnShowDoubleEntries.Size = new System.Drawing.Size(282, 22);
         this.btnShowDoubleEntries.Text = "Doppelte Einträge anzeigen";
         this.btnShowDoubleEntries.Click += new System.EventHandler(this.BtnShowDoubleEntriesClick);
         // 
         // datumInFilmlisteFestlegenToolStripMenuItem
         // 
         this.datumInFilmlisteFestlegenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.cbGridViewDateByFile,
         this.cbGridViewDateByRelease,
         this.cbGridViewDateByAdd});
         this.datumInFilmlisteFestlegenToolStripMenuItem.Name = "datumInFilmlisteFestlegenToolStripMenuItem";
         this.datumInFilmlisteFestlegenToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
         this.datumInFilmlisteFestlegenToolStripMenuItem.Text = "Datum in Filmliste festlegen";
         // 
         // cbGridViewDateByFile
         // 
         this.cbGridViewDateByFile.CheckOnClick = true;
         this.cbGridViewDateByFile.Name = "cbGridViewDateByFile";
         this.cbGridViewDateByFile.Size = new System.Drawing.Size(164, 22);
         this.cbGridViewDateByFile.Text = "Dateidatum";
         this.cbGridViewDateByFile.Click += new System.EventHandler(this.CbGridViewDateByFileClick);
         // 
         // cbGridViewDateByRelease
         // 
         this.cbGridViewDateByRelease.CheckOnClick = true;
         this.cbGridViewDateByRelease.Name = "cbGridViewDateByRelease";
         this.cbGridViewDateByRelease.Size = new System.Drawing.Size(164, 22);
         this.cbGridViewDateByRelease.Text = "Veröffentlichung";
         this.cbGridViewDateByRelease.Click += new System.EventHandler(this.CbGridViewDateByReleaseClick);
         // 
         // cbGridViewDateByAdd
         // 
         this.cbGridViewDateByAdd.Checked = true;
         this.cbGridViewDateByAdd.CheckOnClick = true;
         this.cbGridViewDateByAdd.CheckState = System.Windows.Forms.CheckState.Checked;
         this.cbGridViewDateByAdd.Name = "cbGridViewDateByAdd";
         this.cbGridViewDateByAdd.Size = new System.Drawing.Size(164, 22);
         this.cbGridViewDateByAdd.Text = "Hinzufügedatum";
         this.cbGridViewDateByAdd.Click += new System.EventHandler(this.CbGridViewDateByAddClick);
         // 
         // cacheToolStripMenuItem
         // 
         this.cacheToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.btnClearImageCache});
         this.cacheToolStripMenuItem.Name = "cacheToolStripMenuItem";
         this.cacheToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
         this.cacheToolStripMenuItem.Text = "Cache";
         // 
         // btnClearImageCache
         // 
         this.btnClearImageCache.Name = "btnClearImageCache";
         this.btnClearImageCache.Size = new System.Drawing.Size(170, 22);
         this.btnClearImageCache.Text = "Bildercache leeren";
         this.btnClearImageCache.Click += new System.EventHandler(this.BtnClearImageCacheClick);
         // 
         // webserverToolStripMenuItem
         // 
         this.webserverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.btnHttpServerOpen,
         this.btnShowHttpServerStats,
         this.btnRefreshWebServer,
         this.btnStartStopHttpServer});
         this.webserverToolStripMenuItem.Name = "webserverToolStripMenuItem";
         this.webserverToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
         this.webserverToolStripMenuItem.Text = "Webserver";
         // 
         // btnHttpServerOpen
         // 
         this.btnHttpServerOpen.Name = "btnHttpServerOpen";
         this.btnHttpServerOpen.Size = new System.Drawing.Size(199, 22);
         this.btnHttpServerOpen.Text = "Seite im Browser öffnen";
         this.btnHttpServerOpen.Click += new System.EventHandler(this.BtnHttpServerOpenClick);
         // 
         // btnShowHttpServerStats
         // 
         this.btnShowHttpServerStats.Name = "btnShowHttpServerStats";
         this.btnShowHttpServerStats.Size = new System.Drawing.Size(199, 22);
         this.btnShowHttpServerStats.Text = "Statistik anzeigen...";
         this.btnShowHttpServerStats.Click += new System.EventHandler(this.BtnShowHttpServerStatsClick);
         // 
         // btnRefreshWebServer
         // 
         this.btnRefreshWebServer.Name = "btnRefreshWebServer";
         this.btnRefreshWebServer.Size = new System.Drawing.Size(199, 22);
         this.btnRefreshWebServer.Text = "Aktualisieren";
         this.btnRefreshWebServer.Click += new System.EventHandler(this.BtnRefreshWebServerClick);
         // 
         // btnStartStopHttpServer
         // 
         this.btnStartStopHttpServer.Name = "btnStartStopHttpServer";
         this.btnStartStopHttpServer.Size = new System.Drawing.Size(199, 22);
         this.btnStartStopHttpServer.Text = "Stop";
         this.btnStartStopHttpServer.Click += new System.EventHandler(this.BtnStartStopHttpServerClick);
         // 
         // groupBox2
         // 
         this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
         | System.Windows.Forms.AnchorStyles.Left) 
         | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox2.Controls.Add(this.cbSearchVolumeLabel);
         this.groupBox2.Controls.Add(this.label9);
         this.groupBox2.Controls.Add(this.cbSearchTag);
         this.groupBox2.Controls.Add(this.label8);
         this.groupBox2.Controls.Add(this.cbSearchArchieved);
         this.groupBox2.Controls.Add(this.cbSearchChannels);
         this.groupBox2.Controls.Add(this.cbSearchSdHd);
         this.groupBox2.Controls.Add(this.label7);
         this.groupBox2.Controls.Add(this.cbSearchUnknown);
         this.groupBox2.Controls.Add(this.cbSearchFavourite);
         this.groupBox2.Controls.Add(this.cbSearchWatched);
         this.groupBox2.Controls.Add(this.cbSearch3D);
         this.groupBox2.Controls.Add(this.cbSearchGenre);
         this.groupBox2.Controls.Add(this.label6);
         this.groupBox2.Controls.Add(this.tbSearchDirector);
         this.groupBox2.Controls.Add(this.label5);
         this.groupBox2.Controls.Add(this.tbSearchActor);
         this.groupBox2.Controls.Add(this.label4);
         this.groupBox2.Controls.Add(this.dgvMain);
         this.groupBox2.Controls.Add(this.tbSearchRating);
         this.groupBox2.Controls.Add(this.label2);
         this.groupBox2.Controls.Add(this.tbSearchTitle);
         this.groupBox2.Controls.Add(this.label1);
         this.groupBox2.Location = new System.Drawing.Point(12, 41);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(548, 708);
         this.groupBox2.TabIndex = 15;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Filmliste";
         // 
         // cbSearchVolumeLabel
         // 
         this.cbSearchVolumeLabel.DropDownHeight = 200;
         this.cbSearchVolumeLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbSearchVolumeLabel.DropDownWidth = 150;
         this.cbSearchVolumeLabel.FormattingEnabled = true;
         this.cbSearchVolumeLabel.IntegralHeight = false;
         this.cbSearchVolumeLabel.Location = new System.Drawing.Point(256, 64);
         this.cbSearchVolumeLabel.Name = "cbSearchVolumeLabel";
         this.cbSearchVolumeLabel.Size = new System.Drawing.Size(150, 21);
         this.cbSearchVolumeLabel.TabIndex = 23;
         this.cbSearchVolumeLabel.SelectedIndexChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label9
         // 
         this.label9.Location = new System.Drawing.Point(204, 69);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(46, 17);
         this.label9.TabIndex = 22;
         this.label9.Text = "Disk:";
         this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // cbSearchTag
         // 
         this.cbSearchTag.DropDownHeight = 200;
         this.cbSearchTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbSearchTag.DropDownWidth = 200;
         this.cbSearchTag.FormattingEnabled = true;
         this.cbSearchTag.IntegralHeight = false;
         this.cbSearchTag.Location = new System.Drawing.Point(61, 64);
         this.cbSearchTag.Name = "cbSearchTag";
         this.cbSearchTag.Size = new System.Drawing.Size(121, 21);
         this.cbSearchTag.TabIndex = 21;
         this.cbSearchTag.SelectedIndexChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label8
         // 
         this.label8.Location = new System.Drawing.Point(6, 67);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(49, 17);
         this.label8.TabIndex = 20;
         this.label8.Text = "Tag:";
         this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // cbSearchArchieved
         // 
         this.cbSearchArchieved.Location = new System.Drawing.Point(304, 11);
         this.cbSearchArchieved.Name = "cbSearchArchieved";
         this.cbSearchArchieved.Size = new System.Drawing.Size(110, 24);
         this.cbSearchArchieved.TabIndex = 19;
         this.cbSearchArchieved.Text = "Archivierte Filme";
         this.cbSearchArchieved.ThreeState = true;
         this.cbSearchArchieved.UseVisualStyleBackColor = true;
         this.cbSearchArchieved.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearchChannels
         // 
         this.cbSearchChannels.Checked = true;
         this.cbSearchChannels.CheckState = System.Windows.Forms.CheckState.Indeterminate;
         this.cbSearchChannels.Location = new System.Drawing.Point(199, 88);
         this.cbSearchChannels.Name = "cbSearchChannels";
         this.cbSearchChannels.Size = new System.Drawing.Size(41, 24);
         this.cbSearchChannels.TabIndex = 18;
         this.cbSearchChannels.Text = "5.1";
         this.cbSearchChannels.ThreeState = true;
         this.cbSearchChannels.UseVisualStyleBackColor = true;
         this.cbSearchChannels.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearchSdHd
         // 
         this.cbSearchSdHd.DropDownHeight = 200;
         this.cbSearchSdHd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbSearchSdHd.DropDownWidth = 50;
         this.cbSearchSdHd.FormattingEnabled = true;
         this.cbSearchSdHd.IntegralHeight = false;
         this.cbSearchSdHd.Items.AddRange(new object[] {
         "",
         "SD",
         "720p",
         "1080p"});
         this.cbSearchSdHd.Location = new System.Drawing.Point(347, 88);
         this.cbSearchSdHd.Name = "cbSearchSdHd";
         this.cbSearchSdHd.Size = new System.Drawing.Size(58, 21);
         this.cbSearchSdHd.TabIndex = 17;
         this.cbSearchSdHd.SelectedIndexChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label7
         // 
         this.label7.Location = new System.Drawing.Point(295, 93);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(46, 17);
         this.label7.TabIndex = 16;
         this.label7.Text = "SD/HD:";
         this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // cbSearchUnknown
         // 
         this.cbSearchUnknown.Checked = true;
         this.cbSearchUnknown.CheckState = System.Windows.Forms.CheckState.Indeterminate;
         this.cbSearchUnknown.Location = new System.Drawing.Point(426, 88);
         this.cbSearchUnknown.Name = "cbSearchUnknown";
         this.cbSearchUnknown.Size = new System.Drawing.Size(116, 24);
         this.cbSearchUnknown.TabIndex = 15;
         this.cbSearchUnknown.Text = "Unbekannte Filme";
         this.cbSearchUnknown.ThreeState = true;
         this.cbSearchUnknown.UseVisualStyleBackColor = true;
         this.cbSearchUnknown.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearchFavourite
         // 
         this.cbSearchFavourite.Checked = true;
         this.cbSearchFavourite.CheckState = System.Windows.Forms.CheckState.Indeterminate;
         this.cbSearchFavourite.Location = new System.Drawing.Point(426, 36);
         this.cbSearchFavourite.Name = "cbSearchFavourite";
         this.cbSearchFavourite.Size = new System.Drawing.Size(116, 24);
         this.cbSearchFavourite.TabIndex = 14;
         this.cbSearchFavourite.Text = "Favoriten";
         this.cbSearchFavourite.ThreeState = true;
         this.cbSearchFavourite.UseVisualStyleBackColor = true;
         this.cbSearchFavourite.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearchWatched
         // 
         this.cbSearchWatched.Location = new System.Drawing.Point(426, 11);
         this.cbSearchWatched.Name = "cbSearchWatched";
         this.cbSearchWatched.Size = new System.Drawing.Size(116, 24);
         this.cbSearchWatched.TabIndex = 13;
         this.cbSearchWatched.Text = "Gesehene Filme";
         this.cbSearchWatched.ThreeState = true;
         this.cbSearchWatched.UseVisualStyleBackColor = true;
         this.cbSearchWatched.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearch3D
         // 
         this.cbSearch3D.Checked = true;
         this.cbSearch3D.CheckState = System.Windows.Forms.CheckState.Indeterminate;
         this.cbSearch3D.Location = new System.Drawing.Point(255, 88);
         this.cbSearch3D.Name = "cbSearch3D";
         this.cbSearch3D.Size = new System.Drawing.Size(41, 24);
         this.cbSearch3D.TabIndex = 12;
         this.cbSearch3D.Text = "3D";
         this.cbSearch3D.ThreeState = true;
         this.cbSearch3D.UseVisualStyleBackColor = true;
         this.cbSearch3D.CheckStateChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // cbSearchGenre
         // 
         this.cbSearchGenre.DropDownHeight = 200;
         this.cbSearchGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbSearchGenre.DropDownWidth = 200;
         this.cbSearchGenre.FormattingEnabled = true;
         this.cbSearchGenre.IntegralHeight = false;
         this.cbSearchGenre.Location = new System.Drawing.Point(61, 88);
         this.cbSearchGenre.Name = "cbSearchGenre";
         this.cbSearchGenre.Size = new System.Drawing.Size(121, 21);
         this.cbSearchGenre.TabIndex = 11;
         this.cbSearchGenre.SelectedIndexChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label6
         // 
         this.label6.Location = new System.Drawing.Point(6, 91);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(49, 17);
         this.label6.TabIndex = 10;
         this.label6.Text = "Genre:";
         this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // tbSearchDirector
         // 
         this.tbSearchDirector.Location = new System.Drawing.Point(255, 38);
         this.tbSearchDirector.MaxLength = 100;
         this.tbSearchDirector.Name = "tbSearchDirector";
         this.tbSearchDirector.Size = new System.Drawing.Size(150, 20);
         this.tbSearchDirector.TabIndex = 9;
         this.tbSearchDirector.TextChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label5
         // 
         this.label5.Location = new System.Drawing.Point(188, 41);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(61, 17);
         this.label5.TabIndex = 8;
         this.label5.Text = "Director:";
         this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // tbSearchActor
         // 
         this.tbSearchActor.Location = new System.Drawing.Point(61, 38);
         this.tbSearchActor.MaxLength = 100;
         this.tbSearchActor.Name = "tbSearchActor";
         this.tbSearchActor.Size = new System.Drawing.Size(121, 20);
         this.tbSearchActor.TabIndex = 7;
         this.tbSearchActor.TextChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label4
         // 
         this.label4.Location = new System.Drawing.Point(6, 41);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(49, 17);
         this.label4.TabIndex = 6;
         this.label4.Text = "Actor:";
         this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // dgvMain
         // 
         this.dgvMain.AllowUserToAddRows = false;
         this.dgvMain.AllowUserToDeleteRows = false;
         this.dgvMain.AllowUserToResizeRows = false;
         this.dgvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
         | System.Windows.Forms.AnchorStyles.Left) 
         | System.Windows.Forms.AnchorStyles.Right)));
         this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvMain.Location = new System.Drawing.Point(6, 120);
         this.dgvMain.Name = "dgvMain";
         this.dgvMain.ReadOnly = true;
         this.dgvMain.RowHeadersVisible = false;
         this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
         this.dgvMain.Size = new System.Drawing.Size(536, 582);
         this.dgvMain.TabIndex = 3;
         this.dgvMain.SelectionChanged += new System.EventHandler(this.DgvMainSelectionChanged);
         // 
         // tbSearchRating
         // 
         this.tbSearchRating.Location = new System.Drawing.Point(255, 16);
         this.tbSearchRating.MaxLength = 1;
         this.tbSearchRating.Name = "tbSearchRating";
         this.tbSearchRating.Size = new System.Drawing.Size(25, 20);
         this.tbSearchRating.TabIndex = 3;
         this.tbSearchRating.TextChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(188, 19);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(63, 17);
         this.label2.TabIndex = 2;
         this.label2.Text = "Min.Rating:";
         this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // tbSearchTitle
         // 
         this.tbSearchTitle.Location = new System.Drawing.Point(61, 16);
         this.tbSearchTitle.MaxLength = 100;
         this.tbSearchTitle.Name = "tbSearchTitle";
         this.tbSearchTitle.Size = new System.Drawing.Size(121, 20);
         this.tbSearchTitle.TabIndex = 1;
         this.tbSearchTitle.TextChanged += new System.EventHandler(this.SearchFieldChanged);
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(6, 19);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(49, 17);
         this.label1.TabIndex = 0;
         this.label1.Text = "Titel:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // FrmMain
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1092, 809);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.btnFind);
         this.Controls.Add(this.tbQuery);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ssMain);
         this.Controls.Add(this.menuStripMain);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MainMenuStrip = this.menuStripMain;
         this.MinimumSize = new System.Drawing.Size(1078, 836);
         this.Name = "FrmMain";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "FilmInfo";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMainFormClosing);
         this.ssMain.ResumeLayout(false);
         this.ssMain.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbPoster)).EndInit();
         this.menuStripMain.ResumeLayout(false);
         this.menuStripMain.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.ComboBox cbSearchVolumeLabel;
      private System.Windows.Forms.ComboBox cbSearchTag;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Button btnSaveTagList;
      private System.Windows.Forms.TextBox tbTags;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.ToolStripMenuItem btnSave;
      private System.Windows.Forms.Label myRating;
      private System.Windows.Forms.ToolStripMenuItem btnRefreshWebServer;
      private System.Windows.Forms.ToolStripMenuItem btnExportCsv;
      private System.Windows.Forms.ToolStripMenuItem btnOptions;
      private System.Windows.Forms.CheckBox cbArchieved;
      private System.Windows.Forms.CheckBox cbSearchArchieved;
      private System.Windows.Forms.ToolStripMenuItem btnShowDoubleEntries;
      private System.Windows.Forms.ToolStripMenuItem btnRefreshItems;
      private System.Windows.Forms.ToolStripMenuItem btnScanDirBulk;
      private System.Windows.Forms.LinkLabel lShowMovieThumbnail;
      private System.Windows.Forms.LinkLabel lYoutubeSearch;
      private System.Windows.Forms.LinkLabel lMetacriticSearch;
      private System.Windows.Forms.ToolStripMenuItem btnShowHttpServerStats;
      private System.Windows.Forms.ToolStripMenuItem btnHttpServerOpen;
      private System.Windows.Forms.ToolStripMenuItem btnStartStopHttpServer;
      private System.Windows.Forms.ToolStripMenuItem webserverToolStripMenuItem;
      private System.Windows.Forms.CheckBox cbSearchChannels;
      private System.Windows.Forms.ComboBox cbSearchSdHd;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.ToolStripMenuItem cbGridViewDateByAdd;
      private System.Windows.Forms.ToolStripMenuItem cbGridViewDateByRelease;
      private System.Windows.Forms.ToolStripMenuItem cbGridViewDateByFile;
      private System.Windows.Forms.ToolStripMenuItem datumInFilmlisteFestlegenToolStripMenuItem;
      private System.Windows.Forms.Label lFileDetails;
      private System.Windows.Forms.CheckBox cbSearchUnknown;
      private System.Windows.Forms.CheckBox cbSearchFavourite;
      private System.Windows.Forms.CheckBox cbFavourite;
      private System.Windows.Forms.CheckBox cbSearchWatched;
      private System.Windows.Forms.CheckBox cbWatched;
      private System.Windows.Forms.CheckBox cbSearch3D;
      private System.Windows.Forms.ComboBox cbSearchGenre;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ToolStripMenuItem btnAddDir;
      private System.Windows.Forms.LinkLabel lAmazonSearch;
      private System.Windows.Forms.Label lDriveLabel;
      private System.Windows.Forms.TextBox tbSearchDirector;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.MenuStrip menuStripMain;
      private System.Windows.Forms.TextBox tbSearchActor;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox tbSearchRating;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox tbSearchTitle;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.ToolStripMenuItem btnClearImageCache;
      private System.Windows.Forms.ToolStripMenuItem cacheToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem btnRemoveSelected;
      private System.Windows.Forms.ToolStripMenuItem btnScanDir;
      private System.Windows.Forms.ToolStripMenuItem filmlisteToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem btnClose;
      private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
      private System.Windows.Forms.Button btnAkaChange;
      private System.Windows.Forms.LinkLabel lFilename;
      private System.Windows.Forms.LinkLabel lImdbSearch;
      private System.Windows.Forms.Label lDirectors;
      private System.Windows.Forms.Label lGenres;
      private System.Windows.Forms.LinkLabel llImdbUrl;
      private System.Windows.Forms.Label lActors;
      private System.Windows.Forms.Label lPlot;
      private System.Windows.Forms.Label lYearRuntime;
      private System.Windows.Forms.Label lRating;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.FolderBrowserDialog folderDialog;
      private System.Windows.Forms.PictureBox pbPoster;
      private System.Windows.Forms.ToolStripProgressBar pbMain;
      private System.Windows.Forms.ToolStripStatusLabel lStatus;
      private System.Windows.Forms.StatusStrip ssMain;
      private System.Windows.Forms.DataGridView dgvMain;
      private System.Windows.Forms.Button btnFind;
      private System.Windows.Forms.TextBox tbQuery;
      
      
   }
}
