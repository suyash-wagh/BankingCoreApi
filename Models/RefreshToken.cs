using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingCoreApi.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateCreated  { get; set; }
        public DateTime DateExpire { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }

}
