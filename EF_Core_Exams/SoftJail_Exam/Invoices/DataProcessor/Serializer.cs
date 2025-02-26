namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var query = context.Clients.Where(x => x.Invoices.Any(x => x.IssueDate > date))
                .Select(x => new ExportClientModelXml
                {
                    InvoicesCount = x.Invoices.Count,
                    ClientName = x.Name,
                    VatNumber = x.NumberVat,
                    Invoices = x.Invoices.Select(i => new ExportInvoiceModelXml
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount.ToString("0.##"),
                        ForSortingIssueDate = i.IssueDate,
                        ForSortingDueDate = i.DueDate,
                        DueDate = i.DueDate.ToString("MM/dd/yyyy"),
                        Currency = i.CurrencyType.ToString()
                    })
                    .OrderBy(x => x.ForSortingIssueDate)
                    .ThenByDescending(x => x.ForSortingDueDate)
                    .ToArray()
                })
                .OrderByDescending(x => x.InvoicesCount)
                .ThenBy(x => x.ClientName)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportClientModelXml[]), new XmlRootAttribute("Clients"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            var xmlResult = new StringWriter();
            xmlSerializer.Serialize(xmlResult, query, namespaces);
            return xmlResult.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var query = context.Products
                .Where(x => x.ProductsClients.Any(x => x.Client.Name.Length >= nameLength) && x.ProductsClients.Count > 0)
                .Select(x => new
                {
                    x.Name,
                    Price = Decimal.Parse(x.Price.ToString("0.##")),
                    Category = x.CategoryType.ToString(),
                    Clients = x.ProductsClients.Where(x => x.Client.Name.Length >= nameLength)
                    .Select(c => new
                    {
                        c.Client.Name,
                        c.Client.NumberVat
                    })
                    .OrderBy(x => x.Name)
                    .ToArray()

                })
                .OrderByDescending(x => x.Clients.Length)
                .ThenBy(x => x.Name)
                .Take(5)
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(query, Formatting.Indented);
            return jsonResult.ToString().TrimEnd();
        }
    }
}
