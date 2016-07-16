using System.Collections.Generic;
using System.Linq;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class ModelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Make { get; set; }
        public int MakeId { get; set; }
        public string MakeImage { get; set; }
        public int Votes { get; set; }
        public int Rank { get; set; }
        public IList<string> Comments { get; set; }

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
            Votes = source.Votes;
            Comments = source.Comments.OrderByDescending(x => x.DatePosted).Select(x => x.Comment).Take(3).ToList();
        }
    }
}