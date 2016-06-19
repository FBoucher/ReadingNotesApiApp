

using System.Collections.Generic;

namespace ReadingNotesApiApp.Data
{
    public static class ReadingNoteCategories
    {

        public static readonly Dictionary<string, string> GetCategories = new Dictionary<string, string>
        {
            {"cloud", "Cloud"},
            {"top", "Suggestion of the week"},
            {"data", "Data"},
            {"misc", "Miscellaneous"},
            {"database", "Databases"},
            {"dev", "Programming"}
        };
    }
}