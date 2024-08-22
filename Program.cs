using System;
using System.Data.Common;
using Project0.Models;

try
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Welcome to Phenix Banking");
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Admin");
        Console.WriteLine("3. Exit");

        int choice = GetChoice();

        switch (choice)
        {
            case 1:
                CustomerMenu();
                break;
            case 2:
                AdminMenu();
                break;
            case 3:
                Console.WriteLine("Exiting the application...");
                return;
            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                Console.ReadKey();
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error has occurred in the Main Menu: " + ex.Message);
}
        

static int GetChoice()
{
    int choice;
    while (true)
    {
        try
        {
            choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
}

static void CustomerMenu()
{
    try
    {
        Console.Clear();
        Console.WriteLine("Customer Login");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        CustomerOperations customer = new CustomerOperations();
        TransactionOperations newTransaction = new TransactionOperations();

        if (customer.CheckCustomerLogin(username, password))
        {
            while (true)
            {
                
                Console.Clear();
                customer.ShowCustomerMenu();
                
                int accNo;
                int choice = GetChoice();

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter your Account Number:");
                        accNo = Convert.ToInt32(Console.ReadLine());

                        var account = customer.GetAccountDetails(accNo);

                        if (account != null)
                        {
                            Console.WriteLine("Account Details:");
                            Console.WriteLine("-----------------------------------------------------");
                            Console.WriteLine($"Account No: {account.AccNo}");
                            Console.WriteLine($"Account Name: {account.AccName}");
                            Console.WriteLine($"Account Type: {account.AccType}");
                            Console.WriteLine($"Account Balance: {account.AccBalance:C}");
                            Console.WriteLine($"Account Active: {(account.AccIsActive.HasValue ? (account.AccIsActive.Value ? "Yes" : "No") : "Unknown")}");
                            Console.WriteLine($"Account Username: {account.AccUsername}");
                            Console.WriteLine("-----------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter your Account Number for withdrawal:");
                        accNo = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter amount to withdraw:");
                        double withdrawAmount = Convert.ToDouble(Console.ReadLine());
                        newTransaction.Withdraw(accNo, withdrawAmount);
                        
                        break;

                    case 3:
                        Console.WriteLine("Enter your Account Number for deposit:");
                        accNo = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter amount to deposit:");
                        double depositAmount = Convert.ToDouble(Console.ReadLine());
                        newTransaction.Deposit(accNo, depositAmount);
                        
                        break;

                    case 4:
                        Console.WriteLine("Enter your Source Account Number for transfer:");
                        int sourceAccNo = Convert.ToInt32(Console.ReadLine());
                        
                        Console.WriteLine("Enter recipient's account number:");
                        int recAccNo = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Enter amount to transfer:");
                        double transferAmount = Convert.ToDouble(Console.ReadLine());

                        newTransaction.Transfer(sourceAccNo, recAccNo, transferAmount);
                        Console.WriteLine("Transfer successful");
                        break;

                    case 5:
                        Console.WriteLine("Enter your account number: ");
                        accNo = Convert.ToInt32(Console.ReadLine());
                        newTransaction.Last5Transactions(accNo);
                        break;
                    case 6:
                        Console.WriteLine("Request check book");
                        Console.Write("Enter your Account Number: ");
                        accNo = Convert.ToInt32(Console.ReadLine());
                        customer.RequestCheckBook(accNo);
                        break;
                    case 7:
                        Console.WriteLine("Change Password");
                        Console.Write("Enter your Account Number: ");
                        accNo = Convert.ToInt32(Console.ReadLine());
                        customer.PasswordChange(accNo);
                        break;

                    case 8:
                        Console.WriteLine("Create a New Account");
                        Console.Write("Enter new Account Type (Savings/Checking/Loan): ");
                        string? newAccType = Console.ReadLine();
                        customer.CreateNewAccountType(username, newAccType);
                        break;

                    case 9:
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                    }
                Console.ReadKey(); 
            }
        }
        else
        {
            Console.WriteLine("Invalid credentials. Returning to main menu.");
            Console.ReadKey();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred in the Customer Menu: " + ex.Message);
    }
}

static void AdminMenu()
{
    try
    {
        Console.Clear();
        Console.WriteLine("Admin Login");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        AdminOperations admin = new AdminOperations();
        NewRequestOperations request = new NewRequestOperations();

        if (admin.CheckAdminLogin(username, password))
        {
            while (true)
            {
                Console.Clear();
                admin.ShowAdminMenu();

                int choice = GetChoice();

                switch (choice)
                {
                    case 1:
                        string isAdmin;
                        do
                        {
                            Console.WriteLine("Are you creating an Admin account? (yes/no):");
                            isAdmin = Console.ReadLine().ToLower();
                            if (isAdmin != "yes" && isAdmin != "no")
                            {
                                Console.WriteLine("Invalid Input. Please enter 'yes' or 'no'");
                            }
                        } while (isAdmin != "yes" && isAdmin != "no");

                        string userRole = (isAdmin == "yes") ? "Admin" : "Customer";

                        if (userRole == "Customer")
                        {
                            AccountInfo newAcc = new AccountInfo();
                            
                            Console.WriteLine("Enter Account Details");
                            
                            Console.WriteLine("Enter Account No:");
                            newAcc.AccNo = Convert.ToInt32(Console.ReadLine());
                            
                            Console.WriteLine("Enter Account Holder's Name:");
                            newAcc.AccName = Console.ReadLine();
                            
                            Console.WriteLine("Enter Account Type (Savings/Checking/Loan):");
                            newAcc.AccType = Console.ReadLine();
                            
                            Console.WriteLine("Enter Initial Account Balance:");
                            newAcc.AccBalance = Convert.ToDouble(Console.ReadLine());
                            
                            Console.WriteLine("Is the Account Active? (True/False):");
                            newAcc.AccIsActive = Convert.ToBoolean(Console.ReadLine());

                            Console.WriteLine("Enter the account Username:");
                            string userName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter a Temporary Password:");
                            string passWord = Console.ReadLine() ?? string.Empty;

                            // Create the user account in the users table
                            admin.CreateUserAccount(userName, passWord, userRole);

                            // Set the account's username and create the customer account
                            newAcc.AccUsername = userName;
                            admin.CreateCustomerAccount(newAcc);

                            Console.WriteLine("Customer account created successfully.");
                        }
                        else if (userRole == "Admin")
                        {
                            Console.WriteLine("Enter the account Username:");
                            string userName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter a Temporary Password:");
                            string passWord = Console.ReadLine() ?? string.Empty;

                            // Create the admin account in the users table only
                            admin.CreateUserAccount(userName, passWord, userRole);

                            Console.WriteLine("Admin account created successfully.");
                        }

                        break;

                    case 2:
                        
                        Console.WriteLine("Delete Account");
                        Console.WriteLine("Enter the Account No to delete the account");
                        int accNo = Convert.ToInt32(Console.ReadLine());
                        admin.DeleteAccount(accNo);
                        Console.WriteLine("Account deleted.");
                        break;

                    case 3:
                        Console.WriteLine("Edit Account Details");
                        Console.WriteLine("Enter the Account No to edit");
                        int accNoToEdit = Convert.ToInt32(Console.ReadLine());

                        var existingAcc = admin.GetAccountByNumber(accNoToEdit);

                        if (existingAcc != null)
                        {
                            Console.WriteLine("Enter new details for the account");

                            Console.WriteLine("Enter Account No");
                            existingAcc.AccNo = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter Customer's Name");
                            existingAcc.AccName = Console.ReadLine();

                            Console.WriteLine("Enter Account Balance");
                            existingAcc.AccBalance = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Is the Account Active? Press 1 for Yes and 0 for No");
                            existingAcc.AccIsActive = Convert.ToBoolean(Console.ReadLine());

                            admin.EditAccountDetails(existingAcc);
                            Console.WriteLine("Account details updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }

                        break;

                    case 4:
                        var accounts = admin.GetAllAccounts();

                        if (accounts.Any())
                        {
                            var accTypeCounts = accounts
                                .GroupBy(a => a.AccType)
                                .Select(g => new 
                                {
                                    AccountType = g.Key, 
                                    Count = g.Count()
                                })
                                .ToList();

                            Console.WriteLine("Account Summary:");
                            Console.WriteLine("-----------------------------------------------------");
                            Console.WriteLine("| Account Type | Number of Accounts |");
                            Console.WriteLine("-----------------------------------------------------");

                            foreach (var acc in accTypeCounts)
                            {
                                Console.WriteLine($"| {acc.AccountType, -11} | {acc.Count, -18} |");
                            }

                            Console.WriteLine("-----------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("No accounts found");
                        }
                        break;

                    case 5:
                        Console.WriteLine("Viewing pending requests.");
                        request.ViewPendingRequests();
                        
                        break;

                    case 6:
                        Console.WriteLine("Approve request");
                        Console.Write("Enter Request ID to approve: ");
                        int approveId = Convert.ToInt32(Console.ReadLine());
                        request.ApproveRequest(approveId);

                        break;

                    case 7:
                        Console.WriteLine("Reject request");
                        Console.Write("Enter Request ID to reject: ");
                        int rejectId = Convert.ToInt32(Console.ReadLine());
                        request.RejectRequest(rejectId);

                        break;
                    
                    case 8:
                        AdminOperations adminOps = new AdminOperations();
                        adminOps.ChangeUserPassword();
                        break;

                    case 9:
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
                Console.ReadKey(); 
            }
        }
        else
        {
            Console.WriteLine("Invalid credentials. Returning to main menu.");
            Console.ReadKey(); 
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occured in the Admin Menu: " + ex.Message);
    }
}
 