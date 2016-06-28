using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadingNotesApiApp.Models
{
    /// <summary>
    /// Config that keep tracks of the last ReadingNotes
    /// </summary>
    public class Config
    {
        public string last_bookmark_date { get; set; }

        public int reading_notes_counter { get; set; }

        public static Config CreateFromString(string DeserializeObj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(DeserializeObj);
        }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }


}