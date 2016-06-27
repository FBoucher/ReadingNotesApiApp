using System.Collections.Generic;

namespace ReadingNotesApiApp.Data
{
    //TODO: MOve this to webconfig Section

    /// <summary>
    /// ReadingNotes Categories
    /// </summary>
    public static class ReadingNoteCategories
    {
        /// <summary>
        /// Get a dictionary to change the short version by the long version of category name.
        /// </summary>
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