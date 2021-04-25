using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EF_CodeFirstDemo.Models
{
    [Index(nameof(QuestionId), Name = "IX_QuestionId123")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Content { get; set; }

        public int QuestionId { get; set; } // Navigational prop  (one to many)

        public Question Question { get; set; } // Navigational prop  // => This can be used for auto Join's and LINQ queries

    }
}
