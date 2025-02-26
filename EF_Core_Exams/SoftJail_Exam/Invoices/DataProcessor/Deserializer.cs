namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder outputMessage = new StringBuilder();

            // => Working without XmlHelper class
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportClientModelXml[]), new XmlRootAttribute("Clients"));
            //var clientsDtos = xmlSerializer.Deserialize(new StringReader(xmlString)) as ImportClientModelXml[];

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Clients";
            ImportClientModelXml[] clientsDtos = xmlHelper.Deserialize<ImportClientModelXml[]>(xmlString, xmlRoot);

            ICollection<Client> validClients = new HashSet<Client>();

            foreach (var clientDto in clientsDtos)
            {
                if (IsValid(clientDto) == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                var client = new Client() 
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                };

                foreach (var adressDto in clientDto.Addresses)
                {

                    if (IsValid(adressDto) == false)
                    {
                        outputMessage.AppendLine(ErrorMessage);
                        continue;
                    }

                    var address = new Address()
                    {
                        StreetName = adressDto.StreetName,
                        StreetNumber = adressDto.StreetNumber,
                        PostCode = adressDto.PostCode,
                        City = adressDto.City,
                        Country = adressDto.Country,
                    };

                    client.Addresses.Add(address);

                }

                outputMessage.AppendLine(String.Format(SuccessfullyImportedClients, client.Name));
                validClients.Add(client);
            }

            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return outputMessage.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder outputMessage = new StringBuilder();

            var clientIds = context.Clients.Select(x => x.Id).ToList();
            var invoicesDto = JsonConvert.DeserializeObject<IEnumerable<ImportInvoiceModelJson>>(jsonString);
            var validInvoices = new List<Invoice>();

            foreach (var invoiceDto in invoicesDto)
            {
                if (IsValid(invoiceDto) == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                var issueDate = DateTime.ParseExact(invoiceDto.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                var dueDate = DateTime.ParseExact(invoiceDto.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                if (dueDate < issueDate)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                bool isParsed = Enum.IsDefined(typeof(CurrencyType), invoiceDto.CurrencyType);

                if (isParsed == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.Amount <= 0)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientIds.Any(x => x == invoiceDto.ClientId) == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                var invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };

                validInvoices.Add(invoice);
                outputMessage.AppendLine(String.Format(SuccessfullyImportedInvoices, invoice.Number));

            }

            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();
            return outputMessage.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder outputMessage = new StringBuilder();

            var productsDto = JsonConvert.DeserializeObject<IEnumerable<ImportProductModelJson>>(jsonString);
            var validProducts = new List<Product>();
            var clientIds = context.Clients.Select(x => x.Id).ToList();

            foreach (var productDto in productsDto)
            {
                if (IsValid(productDto) == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                bool categoryTypeIsParsed = Enum.IsDefined(typeof(CategoryType), productDto.CategoryType);

                if (categoryTypeIsParsed == false)
                {
                    outputMessage.AppendLine(ErrorMessage);
                    continue;
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType,
                };

                var uniqueIds = productDto.Clients.Distinct().ToList();

                foreach (var id in uniqueIds)
                {
                    if (clientIds.Contains(id) == false)
                    {
                        outputMessage.AppendLine(ErrorMessage);
                        continue;
                    }

                    var productClient = new ProductClient
                    {
                        Product = product,
                        ClientId = id,
                    };

                    product.ProductsClients.Add(productClient);
                }

                outputMessage.AppendLine(String.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
                validProducts.Add(product);

            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return outputMessage.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
