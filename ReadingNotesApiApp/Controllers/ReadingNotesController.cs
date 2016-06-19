using ReadingNotesApiApp.Data;
using ReadingNotesApiApp.Helpers;
using ReadingNotesApiApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ReadingNotesApiApp.Controllers
{
    public class ReadingNotesController : ApiController
    {


        // POST api/values
        //[SwaggerOperation("Create")]
        //[SwaggerResponse(HttpStatusCode.Created)]
        public ReadingNotes Get()
        {

            var notes = StorageHelper.GetAllNotefromStorage("clippings");
            var readingNotes = new ReadingNotes();

            readingNotes.Title = "Reading Notes #234";
            List<string> allTags = new List<string>();

            foreach (var n in notes) {

                KeepUniqueTag(allTags, n.Tags);
                n.Category = GetCategory(n.Tags);

                if (readingNotes.Notes[n.Category] == null)
                {
                    readingNotes.Notes[n.Category] = new List<Note>();
                }

                ((List<Note>)readingNotes.Notes[n.Category]).Add(n);
            }

            var json = new JavaScriptSerializer().Serialize(readingNotes);


            return readingNotes;

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

            if (!String.IsNullOrEmpty(noteTags))
            {
                var newListTgas = noteTags.Split('.');

                category = newListTgas[0];

            }

            var readingNoteCategories = ReadingNoteCategories.GetCategories[category];
            
            if(!string.IsNullOrEmpty(readingNoteCategories))
                category = readingNoteCategories;

            return category;

        }

    }
}
