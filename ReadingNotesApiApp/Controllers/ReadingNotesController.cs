using ReadingNotesApiApp.Data;
using ReadingNotesApiApp.Helpers;
using ReadingNotesApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ReadingNotesApiApp.Controllers
{
    public class ReadingNotesController : ApiController
    {

        /// <summary>
        /// Build a JSon file with all notes regroup by category and saved it on a Blob storage
        /// </summary>
        /// <returns></returns>
        [Route("BuildReadingNotes")]
        [HttpGet]
        public ReadingNotes BuildReadingNotes()
        {

            var notes = StorageHelper.GetAllNotefromStorage();
            var readingNotes = new ReadingNotes();

            List<string> allTags = new List<string>();

            foreach (var n in notes) {

                allTags = KeepUniqueTag(allTags, n.Tags);
                n.Category = GetCategory(n.Tags);

                // little tweak because it's a pain to set the first caracter uppercase on the kindle.
                n.Comment = n.Comment.First().ToString().ToUpper() + String.Join("", n.Comment.Skip(1));

                if (readingNotes.Notes[n.Category] == null)
                    readingNotes.Notes[n.Category] = new List<Note>();

                ((List<Note>)readingNotes.Notes[n.Category]).Add(n);
            }

            readingNotes.Title = "Reading Notes #238";
            readingNotes.Tags = string.Join(",", allTags.OrderBy(c => c));

            StorageHelper.SaveJSonReadingNotesToStorage(readingNotes.Serialize());
            StorageHelper.SaveReadingNotesToStorage(readingNotes.ToMarkDown());

            return readingNotes;

        }

        [Route("ReProcessReadingNotes")]
        [HttpGet]
        public string ReProcessJSonReadingNotes(string Filename) {

            ReadingNotes readNotes = ReadingNotes.CreateFromString(StorageHelper.GetJSonReadingNotes(Filename));
            var mdNotes = readNotes.ToMarkDown();

            StorageHelper.SaveReadingNotesToStorage(mdNotes);

            return mdNotes;
        }


        private List<string> KeepUniqueTag(List<string> allTags, string noteTags) {

            if (!String.IsNullOrEmpty(noteTags))
            {
                var newListTgas = noteTags.Split('.');

                foreach (var tag in newListTgas)
                {
                    if (!allTags.Contains(tag))
                    {
                        allTags.Add(tag);
                    }
                }
            }
            return allTags;
        }


        private string GetCategory(string noteTags)
        {
            string category = "misc";
            var readingNoteCategories = string.Empty;

            if (!String.IsNullOrEmpty(noteTags))
            {
                var newListTgas = noteTags.Split('.');

                if (ReadingNoteCategories.GetCategories.ContainsKey(newListTgas[0]))
                    category = newListTgas[0];
            }

            return ReadingNoteCategories.GetCategories[category];
        }

    }
}
