using AutoMapper;
using Models;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.MapperProfiler
{
    public class AppProfiler : Profile
    {
        public AppProfiler()
        {
            this.CreateMap<Property, PropertyInfoModel>()
                .ForMember(x => x.BuildingType, y => y.MapFrom(c => c.BuildingType.Name));

            this.CreateMap<District, DistrictInfoModel>()
                .ForMember(x => x.AvgPricePerM2, y => y
                .MapFrom(c => c.Properties.Where(p => p.Price.HasValue).Average(p => p.Price / (decimal)p.Size) ?? 0));

            this.CreateMap<Property, PropertyFullDataModel>()
                .ForMember(x => x.BuildingType, y => y.MapFrom(c => c.BuildingType.Name));

            this.CreateMap<Tag, TagInfoModel>();

        }
    }
}
