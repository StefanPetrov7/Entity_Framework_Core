using System.Linq;
using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile     // creating all the mapping profile configurations
    {

        public ProductShopProfile()
        {
            this.CreateMap<UserInputModel, User>();      // UserInputModel are extracted from the json and mapped into  User models to be added into the db.

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategoryInputModel, Category>();

            this.CreateMap<CategoryProductsInputModel, CategoryProduct>();

            this.CreateMap<Product, ExportProductInRangeModel>()
                .ForMember(x => x.Seller, y => y.MapFrom(s => s.Seller.FirstName + " " + s.Seller.LastName));

            this.CreateMap<Category, ExportProductsByCategoryNameModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name))
                .ForMember(x => x.ProductsCount, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Count == 0 ?
                0.ToString("F2") : s.CategoryProducts.Average(x => x.Product.Price).ToString("F2")))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(x => x.Product.Price).ToString("F2")));

        }
    }
}
