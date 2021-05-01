namespace BookShop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));

            //Console.WriteLine(GetGoldenBooks(db));

            //Console.WriteLine(GetBooksByPrice(db));

            //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));

            // Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));

            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));

            //Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));

            //Console.WriteLine(GetBookTitlesContaining(db, "sK"));

            //Console.WriteLine(GetBooksByAuthor(db, "R"));

            //Console.WriteLine(CountBooks(db,12));

            //Console.WriteLine(CountCopiesByAuthor(db));

            //Console.WriteLine(GetTotalProfitByCategory(db));




        }

        //public static string GetTotalProfitByCategory(BookShopContext context)
        //{
        //    var profitaility = context.Books
        //        .GroupBy(x => x.BookCategories
        //        .Select(x => x.Category.Name)
        //        .Select(x=>new { x. }))
            

      

        //    return string.Join(Environment.NewLine, profitaility.Select(x => $"{x.Category} ${x.Profit}"));

        //}

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BookCopies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.BookCopies)
                .ToArray();

            return string.Join(Environment.NewLine, authors.Select(x => $"{x.FirstName} {x.LastName} - {x.BookCopies}"));

        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToArray();

            return books.Count();

        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            input = input.ToLower();

            var authors = context.Authors
                .Where(x => x.LastName.ToLower().StartsWith(input))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    Books = x.Books
                    .OrderBy(x => x.BookId)
                    .Select(x => new { x.Title })
                })
                .ToArray(); ;

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                foreach (var book in author.Books)
                {
                    sb.AppendLine($"{book.Title} ({author.FirstName} {author.LastName})");
                }
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();

            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input))
                .Select(x => new { x.Title })
                .OrderBy(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(x => x.Title));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new { FullName = x.FirstName + " " + x.LastName })
                .OrderBy(x => x.FullName)
                .ToArray();

            return string.Join(Environment.NewLine, authors.Select(x => x.FullName));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime release = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < release)
                  .OrderByDescending(x => x.ReleaseDate.Value)
                    .Select(x => new { x.Title, x.EditionType, x.Price })
                      .ToArray();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}"));

        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Include(x => x.BookCategories)
                .ThenInclude(x => x.Category)
                .ToArray()
                .Where(x => x.BookCategories
                .Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, books);

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var parsed = Enum.Parse<AgeRestriction>(command, true);

            var bookTitles = context.Books
                .Where(x => x.AgeRestriction == parsed)
                .Select(x => new { x.Title })
                .ToList()
                .OrderBy(x => x.Title);

            StringBuilder sb = new StringBuilder();

            foreach (var book in bookTitles)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => new { x.Title })
                .ToArray();

            string result = string.Join(Environment.NewLine, goldenBooks.Select(x => x.Title));
            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new { x.Title, x.Price })
                .ToArray()
                .OrderByDescending(x => x.Price);

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => new { x.Title })
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(x => x.Title));
        }
    }
}
