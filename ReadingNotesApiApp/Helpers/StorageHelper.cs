using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;

namespace ReadingNotesApiApp.Helpers
{
    public static class StorageHelper
    {

        private static CloudStorageAccount AzureStorage()
        {
            return CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerName"></param>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public static Stream GetStreamFromStorage(string ContainerName, string Filename)
        {
            CloudStorageAccount storageAccount = AzureStorage();
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);
            var blockBlob = container.GetBlockBlobReference(Filename);

            using (var memoryStream = new MemoryStream())
            {

                blockBlob.DownloadToStream(memoryStream);
                byte[] byteArray = memoryStream.ToArray();

                MemoryStream stream = new MemoryStream(byteArray);
                return stream;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerName"></param>
        /// <param name="FileContent"></param>
        /// <returns></returns>
        public static string SaveResultToStorage(string ContainerName, string FileContent)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);

            var filename = "myclippings_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadText(FileContent);

            return filename;
        }


        public static IEnumerable<ReadingNotesApiApp.Models.Note> GetAllNotefromStorage(string ContainerName)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);


            var blobs = container.ListBlobs(null,false);
            var Notes = new Collection<ReadingNotesApiApp.Models.Note>();

            foreach (var blobItem in blobs)
            {
                // This was required because System.IO.Path.GetFileName failled with special caracters
                var filename = blobItem.Uri.LocalPath.Replace(string.Concat("/",ContainerName,"/"), "");

                try
                {
                    var blockBlob = container.GetBlockBlobReference(filename);

                    var noteData = blockBlob.DownloadText();
                    var readNote = JsonConvert.DeserializeObject<ReadingNotesApiApp.Models.Note>(noteData);
                    Notes.Add(readNote);
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(String.Format("Problem with file: {0}. Error: {1}",filename,ex.Message));
                }
            }

            return Notes;
        }



    }
}
