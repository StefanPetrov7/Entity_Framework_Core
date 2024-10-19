using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // <src, dest>

            this.CreateMap<ImportSupplierModelXml, Supplier>();

            this.CreateMap<ImportPartModelXml, Part>();

            this.CreateMap<ImportCarModelXml, Car>()
                .ForMember(d => d.PartsCars,
                opt => opt.MapFrom(s => s.Parts.Select(p => new PartCar() { PartId = p.Id })));

            this.CreateMap<ImportCustomerModelXml, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            this.CreateMap<ImportSaleModelXml, Sale>();
        }
    }
}
