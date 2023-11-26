using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();  
            this.Players = new HashSet<Player>();   
        }

        public int TownId { get; set; }

        public string? Name { get; set; }

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }

        [InverseProperty(nameof(Team.Town))]
        public ICollection<Team>? Teams { get; set; }

        [InverseProperty(nameof(Player.Town))]
        public ICollection<Player>? Players { get; set; }
    }
}
