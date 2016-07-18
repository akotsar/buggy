using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using Buggy.Models.Cars;

namespace Buggy.Data.Seed
{
    [NotMapped]
    internal class SeedModel : Model
    {
        public string MakeName { get; set; }

        public IList<SeedComment> Comments { get; set; }

        public Model ToModel()
        {
            var type = typeof(Model);
            var model = new Model();
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    prop.SetValue(model, prop.GetValue(this));
                }
            }

            return model;
        }
    }
}