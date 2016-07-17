using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Buggy.Data.Users;

using Newtonsoft.Json;

namespace Buggy.Dto
{
    public class UserProfile
    {
        public UserProfile()
        {
        }

        public UserProfile(User source)
        {
            Username = source.UserName;
            FirstName = source.FirstName;
            LastName = source.LastName;
            Gender = source.Gender;
            Age = source.Age?.ToString();
            Address = source.Address;
            Phone = source.Phone;
            Hobby = source.Hobby;
        }

        public string Username { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Hobby { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentPassword { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NewPassword { get; set; }

        public void UpdateUser(User user)
        {
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Gender = Gender;
            // Intentional bug: will fail if the Age field is not a proper number
            user.Age = Age == null ? (int?)null : int.Parse(Age);
            user.Address = Address;
            user.Phone = Phone;
            user.Hobby = Hobby;
        }
    }
}