namespace ATMAPP
{
    public class ATM
    {

        const int SAVING_ACCOUNT = 1;
        const int DEBIT_CARD = 2;
        const int CREDIT_CARD = 3;
        const int INVESTMENT_ACCOUNT = 4;

        static double[] accountBalances = { 0.0, 1001.45, 850.0, -150.0, 10000.0 };

        static string[] accountNames = { "", "Savings Account", "Debit Card",
                                           "Credit Card", "Investment Account" };


        static void Main()
        {
            bool isRunning = true;
            while (isRunning == true)
            {
                AccountsMenu(ShowMainMenu());
            }

        } //end Main


        /// <summary>
        /// Displays the main menu screen with options.
        /// pre: Program is running.
        /// post: Takes you to the relevent menu or exits.
        /// </summary>
        static string ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Welcome to Victor's Bank ATM.\n");
            Console.WriteLine("\tTransaction Menu\n\t================");
            Console.WriteLine("\t1. Check Balance");
            Console.WriteLine("\t2. Withdrawal");
            Console.WriteLine("\t3. Transfer");
            Console.Write("\n\nPlease enter your choice 1, 2, 3 or 0 to exit: ");
            string menuOptionSelected = Console.ReadLine();
            return menuOptionSelected;
        }//end ShowMainMenu


        /// <summary>
        /// Takes us to the relevent menu.
        /// pre: Valid option has been selected on the MainMenu Screen
        /// post: Takes you to the relevent menu or exits.
        /// </summary>
        static void AccountsMenu(string mainMenuOptionSelected)
        {
            switch (mainMenuOptionSelected)
            {
                case "0":
                    ExitATM(); //0. Exit Program
                    break;
                case "1":
                    DisplayAccountsMenu(1); //1. Check Balance
                    AccountsActionMenu(1);
                    break;
                case "2":
                    DisplayAccountsMenu(2); //2. Withdrawal 
                    AccountsActionMenu(2);
                    break;
                case "3":
                    DisplayAccountsMenu(3); //3. Transfer
                    AccountsActionMenu(3);
                    break;
                default:
                    Console.Write("Incorrect selection, Please Try Again: 0 ... 3: ");
                    break;
            }

        }//end MenuTransition

        static void AccountsActionMenu(int mainMenuOptionSelected)
        {
            bool isRunning = true;
            while (isRunning == true)
            {
                int whichAccount;
                UserInputAccountsMenu(out whichAccount);
                if (whichAccount == 0)
                {
                    isRunning = false;
                    break;
                }
                switch (mainMenuOptionSelected)
                {
                    case 1:
                        DisplayBalance(whichAccount);
                        isRunning = UserInputYesNo(mainMenuOptionSelected);
                        break;
                    case 2:
                        WithdrawAmount(whichAccount);
                        isRunning = UserInputYesNo(mainMenuOptionSelected);
                        break;
                    case 3:
                        TransferPromtMenu(whichAccount);
                        isRunning = UserInputYesNo(mainMenuOptionSelected);
                        break;
                }
            }
        }//end AccountsActionMenu


        /// <summary>
        /// Displays balance of "whichAccount"
        /// pre:  whichAccount is either 1, 2, 3 or 4
        /// post: balance of "whichAccount" has been displayed
        /// </summary>
        /// <param name="whichAccount"> the account whose balance is to be displayed</param>
        static void DisplayBalance(int whichAccount)
        {
            DateTime date = DateTime.Now;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("\nCurrent Date: {0} ", date);
            Console.WriteLine("Balance of {0} is {1:C2}\n", accountNames[whichAccount], accountBalances[whichAccount]);
        }//end DisplayBalance


        /// <summary>
        /// Provided that the withdrawal amount does not leave whichAccount with less than the specified 
        /// minimum balance, the amount is withdrawn from whichAccount and if whichAccount is the Investment
        /// Account an additional fee of $1 is deducted.
        /// 
        /// pre: whichAccount is a valid account
        /// post: either acceptable amount is withdrawn from whichAccount or no withdrawal is performed
        /// </summary>
        /// <param name="whichAccount"> the account form which an amount is to be withdrawn</param>
        static void WithdrawAmount(int whichAccount)
        {
            Console.WriteLine("\n--------------------------------------------");
            Console.Write("{0} account balance is {1:C2} \nPlease enter how much to withdraw from {0}, or 0 to exit: ", accountNames[whichAccount], accountBalances[whichAccount], accountNames[whichAccount]);
            double amountToWithdraw = UserTransactionCashAmount(whichAccount, 1); // 1 being 'withdraw' transaction type.
            if (amountToWithdraw > 0)
            {
                DispenseCash(amountToWithdraw, whichAccount);
            }
        }//end WithdrawAmount


