using BankingCoreApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingCoreApi.Infrastructure
{
    public class BankingDbContext : IdentityDbContext<User>, IBankDbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }
        
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
    
}
