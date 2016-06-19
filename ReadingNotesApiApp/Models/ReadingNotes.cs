using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadingNotesApiApp.Models
{
    public class ReadingNotes
    {
        public ReadingNotes() {
            Notes = new Hashtable();
        }

        public string Title { get; set; }
        public Hashtable Notes { get; set; }
    }
}