using System.Collections.Generic;

using Buggy.Models.Cars;

namespace Buggy.Data.Seed
{
    internal class SeedModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public float EngineVol { get; set; }

        public int Votes { get; set; }

        public string MakeName { get; set; }

        public IList<SeedComment> Comments { get; set; }

        public Model ToModel()
        {
            return new Model { Id = Id, Name = Name, Image = Image, EngineVol = EngineVol, Votes = Votes };
        }
    }
}
