using System.Linq;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class MakeDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ModelsList Models { get; set; }

        public MakeDetails()
        {
        }

        public MakeDetails(Make source, int? modelsPage, int modelsPageSize, string modelsOrderBy)
        {
            Name = source.Name;
            Description = source.Description;
            Image = source.Image;
            Models = new ModelsList(source.Models.AsQueryable(), modelsPage, modelsPageSize, modelsOrderBy);
        }
    }
}