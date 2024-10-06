using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models 
{
    public class Country
    {
        public Country()
        {
            this.Towns = new HashSet<Town>();
        }

        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(ValidationsCostants.CountryNameMaxLength)]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Town.Country))]
        public virtual ICollection<Town> Towns { get; set; }
    }
}





