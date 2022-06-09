using BankingCoreApi.DTOs;
using BankingCoreApi.Models;
using System.Collections.Generic;

namespace BankingCoreApi.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;

        public TransactionService(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public void Add(string userID, TransactionDTO transaction)
        {
            _repo.Add(userID, transaction);
        }

        public void DeleteAll(string UserId)
        {
            _repo.DeleteAll(UserId);
        }

        public void DeleteAll()
        {
            _repo.DeleteAll();
        }

        public List<Transaction> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Transaction> GetAllById(string UserId)
        {
            return _repo.GetAllById(UserId);
        }
    }
}
