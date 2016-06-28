using ReadingNotesApiApp.Helpers;
using ReadingNotesApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReadingNotesApiApp.Controllers
{
    public class ConfigController : ApiController
    {
        // GET: api/Config
        public Config Get()
        {
            return Models.Config.CreateFromString(StorageHelper.GetConfig());
        }

        // POST: api/Config
        public void Post([FromBody]Config value)
        {
            StorageHelper.SaveConfiToStorage(value.Serialize());
        }

        // POST: api/IncrementCounter
        [Route("IncrementCounter")]
        public void IncrementCounter()
        {
            Config config = Models.Config.CreateFromString(StorageHelper.GetConfig());
            config.reading_notes_counter += 1;
            StorageHelper.SaveConfiToStorage(config.Serialize());
        }
    }
}
