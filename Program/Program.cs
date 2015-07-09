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

            var newDirectoryPath = string.Format(Paths.HtmlCssFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);
            newDirectoryPath = string.Format(Paths.JavaScriptFolder, string.Empty);
            Directory.CreateDirectory(newDirectoryPath);

            try
            {
                //var HtmlCssusers = GetUsers(Paths.HtmlCssUsersFile, Language.HtmlCss);
                //var JavaScriptUsers = GetUsers(Paths.JavaScriptUsersFile, Language.JavaScript);
                //var HtmlCssRepositories = GetRepositories(Paths.HtmlCssRepositoriesFile,Paths.HtmlCssUsersFile, Language.HtmlCss);
                //var JavaScriptRepositories = GetRepositories(Paths.JavaScriptRepositoriesFile, Paths.JavaScriptUsersFile, Language.JavaScript);

                DownloadRepositories(Paths.HtmlCssRepositoriesFile, Paths.HtmlCssFolder);
                DownloadRepositories(Paths.JavaScriptRepositoriesFile, Paths.JavaScriptFolder);

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

        private static List<Tuple<string, string>> GetRepositories(string fileName, string usersFileName, string language)
        {
            //var repositories = new Dictionary<string, string>(); // TODO: replace Dictionary because in generalB can exist repositories with same name
            var repositories = new List<Tuple<string, string>>();
            var parser = new Parser();
            foreach (var userName in Reader.ReadFile(usersFileName))
            {
                foreach (var item in parser.GetRepository(userName, language))
                {
                    repositories.Add(new Tuple<string, string>(userName, item));
                    //repositories.Add(item, userName.Value);
                }
            }
            Writer.SaveRepositories(fileName, repositories);
            return repositories;
        }

        private static void DownloadRepositories(string repositoriesFileName, string folder)
        {
            string archiveName;
            string fullPath;
            var i = 0;
            foreach (string repository in Reader.ReadFile(repositoriesFileName))
            {
                if (i++ == 2) break;
                archiveName = repository.Replace('/', '-') + ".zip";
                fullPath = string.Format(folder, archiveName);
                //downloads.Add(loader.DownloadZipAsync(repository, fullPath));
            }
            while (downloads.Count > 0)
            {
                finishedTask = Task.WhenAny(downloads).Result;
                downloads.Remove(finishedTask);
            }
        }
    }
}
