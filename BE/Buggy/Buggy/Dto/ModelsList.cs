using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Buggy.Models.Cars;

namespace Buggy.Dto
{
    public class ModelsList
    {
        public int TotalPages { get; set; }
        public IList<ModelItem> Models { get; set; }

        public ModelsList()
        {
        }

        public ModelsList(IQueryable<Model> models, int? page, int pageSize, string orderBy)
        {
            var filtered = models.OrderByDescending(x => x.Votes).ToList();

            var query = filtered
                .Select((x, i) => new { Model = x, Rank = i + 1 })
                .ToList()
                .AsQueryable();

            switch (orderBy?.ToLowerInvariant())
            {
                case "make":
                    query = query.OrderBy(x => x.Model.Make.Name);
                    break;

                case "name":
                    query = query.OrderBy(x => x.Model.Name);
                    break;

                case "votes":
                    query = query.OrderByDescending(x => x.Model.Votes);
                    break;

                case "rank":
                    query = query.OrderBy(x => x.Rank.ToString(CultureInfo.InvariantCulture));
                    break;

                case "engine":
                    query = query.OrderBy(x => x.Model.EngineVol);
                    break;

                case "random":
                    var rnd = new Random();
                    query = query
                        .Select(x => new { x, rnd = rnd.Next() })
                        .OrderBy(x => x.rnd)
                        .Select(x => x.x);
                    break;

                default:
                    query = query.OrderByDescending(x => x.Model.Votes);
                    break;
            }

            int skip = (page.GetValueOrDefault(1) - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            TotalPages = (int)Math.Ceiling((float)filtered.Count / pageSize);
            Models = query.Select(m => new ModelItem(m.Model) { Rank = m.Rank }).ToList();
        }
    }
}