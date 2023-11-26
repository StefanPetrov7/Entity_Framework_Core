using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
            this.PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        public int PlayerId { get; set; }

        public string? Name { get; set; }

        public int SquadNumber { get; set; }

        public int Assists { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        public int PositionId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public Position? Position { get; set; }

        public bool IsInjured { get; set; }

        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team? Team { get; set; }

        [InverseProperty(nameof(PlayerStatistic.Player))]
        public ICollection<PlayerStatistic>? PlayersStatistics { get; set; }

    }
}
