using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
        }

        public int TeamId { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(3)")]
        public string Initials { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(2048)")]
        public string LogoUrl { get; set; }

        [Required]
        public string Name { get; set; }


        public int PrimaryKitColorId { get; set; }

        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryKitColor { get; set; }


        public int SecondaryKitColorId { get; set; }

        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        [InverseProperty("Team")]
        public ICollection<Player> Players { get; set; }

        [InverseProperty("HomeTeam")]
        public ICollection<Game> HomeGames { get; set; }

        [InverseProperty("AwayTeam")]
        public ICollection<Game> AwayGames { get; set; }
    }
}
