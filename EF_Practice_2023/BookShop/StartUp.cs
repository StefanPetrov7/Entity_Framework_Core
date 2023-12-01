namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            // var input = Console.ReadLine();
            // string result = GetMostRecentBooks(db);
            int result = RemoveBooks(db);

            Console.WriteLine(result);

        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(booksToRemove);
            context.SaveChanges();
            return booksToRemove.Count; 
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksToUpdate = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            booksToUpdate.Select(x => x.Price += 5).ToList();

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            //var query = context.Categories    // => working solution as well
            //    .Select(x => new
            //    {
            //        CatName = x.Name,
            //        ListOfBooks = x.CategoryBooks.Select(x => x.Book)
            //    }).ToList();


            var query = context.Categories
                .Select(x => new
                {
                    CatName = x.Name,
                    ListOfBooks = x.CategoryBooks.Select(x => new
                    {
                        BookName = x.Book.Title,
                        Date = x.Book.ReleaseDate,
                    })
                    .OrderByDescending(x => x.Date)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(x => x.CatName)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var cat in query.OrderBy(x => x.CatName))
            {
                result.AppendLine($"--{cat.CatName}");

                foreach (var book in cat.ListOfBooks)
                {
                    result.AppendLine($"{book.BookName} ({book.Date.Value.Year})");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var query = context.Categories
                .Select(x => new
                {
                    Category = x.Name,
                    Sales = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies),
                })
                .OrderByDescending(x => x.Sales)
                .ThenBy(x => x.Category)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var cat in query)
            {
                result.AppendLine($"{cat.Category} ${cat.Sales:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var query = context.Authors
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    BooksCount = x.Books.Sum(x => x.Copies),
                })
                .OrderByDescending(x => x.BooksCount)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var author in query)
            {
                result.AppendLine($"{author.FullName} - {author.BooksCount}");
            }

            return result.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int count = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList()
                .Count();

            return count;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var query = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    BookName = x.Title,
                    AuthorName = x.Author.FirstName + " " + x.Author.LastName,
                    id = x.BookId
                })
                .OrderBy(x => x.id)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var books in query)
            {
                result.AppendLine($"{books.BookName} ({books.AuthorName})");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var query = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var titles in query)
            {
                result.AppendLine($"{titles}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var query = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                })
                .OrderBy(x => x.FullName)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var authors in query)
            {
                result.AppendLine($"{authors.FullName}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var query = context.Books
                .Where(x => x.ReleaseDate < parsedDate)
                .Select(x => new
                {
                    Name = x.Title,
                    Date = x.ReleaseDate,
                    Edition = x.EditionType,
                    BookPrice = x.Price,
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine($"{book.Name} - {book.Edition} - ${book.BookPrice:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ").ToArray();

            var query = context.Categories
                .Where(x => categories.Contains(x.Name.ToLower()))
                .Select(x => new
                {
                    Books = x.CategoryBooks.Select(x => x.Book.Title)
                }).ToList();

            List<string> titles = new List<string>();

            foreach (var cats in query)
            {
                titles.AddRange(cats.Books);
            }

            StringBuilder result = new StringBuilder();

            foreach (var title in titles.OrderBy(x => x))
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var query = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .Select(x => new
                {
                    Id = x.BookId,
                    Name = x.Title,
                })
                .OrderBy(x => x.Id)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine($"{book.Name}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var query = context.Books
                    .Where(x => x.Price > 40)
                    .Select(x => new
                    {
                        BookPrice = x.Price,
                        Name = x.Title,
                    })
                    .OrderByDescending(x => x.BookPrice)
                    .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine($"{book.Name} - ${book.BookPrice:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var query = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .Select(x => new
                {
                    Id = x.BookId,
                    Name = x.Title,
                })
                .OrderBy(x => x.Id)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine(book.Name);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string input)
        {
            var ageLevel = Enum.Parse<AgeRestriction>(input, true);

            var query = context.Books
                .Where(x => x.AgeRestriction == ageLevel)
                .Select(x => x.Title)
                .OrderBy(x => x);

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine(book.ToString());
            }

            return result.ToString().TrimEnd();
        }
    }
}


