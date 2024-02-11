using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace auctionHouse
{
    public class client
    {
        // ***** CLIENT CLASS ***** //
        
        // client's private fields
        private string name, email, password;

        // client's public properties
        public string HomeAddress;
        public bool LoggedInBefore;

        public string Name
        {
            get { return name; }
        }
        public string Email
        {
            get { return email; }
        }

        // empty client constructor
        public client()
        {

        }
        
        // client constructor for existing clients
        public client(string name, string email, string password, string HomeAddress, bool LoggedInBefore)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.HomeAddress = HomeAddress;
            this.LoggedInBefore = LoggedInBefore;
        }

        // client constructor for new clients
        public client(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }

        // ***** METHODS ***** //

        // method to set that little arrow thing in the demostration video
        private void setCursor()
        {
            Console.WriteLine("> ");
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(left + 2, top - 1);
        }

        // method to sort the products list as per criteria
        private List<product> sortProducts(List<product> input)
        {
            var sortedInput = input.OrderBy(t => t.Name).ThenBy(t => t.Description).ThenBy(t => t.Price);

            return sortedInput.ToList();
        }

        public void clientMenu()
        {
            // write out the client details header
            string clientDetails = $"\nPersonal Details for {Name}({Email})";
            Console.WriteLine(clientDetails);
            for (int i = 0; i <= clientDetails.Length - 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            // check if the client has logged in before
            // if not ask them for their address
            if (!LoggedInBefore)
            {
                Console.WriteLine("\nPlease provide your home address.");
                string address = getAddress();

                // finally, tell the user their address and update it in the system
                HomeAddress = address;

                Console.WriteLine($"\nAddress has been updated to {address}");

                // update the client's address in the text file
                LoggedInBefore = true;
                addOrUpdate();
            }

            // loop the client menu until the user chooses to sign out
            bool done = false;
            while (!done)
            {
                Console.WriteLine("\nClient Menu" +
                                  "\n-----------" +
                                  "\n(1) Advertise Product" +
                                  "\n(2) View My Product List" +
                                  "\n(3) Search For Advertised Products" +
                                  "\n(4) View Bids On My Products" +
                                  "\n(5) View My Purchased Items" +
                                  "\n(6) Log off" +
                                  "\n" +
                                  "\nPlease select an option between 1 and 6");
                setCursor();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        advertiseProduct();
                        break;
                    case "2":
                        displayProducts();
                        break;
                    case "3":
                        searchProducts();
                        break;
                    case "4":
                        showBids();
                        break;
                    case "5":
                        viewPurchasedProducts();
                        break;
                    case "6":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("\tInvalid selection");
                        break;
                }
            }
        }

        // method to add or update client information in the clients.txt file
        public void addOrUpdate()
        {
            // read all the lines of the client file and place them into an array
            string[] allLines = File.ReadAllLines("clients.txt");

            // counter to count the number of lines read
            int counter = 0;

            // go through the array and find the matching client
            foreach (string line in allLines)
            {
                string[] lineSplit = line.Split('\t');
                if (lineSplit[1] == email)
                {
                    break;
                }
                else
                {
                    counter++;
                }

            }

            string clientInfoFormat = $"{name}\t{email}\t{password}\t{HomeAddress}\t{LoggedInBefore}";

            // check if a client has been found or not
            if (counter == allLines.Length)
            {
                // a client has not been found, thus, it needs to be added to the list
                using StreamWriter clientsWriter = new StreamWriter("clients.txt", true);

                // add the client's information
                clientsWriter.WriteLine(clientInfoFormat);

                // close the streamreader
                clientsWriter.Close();
            }
            else
            {
                // a client has been found, it needs to be updated
                allLines[counter] = clientInfoFormat;

                // write all the lines back into the text file
                File.WriteAllLines("clients.txt", allLines);
            }
        }

        // method to advertise a product
        private void advertiseProduct()
        {
            // display the client information header
            string clientDetails = $"\nProduct Advertisement for {name} ({email})";
            Console.WriteLine(clientDetails);
            for (int i = 0; i <= clientDetails.Length - 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            // get and validate the product name
            bool validProductName = false;
            string productName = "";
            while (!validProductName)
            {
                Console.WriteLine("\nProduct name");
                setCursor();
                productName = Console.ReadLine();
                if (!string.IsNullOrEmpty(productName))
                {
                    validProductName = true;
                }
                else
                {
                    Console.WriteLine("\tInvalid product name");
                }
            }

            // get and validate the product description
            bool validProductDescription = false;
            string productDescription = "";
            while (!validProductDescription)
            {
                Console.WriteLine("\nProduct description");
                setCursor();
                productDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(productDescription) && productDescription != productName)
                {
                    validProductDescription = true;
                }
                else
                {
                    Console.WriteLine("\tInvalid product description");
                }
            }

            // get and validate the product price
            bool validProductPrice = false;
            string productPrice = "";
            while (!validProductPrice)
            {
                Console.WriteLine("\nProduct price ($d.cc)");
                setCursor();
                productPrice = Console.ReadLine();
                if (!string.IsNullOrEmpty(productPrice) && Regex.IsMatch(productPrice, "\\$[0-9]+\\.[0-9]+"))
                {
                    // remove the '$' from the price to parse it
                    productPrice = productPrice.Remove(0, 1);
                    validProductPrice = true;
                }
                else
                {
                    Console.WriteLine("\tA currency value is required, e.g. $54.95, $9.99, $2314.15.");
                }
            }

            // create a product object with the information
            product newProduct = new product(productName, productDescription, double.Parse(productPrice), email, name);

            // add the product to the system
            newProduct.addOrUpdate();
        }

        // method to show the client's currently advertised products
        private void displayProducts()
        {
            // display the header
            string clientDetails = $"\nProduct List for {name} ({email})";
            Console.WriteLine(clientDetails);
            for (int i = 0; i <= clientDetails.Length - 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            // start the streamreader
            using StreamReader productsReader = new StreamReader("products.txt");

            // create an empty list of products to add to
            List<product> productsList = new List<product>();

            // go through the products list and split each line
            while (!productsReader.EndOfStream)
            {
                // split the contents of the line into an array
                string line = productsReader.ReadLine();
                string[] lineSplit = line.Split('\t');

                // create a product object to manipulate
                product currentProduct = new product(lineSplit[0], lineSplit[1], double.Parse(lineSplit[2]), lineSplit[3], lineSplit[4], lineSplit[5], lineSplit[6],
                    double.Parse(lineSplit[7]), lineSplit[8], lineSplit[9], lineSplit[10]);

                // check if the sellerName and sellerEmail matches our loggedInClient
                if (currentProduct.SellerEmail == email)
                {
                    productsList.Add(currentProduct);
                }
            }

            // stop the StreamReader
            productsReader.Close();

            // if the list if empty, say that no products have been found
            if (!productsList.Any())
            {
                Console.WriteLine("You have no advertised products at the moment.");
            }
            else
            {
                int lineCount = 1;

                // sort the list
                productsList = sortProducts(productsList);
                
                // write the table header
                Console.WriteLine("\nItem #\tProduct name\tDescription\tList price" +
                    "\tBidder name\tBidder email\tBid amt");
                foreach (product item in productsList)
                {
                    Console.WriteLine($"{lineCount}\t{item.Name}\t{item.Description}\t${(item.Price).ToString("0.00")}" +
                        $"\t{item.HighestBidderName}\t{item.HighestBidderEmail}\t${item.Bid.ToString("0.00")}");
                    lineCount++;
                }
            }
        }

        // method to search for products
        private void searchProducts()
        {
            // loop until the user enters a valid search term
            bool validSearch = false;
            string searchTerm = "";
            while (!validSearch)
            {
                // ask the user for their search term
                Console.WriteLine("\nPlease supply a search phrase (ALL to see all products)");
                setCursor();
                searchTerm = Console.ReadLine();

                // check if the search term is valid
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    validSearch = true;
                }
                else
                {
                    Console.WriteLine("\tSearch term cannot be blank.");
                }
            }

            // start the streamreader
            using StreamReader productSearch = new StreamReader("products.txt");

            // empty list to store all the found products
            List<product> foundProducts = new List<product>();

            // find matching products depending on the search term
            if (searchTerm == "ALL")
            {
                while (!productSearch.EndOfStream)
                {
                    // read each line and split
                    string line = productSearch.ReadLine();
                    string[] lineSplit = line.Split('\t');

                    // create a product object to manipulate
                    product currentProduct = new product(lineSplit[0], lineSplit[1], double.Parse(lineSplit[2]), lineSplit[3], lineSplit[4], lineSplit[5], lineSplit[6],
                        double.Parse(lineSplit[7]), lineSplit[8], lineSplit[9], lineSplit[10]);

                    // check if the user doesn't sell this product
                    if (currentProduct.SellerEmail != email)
                    {
                        foundProducts.Add(currentProduct);
                    }

                }
            }
            else
            {
                while (!productSearch.EndOfStream)
                {
                    // read each line and split
                    string line = productSearch.ReadLine();
                    string[] lineSplit = line.Split('\t');

                    // create a product object to manipulate
                    product currentProduct = new product(lineSplit[0], lineSplit[1], double.Parse(lineSplit[2]), lineSplit[3], lineSplit[4], lineSplit[5], lineSplit[6],
                        double.Parse(lineSplit[7]), lineSplit[8], lineSplit[9], lineSplit[10]);

                    if (currentProduct.SellerEmail != email)
                    {
                        if (currentProduct.Name.Contains(searchTerm) || currentProduct.Description.Contains(searchTerm))
                        {
                            foundProducts.Add(currentProduct);
                        }
                    }
                }
            }

            // close the StreamReader
            productSearch.Close();

            // if the list is empty then display a message
            if (!foundProducts.Any())
            {
                Console.WriteLine("\tNo products found");
            }
            else
            {
                Console.WriteLine("\nSearch results" +
                                  "\n--------------");

                // write the table header
                Console.WriteLine("\nItem #\tProduct name\tDescription\tList price" +
                    "\tBidder name\tBidder email\tBid amt");

                // integer to count the number of lines read
                int lineCount = 1;

                foundProducts = sortProducts(foundProducts);

                // display the results
                foreach (product product in foundProducts)
                {
                    Console.WriteLine(lineCount + "\t" + $"{product.Name}\t{product.Description}\t${product.Price.ToString("0.00")}" +
                        $"\t{product.HighestBidderName}\t{product.HighestBidderEmail}\t${product.Bid.ToString("0.00")}");
                        lineCount++;
                }

                // ask the user if they would like to place a bid
                bool validAnswer = false;
                string answer = "";
                while (!validAnswer)
                {
                    Console.WriteLine("\nWould you like to place a bid on any of these items (yes or no)?");
                    setCursor();
                    answer = Console.ReadLine();
                    if (answer.ToLower() == "yes" || answer.ToLower() == "no")
                    {
                        validAnswer = true;
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid input.");
                    }
                }

                if (answer == "yes")
                {
                    placeBid(foundProducts);
                }
            }
        }

        // method for the client to place a bid
        private void placeBid(List<product> foundProducts)
        {
            // loop until the user selects a valid product
            bool validNumber = false;
            string selection = "";
            int selectionInt = -1;
            while (!validNumber)
            {
                Console.WriteLine($"\nPlease enter a non-negative integer between 1 and {(foundProducts.ToArray()).Length}:");
                setCursor();
                selection = Console.ReadLine();
                // check if the user has entered a valid integer
                if (int.TryParse(selection, out selectionInt))
                {
                    if (selectionInt >= 1 && selectionInt <= (foundProducts.ToArray()).Length)
                    {
                        validNumber = true;
                    }
                    else
                    {
                        Console.WriteLine("\tSelection out of range.");
                    }
                }
                else
                {
                    Console.WriteLine("\tSelection must be an integer.");
                }
            }

            // create a new product object from the selected products list
            product selectedProduct = foundProducts[selectionInt - 1];

            // display bid information
            Console.WriteLine($"\nBidding for {selectedProduct.Name} (regular price ${selectedProduct.Price.ToString("0.00")}), current highest bid " +
                $"${selectedProduct.Bid.ToString("0.00")}");

            // loop until the user enters a valid bid
            bool validBid = false;
            string bid = "";
            while (!validBid)
            {
                Console.WriteLine("\nHow much do you bid?");
                setCursor();
                bid = Console.ReadLine();

                if (Regex.IsMatch(bid, "\\$[0-9]+\\.[0-9]+"))
                {
                    // remove the '$' from the bid to parse it
                    bid = bid.Remove(0, 1);
                    if (double.Parse(bid) > selectedProduct.Bid)
                    {
                        selectedProduct.HighestBidderName = name;
                        selectedProduct.HighestBidderEmail = email;
                        selectedProduct.Bid = double.Parse(bid);
                        validBid = true;
                    }
                    else
                    {
                        Console.WriteLine($"\tBid amount must be greater than ${selectedProduct.Bid.ToString("0.00")}");
                    }
                }
                else
                {
                    Console.WriteLine("\tInvalid bid: please enter your bid in the format $d.cc");
                }
            }

            // inform the user their bid is successful
            Console.WriteLine($"\nYour bid of ${bid} for {selectedProduct.Name} is placed.");

            // update the product's information
            selectedProduct.addOrUpdate();

            // ask for delivery options
            delivery(selectedProduct);
        }

        // method for the client to select delivery or pick-up
        private void delivery(product selectedProduct)
        {
            Console.WriteLine("\nDelivery Instructions\n" +
                                "---------------------\n" +
                                "(1) Click and collect\n" +
                                "(2) Home Delivery");

            bool validSelection = false;
            string selection = "";
            while (!validSelection)
            {
                Console.WriteLine("\nPlease select an option between 1 and 2");
                setCursor();
                selection = Console.ReadLine();

                if (selection == "1" || selection == "2")
                {
                    validSelection = true;
                }
                else
                {
                    Console.WriteLine("\tSelection out of range");
                }
            }

            if (selection == "1")
            {
                bool validStartDate = false;
                bool validEndDate = false;
                DateTime startDelivery = new DateTime();
                DateTime endDelivery = new DateTime();
                string startString = "";
                string endString = "";
                // loop until the user enters a valid start date
                while (!validStartDate)
                {
                    Console.WriteLine("\nDelivery window start (dd/MM/yyyy HH:mm)");
                    setCursor();
                    startString = Console.ReadLine();

                    
                    if (DateTime.TryParse(startString, out startDelivery))
                    {
                        DateTime today = DateTime.Now;
                        if ((startDelivery.Date == today.Date && startDelivery.Hour >= today.Hour +1) ||
                            (startDelivery.Date > today.Date))
                        {
                            validStartDate = true;
                        }
                        else
                        {
                            Console.WriteLine("\tDelivery day must be today or later.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tInput is not a valid DateTime");
                    }
                }

                // loop until the user enters a valid end date
                while (!validEndDate)
                {
                    Console.WriteLine("\nDelivery window end (dd/mm/yyyy hh:mm)");
                    setCursor();
                    endString = Console.ReadLine();

                    if (DateTime.TryParse(endString, out endDelivery))
                    {
                        if ((endDelivery.Date == startDelivery.Date && endDelivery.Hour >= startDelivery.Hour + 1) || 
                            (endDelivery.Date > startDelivery.Date))
                        {
                            validEndDate = true;
                        }                            
                        else
                        {
                            Console.WriteLine("\tDelivery end must be on or after the start delivery day.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tInput is not a valid DateTime.");
                    }
                }


                // inform the user their preferences have been recorded
                Console.WriteLine($"\nThank you for your bid. If successful, the item will be provided " +
                    $"via collection between {startString} and {endString}");

                // update the product information
                selectedProduct.PickupStart = startString.ToString();
                selectedProduct.PickupEnd = endString.ToString();
                // clear the delivery address as pickup is the current delivery option
                selectedProduct.DeliveryAddress = "-";
                selectedProduct.addOrUpdate();
            }
            else if (selection == "2")
            {
                Console.WriteLine("\nPlease provide your delivery address.");
                string address = getAddress();

                // finally, tell the user their delivery address and update it in the system
                Console.WriteLine($"\nThank you for your bid. If successful, the item " +
                        $"will be provided via delivery to {address}");
                selectedProduct.DeliveryAddress = $"{address}";

                // clear the pickup info
                selectedProduct.PickupStart = "-";
                selectedProduct.PickupEnd = "-";

                // update the product info
                selectedProduct.addOrUpdate();
            }
        }

        // method to show products for which bids have been placed
        private void showBids()
        {
            // start the streamreader
            using StreamReader productsReader = new StreamReader("products.txt");

            // create an empty products list
            List<product> productsList = new List<product>();

            while (!productsReader.EndOfStream)
            {
                // read each line and split it
                string line = productsReader.ReadLine();
                string[] lineSplit = line.Split('\t');

                // create an instance of the product for easy manipulation
                product currentProduct = new product(lineSplit[0], lineSplit[1], double.Parse(lineSplit[2]), lineSplit[3], lineSplit[4], lineSplit[5], lineSplit[6],
                        double.Parse(lineSplit[7]), lineSplit[8], lineSplit[9], lineSplit[10]);

                // check if the product is sold by the current client
                // and if it has a bid placed
                string productOwner = currentProduct.SellerEmail;
                double bid = double.Parse(lineSplit[7]);
                if (productOwner == email && bid > 0)
                {
                    productsList.Add(currentProduct);
                }
            }

            productsReader.Close();

            // write the header
            string clientDetails = $"\nList Product Bids for {name} ({email})";
            Console.WriteLine(clientDetails);
            for (int i = 0; i <= clientDetails.Length - 2; i++)
            {
                Console.Write("-");
            }

            // check if the products list is empty
            if (!productsList.Any())
            {
                Console.WriteLine("\nNo bids were found.");
            }
            // if not, display the products
            else
            {
                int counter = 1;

                productsList = sortProducts(productsList);
                
                Console.WriteLine("\nItem #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                foreach (product product in productsList)
                {
                    Console.WriteLine($"{counter}\t{product.Name}\t{product.Description}\t${product.Price.ToString("0.00")}\t{product.HighestBidderName}\t" +
                        $"{product.HighestBidderEmail}\t${product.Bid.ToString("0.00")}");
                    counter++;
                }

                // ask the user if they would like to place a bid
                bool validInput = false;
                while (!validInput)
                {
                    Console.WriteLine("\nWould you like to sell something (yes or no)?");
                    setCursor();
                    string sell = Console.ReadLine();
                    if (sell.ToLower() == "yes")
                    {
                        sellProduct(productsList);
                        validInput = true;
                    }
                    else if (sell.ToLower() == "no")
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid input.");
                    }
                }              
            }
        }

        // method to sell any products
        private void sellProduct(List<product> bidsList)
        {
            // ask the user on which product they'd like to sell
            bool validNumber = false;
            string selection = "";
            int selectionInt = 0;
            // loop until the make a valid selection
            while (!validNumber)
            {
                // ask the user to select a product
                Console.WriteLine($"\nPlease enter an integer between 1 and {(bidsList.ToArray()).Length}:");
                setCursor();
                selection = Console.ReadLine();

                if (int.TryParse(selection, out selectionInt))
                {
                    if (selectionInt > 0 && selectionInt <= (bidsList.ToArray()).Length)
                    {
                        validNumber = true;
                    }
                    else
                    {
                        Console.WriteLine("\tSelection out of range.");
                    }
                }
                else
                {
                    Console.WriteLine("\tSelection must be an integer.");
                }
            }

            // get the right product from the bidsList
            product soldProduct = bidsList[selectionInt - 1];

            // inform the user they have sold the product
            Console.WriteLine($"\nYou have sold {soldProduct.Name} to {soldProduct.HighestBidderName} for ${soldProduct.Bid}.");

            // remove the product from the list
            soldProduct.remove();
        }

        // method to view the client's purchased products
        private void viewPurchasedProducts()
        {
            // write the header
            string clientDetails = $"\nPurchased Items for {name} ({email})";
            Console.WriteLine(clientDetails);
            for (int i = 0; i <= clientDetails.Length - 2; i++)
            {
                Console.Write("-");
            }

            // start the streamreader
            using StreamReader productsReader = new StreamReader("purchasedProducts.txt");

            // create an empty list to view the client's products
            List<product> soldProducts = new List<product>();

            while (!productsReader.EndOfStream)
            {
                string line = productsReader.ReadLine();
                string[] lineSplit = line.Split('\t');

                product currentProduct = new product(lineSplit[0], lineSplit[1], double.Parse(lineSplit[2]), lineSplit[3], lineSplit[4], lineSplit[5], lineSplit[6],
                    double.Parse(lineSplit[7]), lineSplit[8], lineSplit[9], lineSplit[10]);

                
                if (currentProduct.HighestBidderEmail == email)
                {
                    soldProducts.Add(currentProduct);
                }
            }

            // close the streamreader
            productsReader.Close();

            // display the products if there are any
            if (!soldProducts.Any())
            {
                Console.WriteLine("\nYou have no purchased products at the moment.");
            }
            else
            {
                soldProducts = sortProducts(soldProducts);
                
                Console.WriteLine("\nItem #\tSeller email\tProduct name\tDescription\tList price\tAmt paid\tDelivery option");

                int counter = 1;
                foreach (product product in soldProducts)
                {
                    string deliveryOption = "";
                    // check if there is a isn't delivery address
                    if (product.DeliveryAddress == "-")
                    {
                        deliveryOption = $"Pick up between {product.PickupStart} & {product.PickupEnd}";
                    }
                    else
                    {
                        deliveryOption = $"Deliver to {product.DeliveryAddress}";
                    }
                    Console.WriteLine($"\n{counter}\t{product.SellerEmail}\t{product.Name}\t{product.Description}\t" +
                        $"${product.Price.ToString("0.00")}\t${product.Bid.ToString("0.00")}\t{deliveryOption}");
                    counter++;
                }
            }
        }

        // method to get the client's address
        private string getAddress()
        {
            // create an empty address object to use
            address newAddress = new address();

            // update the address's information
            newAddress.getUnitNo();
            newAddress.getStreetNo();
            newAddress.getStreetName();
            newAddress.getStreetSuffix();
            newAddress.getCity();
            newAddress.getState();
            newAddress.getPostcode();

            // compile the address's information to return
            return newAddress.compileAddress();
        }
    }
}