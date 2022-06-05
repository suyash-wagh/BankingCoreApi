using BankingCoreApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingCoreApi.Infrastructure
{
    public class BankingDbContext : IdentityDbContext<User>
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }
    }
}
