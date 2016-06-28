using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

namespace ReadingNotesApiApp.Helpers
{
    public static class StorageHelper
    {

        private static CloudStorageAccount AzureStorage()
        {
            return CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
        }

        private static CloudBlobClient BlobClient() {
            CloudStorageAccount storageAccount = AzureStorage();
            return storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContainerName"></param>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public static Stream GetStreamFromStorage(string ContainerName, string Filename)
        {
            var blobClient = BlobClient();
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
            var blobClient = BlobClient();
            var container = blobClient.GetContainerReference(ContainerName);

            var filename = "myclippings_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadText(FileContent);

            return filename;
        }

        private static string GetNotesContainerName() {
            return ConfigurationManager.AppSettings["NotesContainerName"];
        }

        private static string GetJSonReadingNotesContainerName()
        {
            return ConfigurationManager.AppSettings["JSonReadingNotesContainerName"];
        }

        private static string GetReadingNotesContainerName()
        {
            return ConfigurationManager.AppSettings["ReadingNotesContainerName"];
        }

        private static string GetReadingNotesSettingsContainerName() {
            return ConfigurationManager.AppSettings["ReadingNotesSettings"];
        }

        /// <summary>
        /// Get all the files in the container
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ReadingNotesApiApp.Models.Note> GetAllNotefromStorage()
        {
            string containerName = GetNotesContainerName();

            CloudBlobClient blobClient = BlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);


            var blobs = container.ListBlobs(null,false);
            var Notes = new Collection<ReadingNotesApiApp.Models.Note>();

            foreach (var blobItem in blobs)
            {
                string filename = ExtractFileNameFromBlobPath(blobItem, containerName);

                try
                {
                    var blockBlob = container.GetBlockBlobReference(filename);

                    string noteData = blockBlob.DownloadText();
                    Models.Note readNote = JsonConvert.DeserializeObject<ReadingNotesApiApp.Models.Note>(noteData);
                    Notes.Add(readNote);

                    blockBlob.DeleteIfExistsAsync();
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(String.Format("Problem with file: {0}. Error: {1}",filename,ex.Message));
                }
            }

            return Notes;
        }


        public static string GetJSonReadingNotes(string Filename) {

            string notesData = string.Empty;
            var stream = GetStreamFromStorage(GetJSonReadingNotesContainerName(), Filename);

            using (var sr = new System.IO.StreamReader(stream))
            {
                notesData = sr.ReadToEnd();
            }
            return notesData;
        }

        public static string SaveJSonReadingNotesToStorage(string JSonReadingNotes) {

            var blobClient = BlobClient();
            var container = blobClient.GetContainerReference(GetJSonReadingNotesContainerName());

            var filename = "readingnotes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadText(JSonReadingNotes);

            return filename;

        }

        public static string SaveReadingNotesToStorage(string JSonReadingNotes)
        {

            var blobClient = BlobClient();
            var container = blobClient.GetContainerReference(GetReadingNotesContainerName());

            var filename = "readingnotes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".md";
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadText(JSonReadingNotes);

            return filename;

        }

        public static string ExtractFileNameFromBlobPath(IListBlobItem blobItem, string containerName) {
            // This was required because System.IO.Path.GetFileName failled with special caracters
            return blobItem.Uri.LocalPath.Replace(string.Concat("/", containerName, "/"), "");
        }



        public static string GetConfig() {
            string strSettings = string.Empty;
            var stream = GetStreamFromStorage(GetReadingNotesSettingsContainerName(), "setting.json");

            using (var sr = new System.IO.StreamReader(stream))
            {
                strSettings = sr.ReadToEnd();
            }
            return strSettings;
        }

        public static string SaveConfiToStorage(string config)
        {
            var blobClient = BlobClient();
            var container = blobClient.GetContainerReference(GetReadingNotesSettingsContainerName());

            var filename = "setting.json";
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadText(config);

            return filename;
        }
    }
}
