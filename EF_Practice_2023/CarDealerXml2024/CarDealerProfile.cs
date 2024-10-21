using AutoMapper;
using CarDealer.DTOs.Export;
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

            this.CreateMap<Supplier, ExportSupplierModelXml>()
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(s => s.Parts.Count()));

            this.CreateMap<ImportPartModelXml, Part>();

            this.CreateMap<ImportCarModelXml, Car>()
                .ForMember(d => d.PartsCars,
                opt => opt.MapFrom(s => s.Parts.Select(p => new PartCar() { PartId = p.Id })));

            this.CreateMap<Car, ExportCarsPartsModelXml>()
                .ForMember(d => d.Parts,
                opt => opt.MapFrom(s => s.PartsCars.Select(p => new ExportPartModelXml()
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price
                })
                .OrderByDescending(x=>x.Name)
                .ToList()));

            this.CreateMap<Car, ExportCarModelXml>();

            this.CreateMap<Car, ExportBmwModelXml>();

            this.CreateMap<ImportCustomerModelXml, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            this.CreateMap<ImportSaleModelXml, Sale>();
        }
    }
}



