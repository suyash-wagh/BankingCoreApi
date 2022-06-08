using BankingCoreApi.Infrastructure;
using BankingCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BankingDbContext _db;

        public UserController(BankingDbContext db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_db.Users.ToList());
        }

        // GET api/<UserController>/5
        [Authorize(Roles = Roles.User)]
        [HttpGet("getByEmail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            return Ok(_db.Users.Where(u => u.Email == email).Select(u => u));
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
