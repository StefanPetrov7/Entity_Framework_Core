using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_CodeFirst.Models
{
    public class News
    {
        public News()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }   // PK Id column

        [MaxLength(300)]
        public string Title { get; set; } // column Title

        public string Content { get; set; } // Column Content

        public string CategoryId { get; set; } // FK_To Category

        public virtual  Category Category { get; set; } // Relation to Category 

        public virtual ICollection<Comment>  Comments { get; set; } // Additional link to Comment for easier navigation



    }
}
