using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        public int UserId { get; set; }

        [Required]
        public decimal? Balance { get; set; }


        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }


        [Required]
        public string Username { get; set; }

        [InverseProperty("User")]
        public ICollection<Bet> Bets { get; set; }

    }
}
