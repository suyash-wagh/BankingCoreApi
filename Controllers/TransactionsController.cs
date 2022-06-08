using BankingCoreApi.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BankingCoreApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("getall")]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_service.GetAll());
        }

        [Authorize(Roles = Roles.Admin +","+ Roles.User)]
        [HttpGet("getall/{id}")]
        public IActionResult Get(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_service.GetAllById(userId));
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        [HttpPost("add")]
        public void Post([FromBody] Transaction transaction)
        {
            _service.Add(transaction);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("deleteall/{id}")]
        public void Delete(string userId)
        {
            _service.DeleteAll(userId);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("deleteall")]
        public void DeleteAll()
        {
            _service.DeleteAll();
        }
    }
}
