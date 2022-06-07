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
        private readonly BankingDbContext _db;

        public TransactionController(BankingDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("transactions")]
        public IActionResult get()
        {
            return Ok(_db.Transactions.ToList());
        }
    }
}
