using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models 
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(ValidationsCostants.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(ValidationsCostants.UserPasswordLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(ValidationsCostants.UserEmailLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(ValidationsCostants.UserNameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Balance { get; set; }

        [InverseProperty(nameof(Bet.User))]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}




