using System;
using System.Text.RegularExpressions;

namespace auctionHouse
{
    public class mainMenu
    {

        // main menu constructor
        public mainMenu()
        {

        }

        // main loop
        public void welcome()
        {
            // initial setup
            // create the relevant text files in case they're not there
            using StreamWriter createClientsFile = new StreamWriter("clients.txt");
            using StreamWriter createProductsFile = new StreamWriter("products.txt");
            using StreamWriter createSoldProductsFile = new StreamWriter("purchasedProducts.txt");

            // close the created text files
            createClientsFile.Close();
            createProductsFile.Close();
            createSoldProductsFile.Close();

            // welcome the user
            Console.WriteLine(
                "+------------------------------+\r\n" +
                "| Welcome to the Auction House |\r\n" +
                "+------------------------------+\r");
        }

        // method to set that little arrow thing in the demostration video
        public void setCursor()
        {
            Console.WriteLine("> ");
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(left + 2, top - 1);
        }

        // method to register a client
        public void register()
        {
            Console.WriteLine("\nRegistration" +
                "\n------------");

            // get and verify the user's name
            bool validName = false;
            string clientName = "";
            while (!validName)
            {
                Console.WriteLine("\nPlease enter your name");
                setCursor();
                clientName = Console.ReadLine();
                validName = checkName(clientName);

                if (!validName)
                {
                    Console.WriteLine("\tThe supplied value is not a valid name.");
                }
            }

            // get and verify the user's email
            bool validEmail = false;
            bool emailExists = true;
            string clientEmail = "";
            while (!validEmail || emailExists)
            {
                Console.WriteLine("\nPlease enter your email address");
                setCursor();
                clientEmail = Console.ReadLine();
                validEmail = checkEmail(clientEmail);
                emailExists = emailAlreadyExists(clientEmail);

                if (!validEmail && !emailExists)
                {
                    Console.WriteLine("\tThe supplied value is not a valid email address.");
                }
                else if (validEmail && emailExists)
                {
                    Console.WriteLine("\tThe supplied address is already in use");
                }
            }

            // get and verify the user's password
            bool validPassword = false;
            string clientPassword = "";
            while (!validPassword)
            {
                Console.WriteLine(
                "\nPlease choose a password\n" +
                "* At least 8 characters\n" +
                "* No white space characters\n" +
                "* At least one upper-case letter\n" +
                "* At least one lower-case letter\n" +
                "* At least one digit\n" +
                "* At least one special character");
                setCursor();
                clientPassword = Console.ReadLine();
                validPassword = checkPassword(clientPassword);

                if (!validPassword)
                {
                    Console.WriteLine("\tThe supplied value is not a valid password");
                }
            }

            // create a new client object to manipulate
            client newClient = new client(clientName, clientEmail, clientPassword);

            // write the new client into the clients.txt file
            newClient.addOrUpdate();

            // tell the user they have been added to the system
            Console.WriteLine($"\nClient {newClient.Name}({newClient.Email}) has successfully registered" +
                $" at the Auction House.");
        }

        // method to check the client's name during registration
        private bool checkName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // method to check if the client's email already exists
        private bool emailAlreadyExists(string email)
        {
            // check if the email already exists
            bool alreadyExists = false;
            using StreamReader clientsReader = new StreamReader("clients.txt");
            while (!clientsReader.EndOfStream)
            {
                string line = clientsReader.ReadLine();
                string[] lineSplit = line.Split('\t');
                if (lineSplit[1] == email)
                {
                    alreadyExists = true;
                }
            }
            clientsReader.Close();

            return alreadyExists;
        }

