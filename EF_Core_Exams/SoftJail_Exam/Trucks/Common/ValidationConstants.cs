using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Common
{
    public  static class ValidationConstants
    {
        // Truck model
        public const int TruckRegistrationNumberLenght = 8;
        public const int TruckVinNumberNumberLenght = 17;
        public const string TruckRegistrationNumberRegEx = @"[A-Z]{2}\d{4}[A-Z]{2}$";

        // Client model
        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        public const int ClientNationalityMaxLength = 40;

        // Despatcher model
        public const int DespathcerNameMinLenght = 2;
        public const int DespathcerNameMaxLenght = 40;





    }
}
