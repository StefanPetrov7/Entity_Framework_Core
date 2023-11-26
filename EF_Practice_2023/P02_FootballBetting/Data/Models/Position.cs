using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();   
        }

        public int PositionId { get; set; }

        public string? Name { get; set; }

        [InverseProperty(nameof(Player.Position))]
        public ICollection<Player>? Players { get; set; }
    }
}
