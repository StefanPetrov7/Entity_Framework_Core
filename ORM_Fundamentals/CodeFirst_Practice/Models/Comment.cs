using System;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst_Practice.Models
{
    public class Comment
    {
        public int Id { get; set; } // => PK ID

        public int NewsId { get; set; }  // FK to news one (news) to many (comments)

        public virtual News News { get; set; }  // FK to news => navigational prop.

        [MaxLength(50)]
        public string Author { get; set; }

        public string Content { get; set; }
    }
}
