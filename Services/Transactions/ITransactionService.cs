using BankingCoreApi.Models;
using System.Collections.Generic;

namespace BankingCoreApi.Services.Transactions
{
    public interface ITransactionService
    {
        void Add(Transaction transaction);
        void DeleteAll(string UserId);
        List<Transaction> GetAll();
        List<Transaction> GetAllById(string UserId);
        void DeleteAll();
    }
}
