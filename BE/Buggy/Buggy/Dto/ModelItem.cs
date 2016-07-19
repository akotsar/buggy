using System.Collections.Generic;
using System.Linq;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class ModelItem
    {
        private static readonly int MaxComments = 3;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Make { get; set; }
        public int MakeId { get; set; }
        public string MakeImage { get; set; }
        public int Votes { get; set; }
        public int Rank { get; set; }
        public float EngineVol { get; set; }
        public IList<string> Comments { get; set; }
        public int TotalComments { get; set; }

        public ModelItem()
        {
        }

        public ModelItem(Model source)
        {
            Id = source.Id;
            Name = source.Name;
            Image = source.Image;
            Make = source.Make.Name;
            MakeId = source.MakeId;
            MakeImage = source.Make.Image;
            EngineVol = source.EngineVol;
            Votes = source.Votes;
            Comments = source.UserVotes
                .Where(x => !string.IsNullOrEmpty(x.Comment))
                .OrderByDescending(x => x.DateVoted)
                .Select(x => x.Comment).Take(MaxComments).ToList();

            TotalComments = source.UserVotes.Count(x => !string.IsNullOrEmpty(x.Comment));
        }
    }
}