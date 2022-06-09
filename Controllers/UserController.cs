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
        [HttpGet("getUsers")]
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

        // DELETE api/<UserController>/5
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete/{userID}")]
        public void Delete(User user)
        {
            _db.Users.Remove(user);
        }
    }
}
