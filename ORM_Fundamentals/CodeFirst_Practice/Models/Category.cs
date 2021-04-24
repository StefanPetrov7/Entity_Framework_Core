using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst_Practice.Models
{
    public class Category
    {
        public Category()   // A Set of the news related to the categories. One category can have many news. 
        {
            this.News = new HashSet<News>();
        }

        public int Id { get; set; }  // PK ID

        [MaxLength(100)]
        public string Title { get; set; }

        public virtual ICollection<News> News { get; set; } // If the news has FK which references Category. Category needs to have a collection of news.

    }
}
