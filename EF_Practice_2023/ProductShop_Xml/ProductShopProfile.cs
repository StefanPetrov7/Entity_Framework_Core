using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UsersImportModel, User>();

            this.CreateMap<ProductImportModel, Product>();

            this.CreateMap<CategoryImportModel, Category>();

            this.CreateMap<CategoryProductImportModel, CategoryProduct>();


        }
    }
}
