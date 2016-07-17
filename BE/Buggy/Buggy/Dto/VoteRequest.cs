using System.ComponentModel.DataAnnotations;

namespace Buggy.Dto
{
    public class VoteRequest
    {
        [MaxLength(500)]
        public string Comment { get; set; }
    }
}