        /// <summary>
        /// Dispenses the specified amount in the minimumnumber of $20 and $50 dollar notes
        /// 
        /// pre: amount is as a combination of $20 and $50 only.
        /// post minimum number of notes has been dispensed.
        /// </summary>
        /// <param name="amountToWithdraw">the amount to be dispensed</param>
        static void DispenseCash(double amountToWithdraw, int whichAccount)
        {
            int numberOfFiftyNotes = 0;
            int numberOfTwentyNotes = 0;
            double amountToWithdrawCalculator = amountToWithdraw;
            while (amountToWithdrawCalculator > 0)
            {
                if ((amountToWithdrawCalculator < 100) && (amountToWithdrawCalculator > 50) && (amountToWithdrawCalculator % 20 == 0))
                {
                    numberOfTwentyNotes = (numberOfTwentyNotes + Convert.ToInt32(amountToWithdrawCalculator / 20));
                    amountToWithdrawCalculator = 0;
                }
                if (amountToWithdrawCalculator >= 50)
                {
                    amountToWithdrawCalculator = amountToWithdrawCalculator - 50;
                    numberOfFiftyNotes = numberOfFiftyNotes + 1;
                }
                if ((amountToWithdrawCalculator >= 20) && (amountToWithdrawCalculator < 50))
                {
                    amountToWithdrawCalculator = amountToWithdrawCalculator - 20;
                    numberOfTwentyNotes = numberOfTwentyNotes + 1;
                }
                if ((amountToWithdrawCalculator < 20) && (amountToWithdrawCalculator > 0))
                {
                    Console.WriteLine("Invalid amount specified. Unable to dispense in $20 and $50 notes.");
                    break;
                }
            }
            if (amountToWithdrawCalculator == 0)
            {
                accountBalances[whichAccount] = (accountBalances[whichAccount] - amountToWithdraw);
                Console.WriteLine("\n--------------------------------------------");
                Console.WriteLine("\nPlease collect your {0:C2} consisting of: ", amountToWithdraw);
                if(numberOfFiftyNotes > 0)
                {
                    Console.WriteLine("x{0} $50 notes.", numberOfFiftyNotes);
                }
                else
                {
                    Console.WriteLine("x{0} $20 notes.", numberOfTwentyNotes);
                }
                
                if (whichAccount == 4 )
                {
                    accountBalances[whichAccount] = accountBalances[whichAccount] - 1;
                    Console.WriteLine("\nA $1 transaction fee has been deducted for \nwithdrawing from your Investment Account.");
                }
                DisplayBalance(whichAccount);
            }
        }//end DispenseCash


        static void TransferPromtMenu(int fromAccount)
        {
            if (fromAccount != 0)
            {
                int toAccount;
                Console.Write("\nPlease Select the account to transfer to: ");
                Console.Write("\nPlease Enter Your Choice 1 ... 4 or 0 to exit: ");
                UserInputAccountsMenu(out toAccount);
                if (fromAccount == toAccount)
                {
                    Console.Write("\nYou cannot transfer to the same account.\n");
                }
                else if (toAccount != 0)
                {
                    TransferAmount(fromAccount, toAccount);
                }
            }
        }//end TransferPromtMenu


        /// <summary>
        ///  providing fromAccount has sufficient funds, an amount of money is transferred
        ///  to toAccount
        ///  pre: fromAccount and to account are different  valid accounts
        ///  post: if fromAccount has sufficient funds a transfer has occurred 
        ///         else no transfer occurred
        /// </summary>
        /// <param name="fromAccount">account from which money is to withdrawn</param>
        /// <param name="toAccount">account to which money is to be transferred</param>
        static void TransferAmount(int fromAccount, int toAccount)
        {
            Console.Write("\n--------------------------------------------\n");
            Console.Write("Enter amount to transfer from {0} to {1}: $", accountNames[fromAccount], accountNames[toAccount]);
            double userAmountToTransfer;
            userAmountToTransfer = UserTransactionCashAmount(fromAccount, 0); // 0 being a 'transfer' transaction type.
            accountBalances[fromAccount] = accountBalances[fromAccount] - userAmountToTransfer;
            accountBalances[toAccount] = accountBalances[toAccount] + userAmountToTransfer;
            Console.WriteLine("Transfer was successful.");
            DisplayBalance(toAccount);
            DisplayBalance(fromAccount);
        }//end TransferAmount


