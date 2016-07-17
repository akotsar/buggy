using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Buggy.Data.Users;

using Newtonsoft.Json;

namespace Buggy.Dto
{
    public class UserProfile
    {
        public string Username { get; set; }

        [DisplayName("First Name")]
        [Required]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Gender { get; set; }

        [MaxLength(250)]
        public string Age { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(250)]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string Hobby { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentPassword { get; set; }

        [MaxLength(50)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NewPassword { get; set; }

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