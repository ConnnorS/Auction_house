using System;
namespace auctionHouse
{
    public class soldProduct : product
    {
        // sold product constructor with information inherited from the product class
        public soldProduct(string name, string description, double price, string sellerName,
            string sellerEmail, string highestBidderName, double bid, string highestBidderEmail,
            string pickupStart, string pickupEnd, string deliveryAddress)

            : base (name, description, price, sellerEmail, sellerName, highestBidderEmail,
                  highestBidderName, bid, pickupStart, pickupEnd, deliveryAddress)
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

        // method to add the product to the soldProducts.txt file
        public override void addOrUpdate()
        {
            // start the StreamWriter
            using StreamWriter purchasedProductsWriter = new StreamWriter("purchasedProducts.txt", true);

            // write the product to the list
            purchasedProductsWriter.WriteLine($"{name}\t{description}\t{price}\t{sellerEmail}\t{sellerName}\t{HighestBidderEmail}\t{HighestBidderName}\t{Bid}\t{PickupStart}\t{PickupEnd}\t{DeliveryAddress}");

            // close the StreamWriter
            purchasedProductsWriter.Close();
        }
    }
}