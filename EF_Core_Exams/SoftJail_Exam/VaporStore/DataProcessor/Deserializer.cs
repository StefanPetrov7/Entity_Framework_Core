namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        static IMapper mapper;

        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();
            var gamesDto = JsonConvert.DeserializeObject<IEnumerable<GameModel>>(jsonString);

            foreach (var jsonGame in gamesDto)
            {

                if (IsValid(jsonGame) == false || jsonGame.Tags.Count == 0)  // [Min Length (1)] Can beused as attribute for the collection and avoid this if
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var genre = context.Genres.FirstOrDefault(x => x.Name == jsonGame.Genre) ?? new Genre { Name = jsonGame.Genre };
                var developer = context.Developers.FirstOrDefault(x => x.Name == jsonGame.Developer) ?? new Developer { Name = jsonGame.Developer };

                var game = new Game
                {
                    Name = jsonGame.Name,
                    Genre = genre,
                    Developer = developer,
                    Price = jsonGame.Price,
                    ReleaseDate = jsonGame.ReleaseDate.Value,
                };

                foreach (var jsonTag in jsonGame.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == jsonTag) ?? new Tag { Name = jsonTag };
                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                result.AppendLine(string.Format(SuccessfullyImportedGame, jsonGame.Name, jsonGame.Genre, jsonGame.Tags.Count));
                context.Games.Add(game);
                context.SaveChanges();
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            InitializeMapper();
            StringBuilder result = new StringBuilder();

            var usersDto = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonString);
            var filteredUsersDto = new List<UserModel>();

            foreach (var user in usersDto)
            {
                if (IsValid(user) == false || user.Cards.All(x => IsValid(x) == false))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                result.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
                filteredUsersDto.Add(user);
            }

            var users = mapper.Map<IEnumerable<User>>(filteredUsersDto);
            context.Users.AddRange(users);
            context.SaveChanges();
            return result.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder result = new StringBuilder();
            var xmlInput = new XmlSerializer(typeof(PurchaseModel[]), new XmlRootAttribute("Purchases"));
            var purchasesDto = xmlInput.Deserialize(new StringReader(xmlString)) as PurchaseModel[];
            var validated = new List<Purchase>();

            foreach (var purchaseXml in purchasesDto)
            {
                if (IsValid(purchaseXml) == false)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                bool parseDate = DateTime.TryParseExact(purchaseXml.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                if (parseDate == false)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var purchase = new Purchase
                {
                    Date = date,
                    Type = purchaseXml.Type.Value,
                    ProductKey = purchaseXml.ProductKey,
                    Game = context.Games.FirstOrDefault(x => x.Name == purchaseXml.Title),
                    Card = context.Cards.FirstOrDefault(x => x.Number == purchaseXml.Card),
                };

                var userNname = context.Users.Where(x => x.Id == purchase.Card.UserId).Select(x => x.Username).FirstOrDefault();
                result.AppendLine(string.Format(SuccessfullyImportedPurchase, purchaseXml.Title, userNname));
                validated.Add(purchase);
            }

            context.Purchases.AddRange(validated);
            context.SaveChanges();
            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        public static void InitializeMapper()
        {
            var confrig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VaporStoreProfile>();
            });

            mapper = confrig.CreateMapper();
        }
    }
}