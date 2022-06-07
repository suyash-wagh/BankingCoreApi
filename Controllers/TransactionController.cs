using BankingCoreApi.Infrastructure;
using BankingCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BankingCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        
        private readonly BankingDbContext db;

        [Authorize(Roles = Roles.User)]
        [HttpGet("transactions")]
        public IActionResult get()
        {
            return Ok("Authorized");
        }
    }
}
