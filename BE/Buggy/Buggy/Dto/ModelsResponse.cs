using System.Collections.Generic;

namespace Buggy.Dto
{
    public class ModelsResponse
    {
        public int TotalPages { get; set; }
        public IList<ModelItem> Models { get; set; }
    }
}