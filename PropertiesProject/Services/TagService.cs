using Data;
using Models;
using Services.Contracts;
using Services.MapperProfiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    ///// Mapper in inherited from the abstract class MapperInjection giving the mapper functionalities to the Services.
    /// </summary>
    public class TagService : MapperInjection, ITagService
    {
        private readonly AppDBContext _dbContext;
        private readonly IPropertyService _propService;
        public TagService(AppDBContext db, IPropertyService propService)
        {
            this._dbContext = db;
            this._propService = propService;
        }

        public void AddTag(string name, int? importance = null)
        {
            var tag = _dbContext.Tags.FirstOrDefault(x => x.Name == name);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = name,
                    Importance = importance ?? 0,
                };
            }

            this._dbContext.Tags.Add(tag);
            this._dbContext.SaveChanges();
        }

        public void BulkTagProperties()
        {
            var properties = _dbContext.Properties.ToList();
            var tags = _dbContext.Tags.ToList();

            foreach (var prop in properties)
            {
                var distAvgPrice = this._propService.AveragePricePerM2(prop.DistrictId);

                if (prop.Price >= distAvgPrice)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 1));
                }
                else if (prop.Price < distAvgPrice)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 2));
                }

                if (prop.Year.HasValue && prop.Year >= 2000)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 6));
                }
                else if (prop.Year.HasValue && prop.Year < 2000)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 5));
                }

                var avgPropSize = this._propService.AverageSize(prop.DistrictId);

                if (prop.Size >= avgPropSize)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 4));
                }
                else if (prop.Size < avgPropSize)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 3));
                }

                if (prop.Floor.HasValue && prop.Floor == 1)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 7));
                }
                else if (prop.Floor.HasValue && prop.TotalFloors.HasValue && prop.Floor == prop.TotalFloors)
                {
                    prop.Tags.Add(_dbContext.Tags.FirstOrDefault(x => x.Id == 8));
                }
            }

            _dbContext.SaveChanges();

        }
    }
}
