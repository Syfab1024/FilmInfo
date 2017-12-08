

// wenn Seite fertig geladen ist:
$(document).ready(function()
{
   // Click-Events für Checkboxen (Gesehen, Archiviert, Favorit) anmelden
   $("#watched").change(
      function(event) 
      {
         setMovieAttribute("watched");         
      }
   );

   $("#archived").change(
      function(event) 
      {
         setMovieAttribute("archived");         
      }
   );

   $("#favourite").change(
      function(event) 
      {
         setMovieAttribute("favourite");
      }
   );         
   
   $("#shutdownPc").click(
      function(event) 
      {
         shutdownPc()
      }
   );        
              
              

   // damit JSON als Text/Plain empfangen und versendet wird
   $.ajaxSetup({"beforeSend": function(xhr){
      if (xhr.overrideMimeType)
         xhr.overrideMimeType("text/plain");
      }
   });

   // Setting up a loading indicator using Ajax Events
   $(document).ajaxStart(function() 
   {
      $.mobile.loading("show", {
         textVisible: false
      });
   }).ajaxStop(function() 
   {      
      $.mobile.loading("hide");
   });                   
});

/**************************************************************
   FUNCTIONS
**************************************************************/


// führt auf Serverebene eine Aktion mit dem Film aus (AJAX)
// myAction := "watched" | "archived" | "favourite"
function setMovieAttribute(myAction)
{
   var selector = "#" + myAction;

   // Using the core $.ajax() method
   $.ajax({
      // the URL for the request
      url: "ajax_test.txt",
      
      // the data to send (will be converted to a query string)
      data: {
         movie_id: _MovieId,
         action: myAction,
         checked: $(selector).prop("checked")
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
         // Checkbox gemäß Antwort von Server setzen
         $(selector).each(function(){ this.checked = json.NewCheckedState; });
         $(selector).checkboxradio("refresh");
         
         // ggf. Gesehen-Datum anzeigen (wenn CheckBox gecheckt)
         if (myAction == "watched")
         {            
            if ($(selector).prop("checked") == true)
            {
               $(selector + "Text").text("Gesehen (" + json.Datum + ")");
            }
            else
            {
               $(selector + "Text").text("Gesehen");
            }            
         }
         
         console.log( _MovieId );
      },
      
      
      // code to run if the request fails; the raw request and status codes are passed to the function
      error: function( xhr, status, errorThrown ) {
         alert( "Sorry, there was a problem: " + errorThrown + ", " + status );
         //console.dir( xhr );
      },
      
      
      // code to run regardless of success or failure
      complete: function( xhr, status ) {
         //alert( "The request is complete!" );
      }
   });       
}

// PC herunterfahren -> ConfirmDialog, dann ggf. Umleitung auf shutdown-Seite
function shutdownPc() {
   ShowConfirmDialog("Sicherheitsabfrage", 
              "Wollen sie den PC wirklich herunterfahren?", "Ja", "Nein",
              function() 
                  {
                    window.location = "shutdown";
                  }
             );      
} 
   
// generischer Confirm-Dialog, benutzt div-id "MyConfirmDlg"
function ShowConfirmDialog(title, description, yesText, noText, callback) {
  $("#MyConfirmDlg .MyConfirmDlgTitle").text(title);
  $("#MyConfirmDlg .MyConfirmDlgDescription").text(description);
  $("#MyConfirmDlg .MyConfirmDlgBtnNo").text(noText);
  $("#MyConfirmDlg .MyConfirmDlgBtnYes").text(yesText).on("click.MyConfirmDlg", function() {
    callback();
    $(this).off("click.MyConfirmDlg");
  });
  $.mobile.changePage("#MyConfirmDlg");
}   
