using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SuppliersImportModel, Supplier>();

            this.CreateMap<PartsImportModel, Part>();

            this.CreateMap<CustomersImportModel, Customer>();

            this.CreateMap<SaleImportModel, Sale>();


        }
    }
}
