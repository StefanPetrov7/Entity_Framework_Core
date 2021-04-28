﻿using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class PlayerStatistic
    {

        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public int Assists { get; set; }

        public int MinutesPlayed { get; set; }

        public int ScoredGoals { get; set; }
    }
}
