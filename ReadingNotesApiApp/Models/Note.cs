using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadingNotesApiApp.Models
{
    public class Note
    {
            public string Title { get; set; }
            public string Author { get; set; }
            public string DateAdded { get; set; }
            public string Tags { get; set; }
            public string Comment { get; set; }
            public string Url { get; set; }

            public string Category { get; set; }
    }
}