using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_CodeFirstDemo.Models
{
    public class Question
    {

        [Key]
        public int Id { get; set; }

        [Required]   // NOT NULL
        [MaxLength(200)]  // NVARCHAR(200)
        public string Content { get; set; }  // Default data NVARCHAR(MAX) ALLOW NULL

        public DateTime CreatedOn { get; set; }  // Default NOT NULL  // DateTime? => will allow null values for the DateTime

        public string Author { get; set; }

        public ICollection<Comment> Comments { get; set; } // This prop is created for easier LINQ querying. 
                                                           // Comments has FK to Question creating one to many relation.
    }                                                      // One question can have many comments.
}                                                          // This is why we are adding Collection of Comment in the Question class so we can .. LINQ.Question.Comment.
                                                           // It's not mandatory but recommended. 
