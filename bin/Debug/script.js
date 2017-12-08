/*************************************************************************************************************
   wenn Seite fertig geladen ist...
*************************************************************************************************************/
$(document).ready(function()
{
   /*************************************************************************************************************
      Initialisierung
   *************************************************************************************************************/    
   // Initialisierung TagList
   addTagsToTagList(_TagList);
   addTagsToAutocompleteListView(_TagListComplete);

   // Initialisierung der Aktions-Checkboxen
   setActionCheckBox("#watched", _Watched, _WatchedDate);
   setActionCheckBox("#archived", _Archived, null);
   setActionCheckBox("#favourite", _Favourite, null);
  
  
   /*************************************************************************************************************
      Normale Events anmelden
   *************************************************************************************************************/  
   // Click-Events für Checkboxen (Gesehen, Archiviert, Favorit) anmelden
   $("#watched").change(function(event)     { setMovieAttribute("watched"); });
   $("#archived").change(function(event)    { setMovieAttribute("archived");});
   $("#favourite").change(function(event)   { setMovieAttribute("favourite");});         
   $("#SaveTagList").click(function(event)  { setTagList();});        
   $("#remoteExecute").click(function(event){ remoteExecute();});          
   $("#shutdownPc").click(function(event)   { shutdownPc();});                 
   $(".CloseWindow").click(function(event)  { window.close();});               
        
   /*************************************************************************************************************
      EventHandler für aktuelle UND zukünftige Controls (die von jQuery dynamisch erzeugt werden)
      (  $("input").on(...) reicht hier nicht, weil das nicht die dynamisch von jQuery erzeugten Controls abdeckt)
   *************************************************************************************************************/
   
   /*
   // --> für gefilterte ListView -> soll sich wie Autovervollständigung verhalten 
   $( document ).on( "focusout", "#TagListForm input", function(e) 
   {
      // wenn focus verlässt -> AutoComplete-List ausblenden
      var list = $("#TagListView");
      list.filterable( "option", "filterReveal", true);
      list.children().removeClass("ui-screen-hidden");    
      list.listview("refresh");      
   });   
   */
   
   // --> für gefilterte ListView -> soll sich wie Autovervollständigung verhalten 
   $( document ).on( "focus", "#TagListForm input", function(e) 
   {
      showTagListViewFilter($(this).val());
   });
   
   // --> für gefilterte ListView -> soll sich wie Autovervollständigung verhalten 
   $( document ).on( "keyup", "#TagListForm input", function(e) 
   {
       showTagListViewFilter($(this).val());
       
       // wurde Enter gedrückt? --> Tag zu Liste hinzufügen
       if (e.which == 13)
       {
         addTagsToTagList($("#TagListForm input").val());
         $("#TagListForm input").val("");
         $("#TagListForm input").focus();
       }
   });   
   
   // Hinzufügen eines Tags zur Liste bei Klick auf ListView-Tag-Link
   $( document ).on( "click", ".TagListViewItem", function(e) 
   {   
      addTagToTagList($(this).text());
   });   
   
   // Click-Event für Tags --> sollen bei Klick verschwinden
   $( document ).on( "click", ".tag", function(e) 
   {
      $(this).remove();
      if ($("#TagListChange").text() == "")
         $("#TagListChange").text("-");
   });

   // Swipe Left --> Page nach rechts wechseln
   $( document ).on( "swipeleft", "div.ui-page", function(e) 
   {
      var nextpage = $(this).next('div[data-role="page"]');
      // swipe using id of next page if exists
      if (nextpage.length > 0) 
      {
         // künstlichen Stopper einbauen, wenn div "Aktionen" erreicht wurde 
         // (weil es dann nix mehr zu sliden gibt)
         if (this.id != "Aktionen")
         {
            //$.mobile.changePage(nextpage, 'slide');
            $( ":mobile-pagecontainer" ).pagecontainer( "change", nextpage, { transition: "slide" } ); 
         }            
      }
   });
   
   // Swipe Right --> Page nach links wechseln   
   $( document ).on( "swiperight", "div.ui-page", function(e) 
   {
      var prevpage = $(this).prev('div[data-role="page"]');
      // swipe using id of prev page if exists
      if (prevpage.length > 0) 
      {
         // $.mobile.changePage(prevpage, 'slide', true);
         $( ":mobile-pagecontainer" ).pagecontainer( "change", prevpage, { transition: "slide" , reverse: true} ); 
      }
   });   
        
   /*************************************************************************************************************
      Konfiguration (Ajax)
   *************************************************************************************************************/  
   // damit JSON als Text/Plain empfangen und versendet wird
   $.ajaxSetup({"beforeSend": function(xhr){
      if (xhr.overrideMimeType)
         xhr.overrideMimeType("text/plain");
      }
   });

   // Setting up a loading indicator using Ajax Events
   $(document).ajaxStart(function() 
   {
      $("body").addClass('ui-disabled');
      $.mobile.loading("show", {
         textVisible: false
      });
   }).ajaxStop(function() 
   {      
      $.mobile.loading("hide");
      $("body").removeClass('ui-disabled');
   });    
   
}); // Ende von $(document).ready(function()




