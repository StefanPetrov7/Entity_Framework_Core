using System;
using System.Collections.Generic;
using CodeFirst_Practice.Models;

namespace CodeFirst_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDBContext();
            db.Database.EnsureCreated();

            // CRUD Example => With one insert / one category, one news and two comments. 

            db.Categories.Add(new Category
            {
                Title = "Sport",
                News = new List<News>
                {
                    new News
                    {
                        Title = "CSKA won the game with Levski",
                        Content = "Score was 3 : 0",
                        Comments = new List<Comment>
                        {
                            new Comment {Author = "Kambata", Content = "The game was amazing, the players made the ball in a square shape." },
                            new Comment {Author = "BiBin", Content = "Mentioined that he has watched this game already." }
                        }

                    }
                }

            });
            db.SaveChanges();
        }
    }
}
