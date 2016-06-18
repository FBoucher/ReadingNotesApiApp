using ReadingNotesApiApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReadingNotesApiApp.Controllers
{
    public class ReadingNotesController : ApiController
    {


        // POST api/values
        //[SwaggerOperation("Create")]
        //[SwaggerResponse(HttpStatusCode.Created)]
        public IEnumerable<ReadingNotesApiApp.Models.Note> Get()
        {

            var notes = StorageHelper.GetAllNotefromStorage("clippings");
            List<string> allTags = new List<string>();

            foreach (var n in notes) {

                KeepUniqueTag(allTags, n.Tags);
                n.Category = GetCategory(n.Tags);
            }

            return notes;

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
            var category = "misc";

            if (!String.IsNullOrEmpty(noteTags))
            {
                var newListTgas = noteTags.Split('.');

                category = newListTgas[0];

            }
            return category;

        }

    }
}
