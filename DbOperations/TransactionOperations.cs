using Microsoft.Data.SqlClient;
using Project0.Models;
using System.Data.SqlClient;
using System.Linq;
public class TransactionOperations
{
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();
    public void Withdraw(int accNo, double amount)
    {
        var account = db.AccountInfos.Find(accNo);
        if (account != null && account.AccBalance >= amount)
        {
            account.AccBalance -= amount;

            db.Transactions.Add(new Transaction
            {
                AccNo = accNo,
                TransactionType = "Withdrawal",
                TransactionAmount = amount,
                TransactionDate = DateTime.Now
            });

            db.SaveChanges();
            Console.WriteLine("Withdrawal done successfully.");
        }
        else
        {
            Console.WriteLine("Insufficient balance or account not found.");
        }
    }
    public void Deposit(int accNo, double amount)
    {
        var account = db.AccountInfos.Find(accNo);
        if (account != null)
        {
            account.AccBalance += amount;
            
            db.Transactions.Add(new Transaction
            {
                AccNo = accNo,
                TransactionType = "Deposit",
                TransactionAmount = amount,
                TransactionDate = DateTime.Now
            });

            db.SaveChanges();
            Console.WriteLine("Deposit done successfully.");
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
        
    }


    public void Transfer(int sourceAccNo, int recAccNo, double transferAmount)
    {
        using (var transaction = db.Database.BeginTransaction())
        {
            try
            {
                var fromAccount = db.AccountInfos.Find(sourceAccNo);
                var toAccount = db.AccountInfos.Find(recAccNo);

                if (fromAccount == null || toAccount == null)
                {
                    Console.WriteLine("One or both accounts do not exist.");
                    return;
                }

                if (fromAccount.AccBalance < transferAmount)
                {
                    Console.WriteLine("Insufficient balance in the source account.");
                    return;
                }

                // Deduct from source account
                fromAccount.AccBalance -= transferAmount;

                // Add to destination account
                toAccount.AccBalance += transferAmount;

                // Record the transaction for the source account
                db.Transactions.Add(new Transaction
                {
                    AccNo = sourceAccNo,
                    TransactionType = "Transfer Out",
                    TransactionAmount = -transferAmount
                });

                // Record the transaction for the destination account
                db.Transactions.Add(new Transaction
                {
                    AccNo = recAccNo,
                    TransactionType = "Transfer In",
                    TransactionAmount = transferAmount
                });

                // Save changes to the database
                db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("An error occurred during the transfer: " + ex.Message);
            }
        }
    }

    public void Last5Transactions(int accNo)
    {
    try
    {
        using (var context = new P0UsmanBankingDbContext())
        {
            var last5Transactions = context.Transactions
                .Where(t => t.AccNo == accNo)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .Select(t => new
                {
                    t.TransactionDate,
                    t.TransactionType,
                    t.TransactionAmount
                });
            if (last5Transactions.Any())
            {
            foreach (var transaction in last5Transactions)
            {
                Console.WriteLine($"Date: {transaction.TransactionDate}, Type: {transaction.TransactionType}, Amount: {transaction.TransactionAmount}");
            }
            }
            else
            {
                Console.WriteLine("No transactions found for this account.");
            }
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }   
    }

}

