using AutoMapper.QueryableExtensions;
using Data;
using Services.Contracts;
using Services.MapperProfiler;
using Services.Models;
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

    public class DistrictService : MapperInjection, IDistrictService
    {
        private readonly AppDBContext _dbContext;

        public DistrictService(AppDBContext db)
        {
            this._dbContext = db;
        }

        public IEnumerable<DistrictInfoModel> GetMostExpesiveDistricts(int count)
        {
            var districts = _dbContext.Districts
                .Select(x => new DistrictInfoModel
                {
                    Name = x.Name,
                    AvgPricePerM2 = x.Properties.Where(x => x.Price.HasValue).Average(x => x.Price / (decimal)x.Size) ?? 0,
                    PropertiesCount = x.Properties.Count,
                })
                .OrderByDescending(x => x.AvgPricePerM2)
                .Take(count)
                .ToList();

            // Using mapper 

            //var mappedDistricts = _dbContext.Districts
            //    .ProjectTo<DistrictInfoModel>(this.Mapper.ConfigurationProvider)
            //    .OrderByDescending(x => x.AvgPricePerM2)
            //    .Take(count)
            //    .ToList();

            return districts;
        }
    }
}
