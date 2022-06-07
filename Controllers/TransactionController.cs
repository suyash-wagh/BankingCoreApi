using BankingCoreApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BankingCoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly BankingDbContext db;
        [HttpGet("transactions")]
        public IActionResult get()
        {
            return Ok("Authorized.");
        }
    }
}
