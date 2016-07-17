using System.Collections.Generic;
using System.Linq;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class ModelDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Make { get; set; }
        public int MakeId { get; set; }
        public string MakeImage { get; set; }
        public int Votes { get; set; }
        public float EngineVol { get; set; }
        public int MaxSpeed { get; set; }
        public IList<Comment> Comments { get; set; }
        public bool CanVote { get; set; }

        public ModelDetails()
        {
        }

        public ModelDetails(Model source, string userId)
        {
            Name = source.Name;
            Description = source.Description;
            Image = source.Image;
            Make = source.Make.Name;
            MakeId = source.MakeId;
            MakeImage = source.Make.Image;
            EngineVol = source.EngineVol;
            MaxSpeed = source.MaxSpeed;
            Votes = source.Votes;
            CanVote = userId != null && source.UserVotes.All(v => v.UserId != userId);
        }
    }
}