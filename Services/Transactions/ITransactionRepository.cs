using System.Collections.Generic;
using BankingCoreApi.Models;

namespace BankingCoreApi.Services.Transactions
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        void DeleteAll(string UserId);
        List<Transaction> GetAll();
        List<Transaction> GetAllById(string UserId);
        void DeleteAll();
    }
}
