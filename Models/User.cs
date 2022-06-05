using Microsoft.AspNetCore.Identity;

namespace BankingCoreApi.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Balance { get; set; }

    }
}