/**************************************************************
   FUNCTIONS
**************************************************************/

// Schickt die (kommasep.) TagList per Ajax an den Server
function setTagList(taglist)
{
   var tagList = $("#TagListChange").text();

   // Using the core $.ajax() method
   $.ajax({
      // the URL for the request
      url: "action.htm",
      
      // the data to send (will be converted to a query string)
      data: {
         movie_id: _MovieId,
         action: "taglist",
         taglist: tagList
      },
      
      // whether this is a POST or GET request
      type: "POST",
      
      // If set to false, it will force requested pages not to be cached by the browser
      cache: "false",
                     
      // the type of data we expect back
      dataType: "json",      
      
      // code to run if the request succeeds;
      // the response is passed to the function
      success: function( json ) 
      {
         // im Tag-Dialog die Tags zurücksetzen und wieder hinzufügen
         $("#TagListChange").text("");
         addTagsToTagList(json.NewTagList);
         // Tags auf der Hauptseite aktualisieren
         $( "#TagList" ).text(json.NewTagList);
      },      
      
      // code to run if the request fails; the raw request and status codes are passed to the function
      error: function( xhr, status, errorThrown ) {
         alert( "Sorry, there was a problem: " + errorThrown + ", " + status );
         //console.dir( xhr );
      }
   });
}

// führt auf Serverebene eine Aktion mit dem Film aus (AJAX)
// myAction := "watched" | "archived" | "favourite"
function setMovieAttribute(myAction)
{
   var selector = "#" + myAction;

   // Using the core $.ajax() method
   $.ajax({
      // the URL for the request
      url: "action.htm",
      
      // the data to send (will be converted to a query string)
      data: {
         movie_id: _MovieId,
         action: myAction,
         checked: $(selector).prop("checked"),
      },
      
      // whether this is a POST or GET request
      type: "POST",
      
      // If set to false, it will force requested pages not to be cached by the browser
      cache: "false",
                     
      // the type of data we expect back
      dataType: "json",      
      
      // code to run if the request succeeds;
      // the response is passed to the function
      success: function( json ) 
      {
         setActionCheckBox(selector, json.NewCheckedState, json.Datum);
      },      
      
      // code to run if the request fails; the raw request and status codes are passed to the function
      error: function( xhr, status, errorThrown ) {
         alert( "Sorry, there was a problem: " + errorThrown + ", " + status );
         //console.dir( xhr );
      }
   });       
}

// setzt/entfernt Häkchen einer CheckBox und schreibt ggf. noch das Watched-Datum rein
function setActionCheckBox(selector, checkState, dateWatched)
{
   // Häkchen setzen
   $(selector).each(function(){ 
      this.checked = checkState; 
   });
   
   // ggf. Gesehen-Datum anzeigen (wenn CheckBox gecheckt)
   if (selector == "#watched")
   {            
      if ($(selector).prop("checked") == true)
      {
         $(selector + "Text").text("Gesehen (" + dateWatched + ")");
      }
      else
      {
         $(selector + "Text").text("Gesehen");
      }            
   }
}

