using RepositoriesDownloader;
using RepositoriesParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {
        private static Task<string> finishedTask = null;
        private static List<Task<string>> downloads = new List<Task<string>>();
        private static Loader loader = new Loader();

        static void Main(string[] args)
        {
            var fileName = string.Empty;

            var newDirectoryPath = string.Format(Pathes.HtmlCssFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);
            newDirectoryPath = string.Format(Pathes.JavaScriptFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);

            try
            {
                var HtmlCssusers = GetUsers(Pathes.HtmlCssUsersFile, Language.HtmlCss);
                var JavaScriptUsers = GetUsers(Pathes.JavaScriptUsersFile, Language.JavaScript);
                var HtmlCssRepositories = GetRepositories(Pathes.HtmlCssRepositoriesFile, HtmlCssusers, Language.HtmlCss);
                var JavaScriptRepositories = GetRepositories(Pathes.JavaScriptRepositoriesFile, JavaScriptUsers, Language.JavaScript);

                DownloadRepositories(Pathes.HtmlCssRepositoriesFile, Pathes.HtmlCssFolder);
                DownloadRepositories(Pathes.JavaScriptRepositoriesFile, Pathes.JavaScriptFolder);

            }
            catch (WebException ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }

        private static Dictionary<int, string> GetUsers(string fileName, string language)
        {
            var users = new Dictionary<int, string>();
            var parser = new Parser();
            var key = 1;
            foreach (var item in parser.GetUser(language))
            {
                if (!users.ContainsValue(item))
                    users.Add(key++, item);
            }
            Writer.SaveUsers(fileName, users);
            return users;
        }

        private static Dictionary<string, string> GetRepositories(string fileName, Dictionary<int, string> users, string language)
        {
            var repositories = new Dictionary<string, string>();
            var parser = new Parser();
            foreach (var userName in users)
            {
                foreach (var item in parser.GetRepository(userName.Value, language))
                {
                    repositories.Add(item, userName.Value);
                }
            }
            Writer.SaveRepositories(fileName, repositories);
            return repositories;
        }

        private static void DownloadRepositories(string repositoriesFileName, string folder)
        {
            string archiveName;
            foreach (string repository in Reader.ReadRepositoreis(repositoriesFileName))
            {
                archiveName = repository.Replace('/', '-') + ".zip";
                string.Format(folder, archiveName);
                downloads.Add(loader.DownloadZipAsync(repository, folder));
            }
            while (downloads.Count > 0)
            {
                finishedTask = Task.WhenAny(downloads).Result;
                downloads.Remove(finishedTask);
            }
        }
    }
}
