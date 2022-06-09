using BankingCoreApi.Infrastructure;
using BankingCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


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

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("getUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_db.Users.ToList());
        }


        [Authorize(Roles = Roles.User)]
        [HttpGet("getByEmail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            return Ok(_db.Users.Where(u => u.Email == email).Select(u => u));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("delete/{userID}")]
        public void Delete(User user)
        {
            _db.Users.Remove(user);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("lock/{userId}")]
        public IActionResult Lock([FromRoute] string userId)
        {
            _db.Users.Where(u => u.Id == userId).SingleOrDefault().IsLocked = true;
            _db.SaveChanges();
            return Ok("user locked");
        }

    }
}
