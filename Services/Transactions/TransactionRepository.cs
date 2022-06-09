using BankingCoreApi.Infrastructure;
using System.Collections.Generic;
using BankingCoreApi.Models;
using System.Linq;
using System;
using BankingCoreApi.DTOs;

namespace BankingCoreApi.Services.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankingDbContext _db;

        public TransactionRepository(BankingDbContext db)
        {
            _db = db;
        }

        public void Add(string userID, TransactionDTO transaction)
        {
            var balance = _db.Users.Where(u => u.Id == userID).Select(u => u.Balance).First();
            User user;

            if (transaction.TransactionType == "D")
            {
                balance += Convert.ToInt32(transaction.Amount);
                user = _db.Users.Find(userID);
                user.Balance = balance;
            }
            else if(transaction.TransactionType == "W")
            {
                balance -= Convert.ToInt32(transaction.Amount);
                user = _db.Users.Find(userID);
                user.Balance = balance;
            }

            _db.Transactions.Add(new Transaction()
            {
                Amount = transaction.Amount,
                Created = DateTime.UtcNow,
                TransactionType = transaction.TransactionType,
                UserId = userID,
            });
            _db.SaveChanges();
        }

        public void DeleteAll(string UserId)
        {
            _db.Remove(GetAllById(UserId));
            _db.SaveChanges();
        }

        public void DeleteAll()
        {
            _db.Remove(GetAll());
            _db.SaveChanges();
        }

        public List<Transaction> GetAll()
        {
            return _db.Transactions.ToList();
        }

        public List<Transaction> GetAllById(string UserId)
        {
            return _db.Transactions.Where(t => t.UserId == UserId).ToList();
        }
    }
}
