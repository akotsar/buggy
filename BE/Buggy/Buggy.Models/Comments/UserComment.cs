using System;
using System.ComponentModel.DataAnnotations.Schema;

using Buggy.Models.Cars;

namespace Buggy.Models.Comments
{
    public class UserComment
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Model")]
        public int ModelId { get; set; }

        public string Comment { get; set; }

        public DateTime DatePosted { get; set; }

        public virtual Model Model { get; set; }
    }
}
