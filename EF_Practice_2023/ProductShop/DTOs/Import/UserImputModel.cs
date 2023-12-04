using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class UserImputModel
    {

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Age { get; set; }

    }
}
