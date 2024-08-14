using System;


while (true)
{
    Console.Clear();
    Console.WriteLine("Welcome to the Bank");
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
    const string Username = "customer";
    const string Password = "password";

    Console.Clear();
    Console.WriteLine("Customer Login");
    Console.Write("Enter username: ");
    string username = Console.ReadLine();
    Console.Write("Enter password: ");
    string password = Console.ReadLine();

    if (username == Username && password == Password)
    {
        while (true)
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
            Console.WriteLine("8. Exit");

            int choice = GetChoice();

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Account details displayed.");
                    break;
                case 2:
                    Console.WriteLine("Withdrawal successful.");
                    break;
                case 3:
                    Console.WriteLine("Deposit successful.");
                    break;
                case 4:
                    Console.WriteLine("Transfer successful.");
                    break;
                case 5:
                    Console.WriteLine("Displaying last 5 transactions.");
                    break;
                case 6:
                    Console.WriteLine("Check book requested.");
                    break;
                case 7:
                    Console.WriteLine("Password changed.");
                    break;
                case 8:
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

static void AdminMenu()
{
    const string Username = "admin";
    const string Password = "adminpass";

    Console.Clear();
    Console.WriteLine("Admin Login");
    Console.Write("Enter username: ");
    string username = Console.ReadLine();
    Console.Write("Enter password: ");
    string password = Console.ReadLine();

    if (username == Username && password == Password)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Admin Menu");
            Console.WriteLine("1. Create new account");
            Console.WriteLine("2. Delete account");
            Console.WriteLine("3. Edit account details");
            Console.WriteLine("4. Display summary");
            Console.WriteLine("5. Reset customer password");
            Console.WriteLine("6. Approve check book request");
            Console.WriteLine("7. Exit");

            int choice = GetChoice();

            switch (choice)
            {
                case 1:
                    Console.WriteLine("New account created.");
                    break;
                case 2:
                    Console.WriteLine("Account deleted.");
                    break;
                case 3:
                    Console.WriteLine("Account details updated.");
                    break;
                case 4:
                    Console.WriteLine("Displaying summary.");
                    break;
                case 5:
                    Console.WriteLine("Customer password reset.");
                    break;
                case 6:
                    Console.WriteLine("Check book request approved.");
                    break;
                case 7:
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
 