using Newtonsoft.Json;

namespace Buggy.Dto
{
    public class CurrentUserInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsAdmin { get; set; }
    }
}