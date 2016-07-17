using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Buggy.Models.Cars;

namespace Buggy.Models.Votes
{
    public class UserVote
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Model")]
        public int ModelId { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        public DateTime DateVoted { get; set; }

        public virtual Model Model { get; set; }
    }
}
