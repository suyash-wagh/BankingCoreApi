using BankingCoreApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BankingCoreApi.Infrastructure
{
    public class RolesInitializer
    {
        public static async Task AddRolesToDataBase(IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                }
                if (!await roleManager.RoleExistsAsync(Roles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.User));
                }
            }
        }
    }
}
