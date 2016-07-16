using System.Collections.Generic;

namespace Buggy.Models.Cars
{
    public class Make
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Model> Models { get; set; }
    }
}
