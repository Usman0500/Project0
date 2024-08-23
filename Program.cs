using System;
using System.Data.Common;
using Project0.Models;



try
{
    while (true) //Infinite loop to keep the program running until the user decides to exit
    {
        Console.Clear(); //Clears the console screen
        Console.WriteLine("Welcome to Phenix Banking");
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Admin");
        Console.WriteLine("3. Exit");

        int choice = GetChoice(); //Get user choice from the menu

        switch (choice) //Handle the user's menu selection
        {
            case 1:
                CustomerMenu(); //Navigate to the customer menu
                break;
            case 2:
                AdminMenu(); //Navigate to the admin menu
                break;
            case 3:
                Console.WriteLine("Exiting the application...");
                return; //Exit the program
            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                Console.ReadKey(); //Wait for the user to press a key
                break;
        }
    }
}
catch (Exception ex) //Catch any exceptions that might occur
{
    Console.WriteLine("An error has occurred in the Main Menu: " + ex.Message);
}

//Function to get the user's choice and validate it
static int GetChoice()
{
    int choice;
    while (true)
    {
        try
        {
            choice = Convert.ToInt32(Console.ReadLine()); //Convert user input to an integer
            return choice;
        }
        catch (FormatException) //Handle invalid input that can't be converted to an integer
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
}

//Function to display the customer menu and handle customer operations
static void CustomerMenu()
{
    try
    {
        Console.Clear(); //Clears the console screen
        Console.WriteLine("Customer Login");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine(); //Read the username
        Console.Write("Enter password: ");
        string? password = Console.ReadLine(); //Read the password

        //Create instances of the customer and transaction operations classes
        CustomerOperations customer = new CustomerOperations();
        TransactionOperations newTransaction = new TransactionOperations();

        //Check if the login credentials are correct
        if (customer.CheckCustomerLogin(username, password))
        {
            while (true) //Infinite loop for customer operations
            {
                Console.Clear(); //Clears the console screen
                customer.ShowCustomerMenu(); //Display customer menu options

                int accNo; //Variable to store account number
                int choice = GetChoice(); //Get the user's menu choice

                switch (choice)
                {
                    case 1: //View Account Details
                        Console.WriteLine("Enter your Account Number:");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user

                        var account = customer.GetAccountDetails(accNo); //Retrieve account details

                        if (account != null) //If the account is found, display its details
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
                            Console.WriteLine("Account not found."); //If the account isn't found, display a message
                        }
                        break;

                    case 2: //Withdraw money
                        Console.WriteLine("Enter your Account Number for withdrawal:");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user
                        Console.WriteLine("Enter amount to withdraw:");
                        double withdrawAmount = Convert.ToDouble(Console.ReadLine()); //Get withdrawal amount from user
                        newTransaction.Withdraw(accNo, withdrawAmount); //Perform the withdrawal
                        break;

                    case 3: //Deposit money
                        Console.WriteLine("Enter your Account Number for deposit:");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user
                        Console.WriteLine("Enter amount to deposit:");
                        double depositAmount = Convert.ToDouble(Console.ReadLine()); //Get deposit amount from user
                        newTransaction.Deposit(accNo, depositAmount); //Perform the deposit
                        break;

                    case 4: //Transfer money to another account
                        Console.WriteLine("Enter your Source Account Number for transfer:");
                        int sourceAccNo = Convert.ToInt32(Console.ReadLine()); //Get the source account number
                        Console.WriteLine("Enter recipient's account number:");
                        int recAccNo = Convert.ToInt32(Console.ReadLine()); //Get the recipient's account number
                        Console.WriteLine("Enter amount to transfer:");
                        double transferAmount = Convert.ToDouble(Console.ReadLine()); //Get transfer amount from user
                        newTransaction.Transfer(sourceAccNo, recAccNo, transferAmount); //Perform the transfer
                        Console.WriteLine("Transfer successful");
                        break;

                    case 5: //View the last 5 transactions
                        Console.WriteLine("Enter your account number: ");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user
                        newTransaction.Last5Transactions(accNo); //Display the last 5 transactions
                        break;

                    case 6: //Request a checkbook
                        Console.WriteLine("Request check book");
                        Console.Write("Enter your Account Number: ");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user
                        customer.RequestCheckBook(accNo); //Request the checkbook
                        break;

                    case 7: //Change password
                        Console.WriteLine("Change Password");
                        Console.Write("Enter your Account Number: ");
                        accNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user
                        customer.PasswordChange(accNo); //Perform the password change
                        break;

                    case 8: //Create a new account type
                        Console.WriteLine("Create a New Account");
                        Console.Write("Enter new Account Type (Savings/Checking/Loan): ");
                        string? newAccType = Console.ReadLine(); //Get new account type from user
                        customer.CreateNewAccountType(username, newAccType); //Create the new account type
                        break;

                    case 9: //Return to the main menu
                        return;

                    default: //Handle invalid menu choices
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
                Console.ReadKey(); //Wait for the user to press a key before continuing
            }
        }
        else
        {
            Console.WriteLine("Invalid credentials. Returning to main menu."); //Invalid login credentials
            Console.ReadKey(); //Wait for the user to press a key before returning to the main menu
        }
    }
    catch (Exception ex) //Catch any exceptions that might occur
    {
        Console.WriteLine("An error occurred in the Customer Menu: " + ex.Message);
    }
}

//Function to display the admin menu and handle admin operations
static void AdminMenu()
{
    try
    {
        Console.Clear(); //Clears the console screen
        Console.WriteLine("Admin Login");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine(); //Read the username
        Console.Write("Enter password: ");
        string? password = Console.ReadLine(); //Read the password

        //Create instances of the admin and request operations classes
        AdminOperations admin = new AdminOperations();
        NewRequestOperations request = new NewRequestOperations();

        //Check if the login credentials are correct
        if (admin.CheckAdminLogin(username, password))
        {
            while (true) //Infinite loop for admin operations
            {
                Console.Clear(); //Clears the console screen
                admin.ShowAdminMenu(); //Display admin menu options

                int choice = GetChoice(); //Get the user's menu choice

                switch (choice)
                {
                    case 1: //Create a new Admin or Customer account
                        string isAdmin;
                        do
                        {
                            Console.WriteLine("Are you creating an Admin account? (yes/no):");
                            isAdmin = Console.ReadLine().ToLower(); //Determine if creating an Admin or Customer account
                            if (isAdmin != "yes" && isAdmin != "no")
                            {
                                Console.WriteLine("Invalid Input. Please enter 'yes' or 'no'");
                            }
                        } while (isAdmin != "yes" && isAdmin != "no");

                        string userRole = (isAdmin == "yes") ? "Admin" : "Customer"; //Set the user role based on input

                        if (userRole == "Customer") //If creating a Customer account
                        {
                            AccountInfo newAcc = new AccountInfo();

                            Console.WriteLine("Enter Account Details");

                            Console.WriteLine("Enter Account No:");
                            newAcc.AccNo = Convert.ToInt32(Console.ReadLine()); //Get account number from user

                            Console.WriteLine("Enter Account Holder's Name:");
                            newAcc.AccName = Console.ReadLine(); //Get account holder's name from user

                            Console.WriteLine("Enter Account Type (Savings/Checking/Loan):");
                            newAcc.AccType = Console.ReadLine(); //Get account type from user

                            Console.WriteLine("Enter Initial Account Balance:");
                            newAcc.AccBalance = Convert.ToDouble(Console.ReadLine()); //Get initial balance from user
                            
                            Console.WriteLine("Is the Account Active? (True/False):");
                            newAcc.AccIsActive = Convert.ToBoolean(Console.ReadLine());

                            Console.WriteLine("Enter the account Username:");
                            string userName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter a Temporary Password:");
                            string passWord = Console.ReadLine() ?? string.Empty;

                            //Create the user account in the users table
                            admin.CreateUserAccount(userName, passWord, userRole);

                            //Set the account's username and create the customer account
                            newAcc.AccUsername = userName;
                            admin.CreateCustomerAccount(newAcc);

                            Console.WriteLine("Customer account created successfully.");
                        }
                        else if (userRole == "Admin")
                        {
                            //Create an Admin account
                            Console.WriteLine("Enter the account Username:");
                            string userName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter a Temporary Password:");
                            string passWord = Console.ReadLine() ?? string.Empty;

                            //.Create the admin account in the users table only
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
 