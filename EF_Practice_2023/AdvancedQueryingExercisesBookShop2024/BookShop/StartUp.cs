namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);

            // Problem 2
            //string resultTaskTwo = GetBooksByAgeRestriction(db, Console.ReadLine());
            //Console.WriteLine(resultTaskTwo);

            // Problem 6
            //string resultSix = GetBooksByCategory(db, Console.ReadLine());
            //Console.WriteLine(resultSix);

            // Problem 8
            //string resultEight = GetAuthorNamesEndingIn(db, Console.ReadLine());
            //Console.WriteLine(resultEight);

            // Problem 12
            //string resultTwelve = CountCopiesByAuthor(db);
            //Console.WriteLine(resultTwelve);

            // Problem 13
            //string resultThirteen = GetTotalProfitByCategory(db);
            //Console.WriteLine(resultThirteen);

            // Problem 14
            //string fourTeen = GetMostRecentBooks(db);
            //Console.WriteLine(fourTeen);

            // Problem 15
            //IncreasePrices(db);


            // Problem 16
            int deletedCount = RemoveBooks(db);
            Console.WriteLine(deletedCount);
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var idToDelete = context.Books.Where(x => x.Copies < 4200)
                .Select(x => x.BookId)
                .ToArray();

            var booksToDelete = context.Books.Where(x => x.Copies < 4200).ToArray();
            var mappingsToDelete = context.BooksCategories.Where(x => idToDelete.Contains(x.BookId)).ToArray();

            context.BooksCategories.RemoveRange(mappingsToDelete);
            context.Books.RemoveRange(booksToDelete);
            context.SaveChanges();
            return booksToDelete.Count();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksToUpdate = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var book in booksToUpdate)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var query = context.Categories.Select(x => new
            {
                x.Name,
                MostRecentBooks = x.CategoryBooks.Select(b => new
                {
                    b.Book.Title,
                    b.Book.ReleaseDate,
                })
                .OrderByDescending(x => x.ReleaseDate)
                .Take(3)
                .ToArray()
            })
                .OrderBy(x => x.Name)
                .ToArray();

            StringBuilder result = new StringBuilder();

            foreach (var category in query)
            {
                result.AppendLine($"--{category.Name}");

                foreach (var book in category.MostRecentBooks)
                {
                    result.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var query = context.Authors.Select(x => new
            {
                FullName = x.FirstName + " " + x.LastName,
                BookCopies = x.Books.Sum(x => x.Copies)
            })
            .OrderByDescending(x => x.BookCopies)
            .ToArray();

            StringBuilder result = new StringBuilder();

            foreach (var author in query)
            {
                result.AppendLine($"{author.FullName} - {author.BookCopies}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            // Note that EF core can give errors with using some string methods like string.Concat() etc... but not limited too!
            string endLetters = input.ToLower();

            var query = context.Authors
                .Where(x => x.FirstName.EndsWith(endLetters))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                })
                .OrderBy(x => x.FullName).ToList();

            StringBuilder result = new StringBuilder();

            foreach (var author in query)
            {
                result.AppendLine(author.FullName);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var query = context.Books.Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(b => new
                {
                    b.Title

                })
                .OrderBy(x => x.Title).
                ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in query)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string stringInput)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(stringInput, true);

            var query = context.Books.Where(x => x.AgeRestriction == ageRestriction)
                .Select(b => new
                {
                    b.Title,
                })
                .OrderBy(x => x.Title)
                .ToList();

            StringBuilder result = new StringBuilder();


            foreach (var book in query)
            {
                result.AppendLine(book.Title);
            }

            return result.ToString().TrimEnd();
        }
    }
}


