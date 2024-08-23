using Azure;
using Microsoft.Data.SqlClient;
using Project0.Models;
using System.Data.SqlClient;
using System.Linq;
public class CustomerOperations
{
    //Database context for interacting with the database
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();
    //Method to check if the customer's login credentials are correct
    public bool CheckCustomerLogin(string? username, string? password)
    {
        //Query to find the customer with the given username and role "Customer"
        var checkCustomer = (from a in db.Users
                             where a.Username == username && a.Password == password && a.UserRole == "Customer"
                             select a).SingleOrDefault(); //Using Single() returns an Exception if there's no results

        //If true, return checkCustomer
        return checkCustomer != null;

    }
    
    //Method to display the customer menu options
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

    //Method to get the details of a specific account by its number
    public AccountInfo? GetAccountDetails(int accNo)
    {
        return db.AccountInfos.Find(accNo); //Find returns null if no account is found   
    }

    //Method to request a check book for a specific account
    public void RequestCheckBook(int accNo)
    {
        //Create a new service request for a check book
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
            db.SaveChanges(); //Save changes to the database
            Console.WriteLine("Check book requested.");
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    //Method to create a new account type for the customer
    public void CreateNewAccountType(string? username, string? accType)
    {
        try
        {
            var user = db.Users.Find(username);  //Find the user by username
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

                newAcc.AccIsActive = true; //Set account status to active
                newAcc.AccUsername = username; //Set the username for the account

                db.AccountInfos.Add(newAcc); //Add the new account to the database
                db.SaveChanges(); //Save changes to the database

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
                    //Handle incorrect temporary password
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
                        return; //Exit the method if the user does not want to request a new password
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
