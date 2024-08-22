using Azure;
using Microsoft.Data.SqlClient;
using Project0.Models;
using System.Data.SqlClient;
using System.Linq;
public class CustomerOperations
{
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();
    public bool CheckCustomerLogin(string? username, string? password)
    {
        var checkCustomer = (from a in db.Users
                             where a.Username == username && a.Password == password && a.UserRole == "Customer"
                             select a).SingleOrDefault(); //using Single() returns an Exception

        return checkCustomer != null;

    }

    public void ShowCustomerMenu()
    {
        Console.Clear();
        Console.WriteLine("Customer Menu");
        Console.WriteLine("1. Check account details");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Deposit");
        Console.WriteLine("4. Transfer");
        Console.WriteLine("5. Last 5 transactions");
        Console.WriteLine("6. Request Check book");
        Console.WriteLine("7. Change password");
        Console.WriteLine("8. Create a new account");
        Console.WriteLine("9. Exit");
    }

    public AccountInfo? GetAccountDetails(int accNo)
    {
        return db.AccountInfos.Find(accNo);    
    }


    public void RequestCheckBook(int accNo)
    {
        var account = db.AccountInfos.Find(accNo);
        if (account != null)
        {
            db.NewServiceRequests.Add(new NewServiceRequest
            {
                AccNo = accNo,
                ServiceType = "Check Book",
                RequestStatus = "Pending",
                RequestDate = DateTime.Now
            });
            db.SaveChanges();
            Console.WriteLine("Check book requested.");
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    public void CreateNewAccountType(string? username, string? accType)
    {
        try
        {
            var user = db.Users.Find(username);
            if (user != null)
            {
                AccountInfo newAcc = new AccountInfo();
                Console.WriteLine("Enter Account No:");
                newAcc.AccNo = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Account Name:");
                newAcc.AccName = Console.ReadLine();

                newAcc.AccType = accType;
                Console.WriteLine("Enter Initial Deposit:");
                newAcc.AccBalance = Convert.ToDouble(Console.ReadLine());

                newAcc.AccIsActive = true;
                newAcc.AccUsername = username;

                db.AccountInfos.Add(newAcc);
                db.SaveChanges();

                Console.WriteLine($"{accType} account created successfully");
            }
            else
            {
                Console.WriteLine("User not found. Cannot create account.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating a new account: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }

    public void PasswordChange(int accNo)
    {
        var account = db.AccountInfos.Find(accNo);
        if (account != null)
        {
            var user = db.Users.Find(account.AccUsername);
            if (user != null)
            {
                Console.Write("Enter the temporary password: ");
                string tempPassword = Console.ReadLine() ?? string.Empty;

                if (user.Password == tempPassword)
                {
                    Console.Write("Enter your new password: ");
                    string newPassword = Console.ReadLine() ?? string.Empty;

                    user.Password = newPassword;
                    db.SaveChanges();

                    Console.WriteLine("Password has been successfully changed.");
                }
                else
                {
                    Console.WriteLine("The temporary password is incorrect.");
                    Console.WriteLine("Would you like to request a new password?");
                    string response = (Console.ReadLine() ?? string.Empty).ToLower();

                    if (response == "yes")
                    {
                        db.NewServiceRequests.Add(new NewServiceRequest
                        {
                        AccNo = accNo,
                        ServiceType = "Password Change",
                        RequestStatus =  "Pending",
                        RequestDate = DateTime.Now
                        });
                        db.SaveChanges();
                        Console.WriteLine("Password change requested.");
                    }
                    else if (response == "no")
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                    }
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

}
