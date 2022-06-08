using BankingCoreApi.Infrastructure;
using System.Collections.Generic;
using BankingCoreApi.Models;
using System.Linq;

namespace BankingCoreApi.Services.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankingDbContext _db;

        public TransactionRepository(BankingDbContext db)
        {
            _db = db;
        }

        public void Add(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
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
