
namespace RepositoriesParser
{
    public class Links
    {
        public const string bitbucketLink = "https://bitbucket.org{0}";
        public const string searchLink = "/repo/all?name={0}&language={1}"; // language=html%2Fcss/language=javascript
        public const string userProfileLink = "https://bitbucket.org/{0}";
        public const string downloadZipLink = "https://bitbucket.org/{0}/{1}/get/{2}.zip"; // https://bitbucket.org/{username}/{repository}/get/default.zip default/master
    }
}
