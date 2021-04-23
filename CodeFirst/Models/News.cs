using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst_Practice.Models
{
    public class News
    {

        public News()  // => A Set of comments, one news can have many comments
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }  

        public string Content { get; set; }

        public int CategoryId { get; set; }  // FK ID 

        public virtual Category Category { get; set; }  // FK to Category => one (categories) to many (news) => navigational prop.

        public virtual ICollection<Comment> Comments { get; set; } // Comments has FK references news and news need to have collection of comments. 

    }
}
