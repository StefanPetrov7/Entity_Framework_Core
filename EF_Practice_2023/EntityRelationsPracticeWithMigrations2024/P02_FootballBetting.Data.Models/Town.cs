using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models 
{
    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(ValidationsCostants.TownNameMaxLength)]
        public string Name { get; set; } = null!;


        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;


        [InverseProperty(nameof(Team.Town))]   // Inverse property is not needed when we have only one relation from the same Entity, could be used for better understanding. 
        public virtual ICollection<Team> Teams { get; set; }

    }
}




 
