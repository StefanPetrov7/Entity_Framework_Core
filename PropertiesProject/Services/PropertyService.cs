using AutoMapper.QueryableExtensions;
using Data;
using Models;
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

    public class PropertyService : MapperInjection, IPropertyService
    {
        private readonly AppDBContext _dbContext;

        public PropertyService(AppDBContext db)
        {
            this._dbContext = db;
        }

        public void AddProperty(string district, int floor, int totalFloor, int size, int yardSize, int year, string propertyType, string buildingType, int price)
        {
            var property = new Property
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor >= 255 ? null : (byte)floor,
                TotalFloors = totalFloor <= 0 || totalFloor >= 255 ? null : (byte)totalFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year,
            };

            var dbDistrict = this._dbContext.Districts.FirstOrDefault(x => x.Name == district);

            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }

            property.District = dbDistrict;

            var dbPropertyType = this._dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);

            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }

            property.PropertyType = dbPropertyType;

            var dbBuildingType = this._dbContext.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);

            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }

            property.BuildingType = dbBuildingType;

            this._dbContext.Add(property);
            this._dbContext.SaveChanges();
        }

        public decimal AveragePricePerM2()
            => this._dbContext.Properties.Where(x => x.Price.HasValue).Average(x => x.Price / (decimal)x.Size) ?? 0;


        public decimal AveragePricePerM2(int? districtId)   // Method overload
            => this._dbContext.Properties.Where(x => x.Price.HasValue && x.DistrictId == districtId).Average(x => x.Price / (decimal)x.Size) ?? 0;


        public double AverageSize(int? districtId)
            => this._dbContext.Properties.Where(x => x.DistrictId == districtId).Average(x => (double)x.Size);

        public IEnumerable<PropertyFullDataModel> GetPropertyFullDataModels(int count)
        {

             // TODO to have the year passed from outside

            var selectedProperties = _dbContext.Properties
                .Where(x => x.Floor.HasValue && x.Floor > 1 && x.Floor.HasValue && x.Floor < 8 && x.Year.HasValue && x.Year < 2020)
                .ProjectTo<PropertyFullDataModel>(this.Mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Price)
                .ThenByDescending(x => x.Size)
                .ThenByDescending(x => x.Year)
                .Take(count)
                .ToList();

            return selectedProperties;
        }


        public IEnumerable<PropertyInfoModel> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties = this._dbContext.Properties.Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
                .Select(x => new PropertyInfoModel
                {
                    DistrictName = x.District.Name,
                    Size = x.Size,
                    Price = x.Price ?? 0,    // => If the price is null then => 0
                    PropertyType = x.PropertyType.Name,
                    BuildingType = x.BuildingType.Name
                }).ToList();

            /// Using Mapper 

            //var mappedProperties = this._dbContext.Properties.Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
            //    .ProjectTo<PropertyInfoModel>(this.Mapper.ConfigurationProvider)
            //    .ToList();

            return properties;
        }
    }
}
