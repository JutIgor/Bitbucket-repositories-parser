using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace DownloadService
{
    public class Serializer
    {
        private static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string localDownloader = appDirectory + "Downloader{0}.xml";

        public static void SerializeDownloader(RepositoriesDownloader downloader, string language)
        {
            string localDownloaderName = string.Format(localDownloader, language);
            using (var file = new FileStream(localDownloaderName, FileMode.OpenOrCreate))
            {
                var serializer = new DataContractSerializer(typeof(RepositoriesDownloader));
                serializer.WriteObject(file, downloader);
            }
        }

        public static RepositoriesDownloader DeserializeDownloader(string language)
        {
            string localDownloaderName = string.Format(localDownloader, language);
            if (new FileInfo(localDownloaderName).Length == 0) return new RepositoriesDownloader();
            var serializer = new DataContractSerializer(typeof(RepositoriesDownloader));
            using (var file = new FileStream(localDownloaderName, FileMode.OpenOrCreate)) // Check FileMode
            {
                using (var reader = XmlDictionaryReader.CreateTextReader(file, new XmlDictionaryReaderQuotas()))
                {
                    var downloader = (RepositoriesDownloader)serializer.ReadObject(reader);
                    downloader.AllocateMemoryForDownloads();
                    return downloader;
                }
            }
        }

        public static void ClearDownloader(string language)
        {
            string localDownloaderName = string.Format(localDownloader, language);
            File.WriteAllText(localDownloaderName, string.Empty);
        }
    }
}
