﻿using BankingCoreApi.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BankingCoreApi.Models;
using BankingCoreApi.DTOs;

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
        //Roles.Admin +","+
        [Authorize(Roles = Roles.User)]
        [HttpGet("getall/{userId}")]
        public IActionResult Get([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(_service.GetAllById(userId));
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost("add/{userID}")]
        public IActionResult Post([FromBody] TransactionDTO transaction, string userID)
        {
           _service.Add(userID, transaction);
            return Ok("Transaction added");
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
