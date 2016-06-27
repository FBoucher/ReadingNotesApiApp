using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ReadingNotesApiApp.Models
{
    /// <summary>
    /// List of notes re-group by category 
    /// </summary>
    public class ReadingNotes
    {
        public ReadingNotes() {
            Notes = new Hashtable();
        }

        public string Title { get; set; }
        public string Tags { get; set; }
        public Hashtable Notes { get; set; }


        public string Serialize() {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Return a Markdown string representing the instance.
        /// </summary>
        /// <returns></returns>
        public string ToMarkDown() {

            var md = new StringBuilder();

            //== YAML header
            md.AppendFormat("---{0}", Environment.NewLine);
            md.Append(string.Format("Title: {0}{1}",Title, Environment.NewLine));
            md.Append(string.Format("Tags: {0}{1}", Tags, Environment.NewLine));
            md.AppendFormat("---{0}", Environment.NewLine);

            md.Append(Title + Environment.NewLine);
            md.Append('=',Title.Length);

            //== All Notes
            foreach (var key in this.Notes.Keys) {

                md.AppendFormat("{0}{0}## {1}{0}{0}", Environment.NewLine, key);

                foreach (var note in ((List<Note>)Notes[key]))
                {
                    md.Append(note.ToMarkDown() + Environment.NewLine);
                }

            }

            return md.ToString();
        }



        public static ReadingNotes CreateFromString(string DeserializeObj)
        {
            var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadingNotes>(DeserializeObj);
            var deserializeList = new Hashtable();

            var keys = temp.Notes.Keys;

            foreach (var category in keys)
            {
                var lstNotes = ((Newtonsoft.Json.Linq.JArray)temp.Notes[category]).ToObject<List<Note>>();
                deserializeList[category] = lstNotes;
            }
            temp.Notes = deserializeList;
            return temp;
        }

    }
}