using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class Game
    {
        public Game()
        {
            this.Bets = new HashSet<Bet>();
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
        }

        public int GameId { get; set; }


        public float AwayTeamBetRate { get; set; }


        public int AwayTeamGoals { get; set; }


        public int AwayTeamId { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; }

        public float HomeTeamBetRate { get; set; }

        public int HomeTeamGoals { get; set; }


        public int HomeTeamId { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; }

        public float DrawBetRate { get; set; }


        public string Result { get; set; } 


        public DateTime DateTime { get; set; }


        [InverseProperty("Game")]
        public ICollection<Bet> Bets { get; set; }

        [InverseProperty("Game")]
        public ICollection <PlayerStatistic> PlayerStatistics { get; set; }

    }
}
