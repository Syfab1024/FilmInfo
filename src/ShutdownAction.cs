/*
 * Erstellt mit SharpDevelop.
 * Benutzer: schulz
 * Datum: 20.03.2010
 * Zeit: 13:46
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using Org.Mentalis.Utilities;

namespace FilmInfo
{
    /// <summary>
    /// Description of Action.
    /// </summary>
    public class ShutdownAction
    {
        public string Name;
        public string ShortName;
        
        public ShutdownAction(string name, string shortName)
        {
            this.Name = name;
            this.ShortName = shortName;
        }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
