using BankingCoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingCoreApi.Infrastructure
{
    public interface IBankDbContext
    {
        DbSet<Transaction> Transactions { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

