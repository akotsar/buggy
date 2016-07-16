using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class ModelItem
    {
        public ModelItem()
        {
        }

        public ModelItem(Model source)
        {
            Id = source.Id;
            Name = source.Name;
            Make = source.Make.Name;
            MakeId = source.MakeId;
            Votes = source.Votes;
            Comments = source.Comments.OrderByDescending(x => x.DatePosted).Select(x => x.Comment).Take(3).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public int MakeId { get; set; }
        public int Votes { get; set; }
        public int Rank { get; set; }
        public IList<string> Comments { get; set; }
    }
}