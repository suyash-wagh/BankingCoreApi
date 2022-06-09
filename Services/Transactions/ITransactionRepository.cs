using System.Collections.Generic;
using BankingCoreApi.DTOs;
using BankingCoreApi.Models;

namespace BankingCoreApi.Services.Transactions
{
    public interface ITransactionRepository
    {
        void Add(string userID, TransactionDTO transaction);
        void DeleteAll(string UserId);
        List<Transaction> GetAll();
        List<Transaction> GetAllById(string UserId);
        void DeleteAll();
    }
}
