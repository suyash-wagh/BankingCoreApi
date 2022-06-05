using System;

namespace BankingCoreApi.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime ExiresAt { get; set; }
    }
}
