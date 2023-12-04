using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            this.CreateMap<UserImputModel, User>();

            this.CreateMap<ProductImputModel, Product>();

            this.CreateMap<CategoryImportModel, Category>();

            this.CreateMap<CategoryProductsImportModel, CategoryProduct>();

        }
    }
}
