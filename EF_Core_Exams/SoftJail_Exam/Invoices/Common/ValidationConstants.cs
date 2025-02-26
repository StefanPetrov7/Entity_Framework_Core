using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
    public static class ValidationConstants
    {
        // Product
        public const int ProductNameMinLength = 9;
        public const int ProductNameMaxLength = 30;
        public const decimal ProductMinPrice = 5.00M;
        public const decimal ProductMaxPrice = 1000.00M;

        // Address
        public const int AddressStreetNameMinLength = 10;
        public const int AddressStreetNameMaxLength = 20;
        public const int AddressCityNameMinLength = 5;
        public const int AddressCityNameMaxLength = 15;
        public const int AddressCountryNameMinLength = 5;
        public const int AddressCountryNameMaxLength = 15;

        // Client
        public const int ClientNameMinLength = 10;
        public const int ClientNameMaxLength = 25;
        public const int ClientVatMinLength = 10;
        public const int ClientVatMaxLength = 15;

        // Invoice
        public const int InvoiceMinNumber = 1_000_000_000;
        public const int InvoiceMaxNumber = 1_500_000_000;




    }
}
