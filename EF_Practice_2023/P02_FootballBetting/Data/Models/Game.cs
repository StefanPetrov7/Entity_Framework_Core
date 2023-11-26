using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        public Game()
        {
            this.PlayersStatistics = new HashSet<PlayerStatistic>();
            this.Bets = new HashSet<Bet>();
        }

        public int GameId { get; set; }

        public int HomeTeamId { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        public Team? HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        public Team? AwayTeam { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime DateTime { get; set; }

        public double HomeTeamBetRate { get; set; }

        public double AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        public int Result { get; set; }

        [InverseProperty(nameof(PlayerStatistic.Game))]
        public ICollection<PlayerStatistic>? PlayersStatistics { get; set; }

        [InverseProperty(nameof(Bet.Game))]
        public ICollection<Bet>? Bets { get; set; }



    }
}
