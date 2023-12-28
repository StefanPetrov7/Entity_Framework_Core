namespace VaporStore.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ExportDto;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var query = context.Genres.Where(x => genreNames.Contains(x.Name))
                .Select(x => new GenreModel
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Where(g => g.Purchases.Count > 0).Select(x => new GameExModel
                    {
                        Id = x.Id,
                        Title = x.Name,
                        Developer = x.Developer.Name,
                        Tags = string.Join(", ", x.GameTags.Select(g => g.Tag.Name)),
                        Players = x.Purchases.Count,
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.SelectMany(x => x.Purchases).Count(),
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(query, Formatting.Indented);
            return jsonResult.ToString();
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)  // Dificult One 
        {
            var type = Enum.Parse<PurchaseType>(purchaseType);

            var query = context.Users.ToArray().Where(x => x.Cards.Any(x => x.Purchases.Any(x => x.Type == type)))
                .Select(x => new UserModelXml
                {
                    Username = x.Username,
                    Purchases = x.Cards.SelectMany(p => p.Purchases)
                    .Where(x => x.Type == type)
                    .Select(c => new PurchaseXml
                    {
                        Card = c.Card.Number,
                        Cvc = c.Card.Cvc,
                        Date = c.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameXml
                        {
                            Title = c.Game.Name,
                            Genre = c.Game.Genre.Name,
                            Price = c.Game.Price,
                        }
                    })
                    .OrderBy(x => x.Date)
                    .ToArray(),
                    TotalSpent = x.Cards.Sum(x => x.Purchases.Where(x => x.Type == type).Sum(x => x.Game.Price)),
                })
                .OrderByDescending(x => x.TotalSpent)
                .ThenBy(x => x.Username)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserModelXml[]), new XmlRootAttribute("Users"));
            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");
            var xmlResult = new StringWriter();
            xmlSerializer.Serialize(xmlResult, query, nameSpaces);
            return xmlResult.ToString().TrimEnd();
        }
    }
}