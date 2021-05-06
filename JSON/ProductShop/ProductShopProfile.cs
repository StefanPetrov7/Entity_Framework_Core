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
        }
      
    }
}
