using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ReadingNotesApiApp.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Note
    {

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        public string Author { get; set; }
        public string DateAdded { get; set; }
        public string Tags { get; set; }
        public string Comment { get; set; }
            
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToMarkDown()
        {

            var md = new StringBuilder();               

            md.AppendFormat("{0}- ", Environment.NewLine);
            if (!string.IsNullOrEmpty(Url))
            {
                md.AppendFormat("**[{0}]({1})** ", Title, Url);
            }
            else {
                md.AppendFormat("**[{0}](#)** ", Title);
            }

            if (!string.IsNullOrEmpty(Author))
                md.AppendFormat(" ({0}) ", Author);

            md.AppendFormat("- {0}", Comment);

            return md.ToString();
        }


        public static Note CreateFromString(string DeserializeObj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Note>(DeserializeObj);
           
        }
    }


}