using System;
namespace ProductShop.DTO
{
    public class UserInputModel     //A user DTO model which will be templates for the onj to add into the db, created from the json file.
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

    }
}