// Aufruf des serverseitig festgelegten Programms
function remoteExecute()
{
   $( ":mobile-pagecontainer" ).pagecontainer( "change", "execute", { role: "dialog" } ); 
}

// PC herunterfahren -> ConfirmDialog, dann ggf. Umleitung auf shutdown-Seite
function shutdownPc() 
{
   ShowConfirmDialog(
      "Sicherheitsabfrage", 
      "Wollen sie den PC wirklich herunterfahren?", "Ja", "Nein",
      function() 
      {
         $( ":mobile-pagecontainer" ).pagecontainer( "change", "shutdown", { role: "dialog" } );
      }
   );      
} 
   
// Texte in generischen Confirm-Dialog befüllen und anzeigen
// (benutzt div-id "MyConfirmDlg")
function ShowConfirmDialog(title, description, yesText, noText, callback) 
{
   // Texte der Controls im Confirm-Dialog festlegen
   $("#MyConfirmDlgTitle").text(title);
   $("#MyConfirmDlgDescription").text(description);
   $("#MyConfirmDlgBtnNo").text(noText);
   $("#MyConfirmDlgBtnYes").text(yesText);
  
   // was soll beim Klick auf "ja" passieren?
   $("#MyConfirmDlgBtnYes").on("click.MyConfirmDlg", function() 
   {
      // callback aufrufen
      callback();
      // remove event handler
      $(this).off("click.MyConfirmDlg");
   });
   
   // Dialog anzeigen
   $( ":mobile-pagecontainer" ).pagecontainer( "change", "#MyConfirmDlg" );
}   


// für gefilterte ListView -> soll sich wie Autovervollständigung verhalten 
//  --> bei Klick in zugehöriges Textfeld soll sich Liste (ungefiltert) öffnen und alles anzeigen
//     --> beim Schreiben in zugehöriges Textfeld soll Liste gefiltert werden
function showTagListViewFilter(inputFieldText)
{
    var list = $("#TagListView");
    
    if (inputFieldText != "")
    {
       list.filterable( "option", "filterReveal", true);
       list.children().addClass("ui-screen-hidden");    
    }
    else
    {
       list.filterable( "option", "filterReveal", false );
       list.children().removeClass("ui-screen-hidden");    
    }

    list.listview("refresh");
}

// fügt Komma/Leerzeichenseparierte Liste von Tags der TagListe hinzu
// TagListe: Grün hinterlegte Tags im Dialog
function addTagsToTagList(tagToAdd)
{
   tagToAdd = tagToAdd.replace(/\s/g, ",");
   var tagArray = tagToAdd.split(",");
   $.each(tagArray, function(index, value) 
   {
      addTagToTagList(value);
   });
}

// fügt ein einzelnes Tag der Tagliste hinzu
// TagListe: Grün hinterlegte Tags im Dialog
function addTagToTagList(tagToAdd)
{
   var tagList = $("#TagListChange");
   
   tagToAdd = tagToAdd.trim();
   
   var tagArray = tagList.text().split(" ");
   var tagFound = false;
   $.each(tagArray, function(index, value) 
   {
      if (value.trim().toLowerCase() == tagToAdd.toLowerCase())
      {
         tagFound = true;
      }
   });
   
   // wenn Tag bereits vorhanden --> nix machen
   if (tagFound || tagToAdd == "-")
      return;
   
   // grüne Markierung um Tags hinzufügen
   tagToAdd = '<span class="tag">' + tagToAdd + '</span> ';

   if (tagList.text() == "-")
   {
      tagList.text("");
   }
   tagList.append(tagToAdd);
}

// fügt eine kommasep. Liste von Tags zur Autocomplete-List hinzu
function addTagsToAutocompleteListView(tagList)
{
   tagList = tagList.replace(/\s/g, ",");
   var tagArray = tagList.split(",");
   $.each(tagArray, function(index, value) 
   {   
      if (value != "")
         $("#TagListView").append('<li><a href="#" class="TagListViewItem">' + value + '</a></li>');
   });
}
