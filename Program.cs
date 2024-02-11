using System;
namespace auctionHouse
{
    public class Program
    {
        static void Main()
        {
            // create a new mainMenu object
            mainMenu mainLoop = new mainMenu();

            // run the welcome method
            mainLoop.welcome();

            // run the main menu
            bool done = false;

            while (!done)
            {
                // main menu
                Console.WriteLine("\nMain Menu\n" +
                                  "---------\n" +
                                  "(1) Register\n" +
                                  "(2) Sign In\n" +
                                  "(3) Exit\n" +
                                  "\nPlease select an option between 1 and 3");
                mainLoop.setCursor();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        mainLoop.register();                        
                        break;
                    case "2":
                        // run the sign in dialogue and receive the client object
                        client signedInClient = mainLoop.signIn();

                        // check if the client is not null (not incorrect login info)
                        if (signedInClient != null)
                        {
                            signedInClient.clientMenu();
                        }
                        break;
                    case "3":
                        Console.WriteLine(
                              "+--------------------------------------------------+" +
                            "\n| Good bye, thank you for using the Auction House! |" +
                            "\n+--------------------------------------------------+");
                        done = true;
                        break;
                    default:
                        Console.WriteLine("\tInvalid selection");
                        break;
                }
            }
        }
    }
}