        static void DisplayAccountsMenu(int optionSelected)
        {
            Console.Clear();
            string[] menuOptions = { "", "Check Balance", "Withdrawal", "Transfer" };
            Console.Write("\n--------------------------------------------\n\n\tAccounts\n\t============\n");
            Console.WriteLine("\n\t{0}", menuOptions[optionSelected]);
            Console.WriteLine("\t1. {0}\n\t2. {1}\n\t3. {2}\n\t4. {3}\n", accountNames[1], accountNames[2], accountNames[3], accountNames[4]);
            if (optionSelected == 3)
            {
                Console.WriteLine("\nPlease select account to transfer from.");
            }
            Console.Write("Please Enter Your Choice 1 ... 4 or 0 to exit: ");
        }//end DisplayAccountsMenu


        static double UserTransactionCashAmount(int whichAccount, int transactionType)
        {
            bool isRunning = true;
            double userCashInput = 0;
            while (isRunning == true)
            {
                bool isGoodNumber = double.TryParse(Console.ReadLine(), out userCashInput);
                if (isGoodNumber && (userCashInput == 0))
                { // If input is 0 stop running
                    isRunning = false;
                }
                else if (isGoodNumber && userCashInput > 0)
                {
                    if ((whichAccount == 3) && ((accountBalances[whichAccount] - userCashInput) < -2000))
                    {// Overdrawing the credit account
                        Console.Write("\nYou cannot be more than $2000 in debt on your credit account. \nPlease try another amount or 0 to cancel: ");
                    }
                    else if ((whichAccount == 3) && ((accountBalances[whichAccount] - userCashInput) >= -2000))
                    { // Credit Withdraw
                        isRunning = false;
                        return userCashInput;
                    }
                    else if ((userCashInput > accountBalances[whichAccount]))
                    { // Not enough in account for transaction
                        Console.Write("\nSorry, there is insufficent fund in the account. \nPlease try another amount or 0 to cancel:  ");
                    }
                    else if ((userCashInput <= accountBalances[whichAccount] && ((accountBalances[whichAccount] - userCashInput) < transactionType)))
                    { //Normal Account overdraw 
                        Console.Write("\nSorry, there is insufficent fund in the account. \nPlease try another amount or 0 to cancel: ");
                    }
                    else if ((userCashInput <= accountBalances[whichAccount] && ((accountBalances[whichAccount] - userCashInput) >= transactionType)))
                    { //Normal Account withdraw 
                        isRunning = false;
                        return userCashInput;
                    }
                }
                else
                {
                    Console.Write("\nSorry, that is not a valid amount. \nPlease try another amount or 0 to cancel: ", userCashInput);
                }
            }
            return 0;
        }//end UserInputAccountsMenu


        static void UserInputAccountsMenu(out int userInput)
        {
            bool isRunning = true;
            userInput = 0;
            while (isRunning == true)
            {
                bool isGoodNumber = int.TryParse(Console.ReadLine(), out userInput);
                if (isGoodNumber && (userInput == 0 || userInput == 1 || userInput == 2 || userInput == 3 || userInput == 4))
                {
                    isRunning = false;
                }
                else
                {
                    Console.Write("Incorrect Selection. Please Try Again, Please select either: 0, 1, 2, 3, or 4: ");
                }
            }
        }//end UserInputAccountsMenu


        static bool UserInputYesNo(int mainMenuOptionSelected)
        {
            string[] accountAction = { "", "check the balance of", "withdraw from", "transfer to" };
            Console.Write("Would you like to {0} another account <Y or N> ? ", accountAction[mainMenuOptionSelected]);
            bool validInput = false;
            while (validInput == false)
            {
                string userChoice = Console.ReadLine();
                userChoice = userChoice.ToLower();
                switch (userChoice)
                {
                    case "y":
                        validInput = true;
                        Console.Clear();
                        DisplayAccountsMenu(mainMenuOptionSelected);
                        return true;
                    case "n":
                        validInput = true;
                        Console.Clear();
                        return false;
                    default:
                        Console.Write("Incorrect selection, Please Try Again: <Y or N> ");
                        break;
                }
            }
            return false;
        }


        static void ExitATM()
        {
            Console.Clear();
            Console.WriteLine("\n\tThank you for using Victor's Bank ATM, have a nice day!");
            Console.WriteLine("\n\n\tPress any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }//end ExitATM

    }
}