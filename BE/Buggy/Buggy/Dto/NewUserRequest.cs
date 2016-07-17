using System.ComponentModel.DataAnnotations;

namespace Buggy.Dto
{
    public class NewUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}