        // method to check the client's email during registration
        private bool checkEmail(string email)
        {
            // boolean to store if the email meets the first set of requirements
            bool containsAt = email.Contains("@");
            bool atNotAtStart = !email.StartsWith("@");
            bool atNotAtEnd = !email.EndsWith("@");

            // booleans to check the parts of the email
            bool validFirstHalf = false;
            bool validSecondHalf = false;
            bool containsDot = false;
            bool validAfterDot = false;

            // split the email string at the "@"
            // if the placement of the "@" is correct
            if (containsAt && atNotAtEnd && atNotAtStart)
            {
                string[] emailSplit = email.Split('@');

                // check the first half of the email
                string firstHalf = emailSplit[0];
                char lastCharInFirstHalf = firstHalf[firstHalf.Length - 1];
                validFirstHalf = Regex.IsMatch(firstHalf, "^[a-zA-Z0-9_.-]+$") && lastCharInFirstHalf != '_' &&
                                       lastCharInFirstHalf != '-' && lastCharInFirstHalf != '.';

                // check the second half of the email
                string secondHalf = emailSplit[1];
                validSecondHalf = Regex.IsMatch(secondHalf, "^[a-zA-Z0-9.-]+$") && !(secondHalf.StartsWith(".")) &&
                                        !(secondHalf.EndsWith("."));
                containsDot = false;
                foreach (char i in secondHalf)
                {
                    if (i == '.')
                    {
                        containsDot = true;
                        break;
                    }
                }
                validAfterDot = false;
                if (containsDot)
                {
                    string[] secondHalfSplit = secondHalf.Split(".");
                    validAfterDot = Regex.IsMatch(secondHalfSplit[secondHalfSplit.Length - 1], "^[A-Za-z]+$");
                }
            }

            // determine if the email is valid and return the value
            if (containsAt && atNotAtStart && atNotAtEnd && validFirstHalf && validSecondHalf && containsDot && validAfterDot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // method to check the client's password during registration
        private bool checkPassword(string password)
        {
            // check if the password is at least 8 characters
            if (password.Length >= 8)
            {
                bool hasUpper = false;
                bool hasLower = false;
                bool hasDigit = false;
                bool hasNonAlpha = false;
                bool hasNoSpaces = true;
                foreach (char c in password)
                {
                    if (char.IsUpper(c))
                    {
                        hasUpper = true;
                    }
                    if (char.IsLower(c))
                    {
                        hasLower = true;
                    }
                    if (char.IsDigit(c))
                    {
                        hasDigit = true;
                    }
                    if (!char.IsLetterOrDigit(c))
                    {
                        hasNonAlpha = true;
                    }
                    if (char.IsWhiteSpace(c))
                    {
                        hasNoSpaces = false;
                    }
                }
                if (hasUpper && hasLower && hasDigit && hasNonAlpha && hasNoSpaces)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // method to sign in the client
        public client signIn()
        {

            Console.WriteLine("\nSign In" +
                "\n-------");
            
            // ask the user for their email
            Console.WriteLine("\nPlease enter your email address");
            setCursor();
            string logInEmail = Console.ReadLine();

            // ask the user for their password
            Console.WriteLine("\nPlease enter your password");
            setCursor();
            string logInPassword = Console.ReadLine();

            // read through the clients file to match the login info
            using StreamReader clientsReader = new StreamReader("clients.txt");

            // bool to track whether the user has been found
            bool foundUser = false;

            // empty client object to use later
            client signedInClient = new client();

            // read through the clients file to find the user
            while (!clientsReader.EndOfStream && !foundUser)
            {
                string line = clientsReader.ReadLine();
                string[] lineSplit = line.Split('\t');

                // check if the user's information is matched
                if (lineSplit[1] == logInEmail && lineSplit[2] == logInPassword)
                {
                    // edit our empty client object
                    signedInClient = new client(lineSplit[0], lineSplit[1], lineSplit[2], lineSplit[3], bool.Parse(lineSplit[4]));

                    // stop the loop
                    foundUser = true;
                    break;
                }
            }

            // close the StreamReader
            clientsReader.Close();

            // check if a client has been found
            if (foundUser)
            {
                return signedInClient;
            }
            else
            {
                Console.WriteLine("\tUser not found");
                return null;
            }
        }        
    }
}