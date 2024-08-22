using System.Data;
using Microsoft.VisualBasic;
using Project0.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Azure.Identity;



public class AdminOperations
{
    //Connection string to connect to the SQL database
    //string connectionString = "Server=DESKTOP-C0M19OK\\COLUMBUSSERVER;Database=Project0;Trusted_Connection=True;TrustServerCertificate=True;";
  
    //Database context for interacting with the database
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();
    //Method to check if the admin login is valid
    public bool CheckAdminLogin(string? username, string? password)
    {
        //Query the database to find an admin user with the given username and password
        var checkAdmin = (from a in db.Users
                        where a.Username == username && a.Password == password && a.UserRole == "Admin"
                        select a).SingleOrDefault();

        //Return true if the admin user exists
       return checkAdmin != null;
    }

    //Method to display the admin menu options
    public void ShowAdminMenu()
    {
        Console.WriteLine("Admin Menu");
        Console.WriteLine("1. Create new account");
        Console.WriteLine("2. Delete account");
        Console.WriteLine("3. Edit account details");
        Console.WriteLine("4. Display summary");
        Console.WriteLine("5. View Pending Requests");
        Console.WriteLine("6. Approve Request");
        Console.WriteLine("7. Reject Request");
        Console.WriteLine("8. Change Password");
        Console.WriteLine("9. Exit");
    }

    //Method to create a new account
    public void CreateCustomerAccount(AccountInfo accObj)
    {
        // using (SqlConnection connection = new SqlConnection(connectionString))
        
        try
        {
        //Add the new account to the database and Save the changes to the database
        db.AccountInfos.Add(accObj);
        db.SaveChanges();
        }
        catch(Exception ex)
        {
            //Handle errors that may occur while creating the account
            Console.WriteLine("There was an error creating the customer account: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }

    //Method to create a new user account
    public void CreateUserAccount(string username, string password, string role)
    {
        try
        {
            //Add the new user to the database and save changes
            db.Users.Add(new User
            {
                Username = username,
                Password = password,
                UserRole = role
            });
            db.SaveChanges();
            Console.WriteLine($"{role} account created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was an error creating the user account: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("Inner exception: " + ex.InnerException.Message);
            }
        }
    }
    //Method to delete an account by its account number
    public void DeleteAccount(int accNo)
    {
        //Find the account using the account number
       var accObj = db.AccountInfos.Find(accNo);
       if(accObj != null)
       {
        //Find and remove the associated user from the users table in SQL
        
        var userObj = db.Users.Find(accObj.AccUsername);
        if (userObj != null)
        {
            db.Users.Remove(userObj);
        }
        //Remove the account from the database (account_info table)
        //Save changes after deletion
        db.AccountInfos.Remove(accObj);
        db.SaveChanges();
       }
       else
       {
        Console.WriteLine("Account not found.");
       }
    }

    //Method to edit the details of an account
    public void EditAccountDetails(AccountInfo changes)
    {
        //Find the account using account number
        AccountInfo? acc = db.AccountInfos.Find(changes.AccNo);

        if(acc !=null)
        {
            //Update the account details with new values and save changes
            acc.AccName = changes.AccName;
            acc.AccType = changes.AccType;
            acc.AccBalance = changes.AccBalance;
            acc.AccIsActive = changes.AccIsActive;

            db.SaveChanges();
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
    }

    //Method to get an account by its number, used in editing account details
     public AccountInfo? GetAccountByNumber(int accNo)
    {
        return db.AccountInfos.Find(accNo);
    }

    //Method to get a list of all accounts, used to display a summary of all acounts
    public IEnumerable<AccountInfo> GetAllAccounts()
    {
        return db.AccountInfos.ToList();
    }

    //Method to change the User's password
    public void ChangeUserPassword()
    {
        try
        {
            Console.Write("Enter the username for password change: ");
            string username = Console.ReadLine() ?? string.Empty;

            var user = db.Users.Find(username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.Write("Enter the current password: ");
            string currentPassword = Console.ReadLine() ?? string.Empty;

            if (user.Password != currentPassword)
            {
                Console.WriteLine("The current password is incorrect.");
                return;
            }

            Console.Write("Enter the new password: ");
            string newPassword = Console.ReadLine() ?? string.Empty;

            user.Password = newPassword;
            db.SaveChanges();

            Console.WriteLine("The password has been successfully changed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while changing the password: {ex.Message}");
        }
    }
}