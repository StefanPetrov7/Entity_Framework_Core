using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile   // Mapping DTO class
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSuppliersModel, Supplier>();

            this.CreateMap<ImportPartModel, Part>();

            this.CreateMap<ImportCarModel, Car>();

            this.CreateMap<ImportCustomersModel, Customer>();

            this.CreateMap<ImportSalesModel, Sale>();

            this.CreateMap<Customer, ExportCustomersModel>();
        }
    }
}
