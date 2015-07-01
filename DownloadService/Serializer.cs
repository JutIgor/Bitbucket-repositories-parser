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

        public static void SerializeDownloader(RepositoriesDownloader downloader, string name)
        {
            string localDownloaderName = string.Format(localDownloader, name);
            using (var file = new FileStream(localDownloaderName, FileMode.OpenOrCreate))
            {
                var serializer = new DataContractSerializer(typeof(RepositoriesDownloader));
                serializer.WriteObject(file, downloader);
            }
        }

        public static RepositoriesDownloader DeserializeDownloader(string name)
        {
            string localDownloaderName = string.Format(localDownloader, name);
            var serializer = new DataContractSerializer(typeof(RepositoriesDownloader));
            using (var file = new FileStream(localDownloaderName, FileMode.Open))
            {
                using (var reader = XmlDictionaryReader.CreateTextReader(file, new XmlDictionaryReaderQuotas()))
                {
                    var downloader = (RepositoriesDownloader)serializer.ReadObject(reader);
                    downloader.AllocateMemoryForDownloads();
                    return downloader;
                }
            }
        }
    }
}
