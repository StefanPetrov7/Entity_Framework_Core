using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models 
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(ValidationsCostants.PositionNameMaxLength)]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Player.Position))]
        public virtual ICollection<Player> Players { get; set; }
    }
}





