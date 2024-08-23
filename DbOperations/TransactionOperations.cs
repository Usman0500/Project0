using Microsoft.Data.SqlClient;
using Project0.Models;
using System.Data.SqlClient;
using System.Linq;
public class TransactionOperations
{
    //Database context for interacting with the database
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();
    //Method to withdraw a specifiecd amount from an accuont
    public void Withdraw(int accNo, double amount)
    {
        var account = db.AccountInfos.Find(accNo); //Find the accoount by its number
        if (account != null && account.AccBalance >= amount) //Check if the account exists and has sufficient balance
        {
            account.AccBalance -= amount; //Deduct the amount from the account balance

            //Record the withdrawal transaction
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
    //Method to deposit a specified amount into an account
    public void Deposit(int accNo, double amount)
    {
        var account = db.AccountInfos.Find(accNo);
        if (account != null)
        {
            account.AccBalance += amount; //Add the amount to the account balance
            
            //Record the deposit transaction
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

    //Method to transfer a specified amount from one account to another
    public void Transfer(int sourceAccNo, int recAccNo, double transferAmount)
    {
        using (var transaction = db.Database.BeginTransaction()) //Begin a database transaction
        {
            try
            {
                var fromAccount = db.AccountInfos.Find(sourceAccNo); //Find the source account
                var toAccount = db.AccountInfos.Find(recAccNo); //Find the destination account

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

                //Deduct from source account
                fromAccount.AccBalance -= transferAmount;

                //Add to destination account
                toAccount.AccBalance += transferAmount;

                //Record the transaction for the source account
                db.Transactions.Add(new Transaction
                {
                    AccNo = sourceAccNo,
                    TransactionType = "Transfer Out",
                    TransactionAmount = -transferAmount
                });

                //Record the transaction for the destination account
                db.Transactions.Add(new Transaction
                {
                    AccNo = recAccNo,
                    TransactionType = "Transfer In",
                    TransactionAmount = transferAmount
                });

                //Save changes to the database
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
    //Method to display the last 5 transactions for a specific amount
    public void Last5Transactions(int accNo)
    {
    try
    {
        using (var context = new P0UsmanBankingDbContext()) //Use a new database context for this operation 
        {
            var last5Transactions = context.Transactions
                .Where(t => t.AccNo == accNo) //Filter transactions by account number
                .OrderByDescending(t => t.TransactionDate) //Order transactions by date in descending order
                .Take(5) //Take the latest 5 transactions
                .Select(t => new
                {
                    t.TransactionDate,
                    t.TransactionType,
                    t.TransactionAmount
                });
            if (last5Transactions.Any()) //Display each transaction if there are any
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

