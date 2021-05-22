using System;
namespace ProductShop.DTO
{
    public class ProductInputModel   // Product DTO for importing the obj from the json file and then mapping it into Products obj's before adding it into the db.
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

    }
}
