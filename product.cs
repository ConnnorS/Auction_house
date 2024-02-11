using System;

namespace auctionHouse
{
    public class product
    {
        // ***** PRODUCT CLASS ***** //

        // product's private fields
        protected string name, description, sellerName, sellerEmail;
        protected double price;

        // product's public properties
        public string HighestBidderName, HighestBidderEmail, PickupStart, PickupEnd, DeliveryAddress;
        public double Bid;

        public string Name
        {
            get { return name; }
        }
        public string Description
        {
            get { return description; }
        }
        public double Price
        {
            get { return price; }
        }
        public string SellerEmail
        {
            get { return sellerEmail; }
        }
        public string SellerName
        {
            get { return sellerName; }
        }


        // product constructor when it is first added to the system
        public product(string name, string description, double price, string sellerEmail, string sellerName)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.sellerEmail = sellerEmail;
            this.sellerName = sellerName;
            HighestBidderName = "-";
            HighestBidderEmail = "-";
            Bid = 0;
            PickupStart = "-";
            PickupEnd = "-";
            DeliveryAddress = "-";
        }

        // product constructor for existing products
        public product(string name, string description, double price, string sellerEmail, string sellerName,
            string highestBidderEmail, string highestBidderName, double bid,
            string pickupStart, string pickupEnd, string deliveryAddress)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.sellerEmail = sellerEmail;
            this.sellerName = sellerName;
            this.Bid = bid;
            this.HighestBidderEmail = highestBidderEmail;
            this.HighestBidderName = highestBidderName;
            this.PickupStart = pickupStart;
            this.PickupEnd = pickupEnd;
            this.DeliveryAddress = deliveryAddress;
        }

        // ***** METHODS ***** //

        // method to add a new product to the system
        public virtual void addOrUpdate()
        {
            // read all the lines of the products file and place them into an array
            string[] allLines = File.ReadAllLines("products.txt");

            // counter to count the number of lines read through
            int counter = 0;

            // go through the array and find the matching product
            foreach (string line in allLines)
            {
                string[] lineSplit = line.Split('\t');
                if (lineSplit[0] == name && lineSplit[1] == description && double.Parse(lineSplit[2]) == price)
                {
                    break;
                }
                else
                {
                    counter++;
                }
            }

            string productInfoFormat = $"{name}\t{description}\t{price}\t{sellerEmail}\t{sellerName}\t{HighestBidderEmail}\t{HighestBidderName}\t{Bid}\t{PickupStart}\t{PickupEnd}\t{DeliveryAddress}";

            // check if a product has been found or not
            if (counter == allLines.Length)
            {
                // a product has not been found, thus, it needs to be added to the list
                // start the streamwriter
                using StreamWriter productsWriter = new StreamWriter("products.txt", true);

                // save the product information
                // using the format name, description, price, seller email, seller name, highest bidder name, highest bid, pickup start time, pickup end time, delivery address
                productsWriter.WriteLine(productInfoFormat);

                // close the streamwriter
                productsWriter.Close();

                // display a message confirming the addition of the item
                Console.WriteLine($"\nSuccessfully added product {name}, {description}, ${price.ToString("0.00")}.");
            }
            else
            {
                // a product has been found, thus, it needs to be updated
                // update the product info in the array
                allLines[counter] = productInfoFormat;

                // write all the lines back into the text file
                File.WriteAllLines("products.txt", allLines);
            }
        }

        // method to remove a product from the list
        // and add it to the sold products list
        public void remove()
        {
            // read all the lines of the products file and place them into an array
            string[] allLines = File.ReadAllLines("products.txt");

            // counter to count the number of lines read through
            int counter = 0;

            // go through the array and find the matching product
            foreach (string line in allLines)
            {
                string[] lineSplit = line.Split('\t');
                if (this.name == lineSplit[0] && this.description == lineSplit[1] && this.price == double.Parse(lineSplit[2]))
                {
                    break;
                }
                else
                {
                    counter++;
                }
            }

            // convert the array to list and remove the product
            List<string> products = allLines.ToList();
            products.Remove(products[counter]);

            // write all the lines back into the text file
            File.WriteAllLines("products.txt", products);

            // create a new soldProduct object
            soldProduct soldItem = new soldProduct(name, description, price, sellerName, sellerEmail,
                HighestBidderName, Bid, HighestBidderEmail, PickupStart, PickupEnd, DeliveryAddress);

            // add the sold product to the soldProducts.txt file
            soldItem.addOrUpdate();
        }
    }
}