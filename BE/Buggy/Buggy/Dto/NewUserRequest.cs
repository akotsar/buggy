using System.ComponentModel.DataAnnotations;

namespace Buggy.Dto
{
    public class NewUserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}