using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();   
            this.Players = new HashSet<Player>();   
        }


        public int TeamId { get; set; }

        public string? Name { get; set; }

        public string? LogoUrl { get; set; }

        public string? Initials { get; set; }

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }

        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color? PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }

        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color? SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town? Town { get; set; }

        [InverseProperty(nameof(Game.HomeTeam))]
        public ICollection<Game>? HomeGames { get; set; }

        [InverseProperty(nameof(Game.AwayTeam))]
        public ICollection<Game>? AwayGames { get; set; }

        [InverseProperty(nameof(Player.Team))]
        public ICollection<Player>? Players { get; set; }


    }
}

