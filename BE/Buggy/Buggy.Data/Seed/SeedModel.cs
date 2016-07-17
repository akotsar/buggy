﻿using System.Collections.Generic;

using Buggy.Models.Cars;

namespace Buggy.Data.Seed
{
    internal class SeedModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public float EngineVol { get; set; }

        public int MaxSpeed { get; set; }

        public int Votes { get; set; }

        public string MakeName { get; set; }

        public IList<SeedComment> Comments { get; set; }

        public Model ToModel()
        {
            return new Model
                   {
                       Id = Id,
                       Name = Name,
                       Description = Description,
                       Image = Image,
                       EngineVol = EngineVol,
                       MaxSpeed = MaxSpeed,
                       Votes = Votes
                   };
        }
    }
}
