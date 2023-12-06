using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SuplierImportModel, Supplier>();

            this.CreateMap<PartsImportModel, Part>();

            this.CreateMap<CustomersModelsImport, Customer>();

            this.CreateMap<SalesImportModel, Sale>();
        }
    }
}
