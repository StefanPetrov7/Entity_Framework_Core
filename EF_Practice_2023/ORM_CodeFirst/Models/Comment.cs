using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_CodeFirst.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int NewsId { get; set; }

        public virtual  News News { get; set; }

        public string Author { get; set; }

        [MaxLength(50)]
        public string Content { get; set; }
    }
}
