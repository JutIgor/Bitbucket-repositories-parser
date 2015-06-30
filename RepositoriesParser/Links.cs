namespace RepositoriesParser
{
    public class Links
    {
        public const string bitbucketLink = "https://bitbucket.org{0}";
        public const string searchLink = "/repo/all?name={0}&language={1}"; // language=html%2Fcss/language=javascript
        public const string userProfileLink = "/{0}/profile/repositories?language={1}"; // {0} - username; {1} - language
    }
}
