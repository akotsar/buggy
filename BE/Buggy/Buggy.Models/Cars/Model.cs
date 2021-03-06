﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using Buggy.Models.Votes;

namespace Buggy.Models.Cars
{
    public class Model
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [ForeignKey("Make")]
        public int MakeId { get; set; }

        public float EngineVol { get; set; }

        public int MaxSpeed { get; set; }

        [DefaultValue(0)]
        public int Votes { get; set; }

        public virtual Make Make { get; set; }

        public virtual ICollection<UserVote> UserVotes { get; set; }
    }
}
