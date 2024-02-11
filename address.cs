using System;

namespace auctionHouse
{
    public class address
    {
        // ***** ADDRESS CLASS ***** //

        // private address fields
        private int unitNo, streetNo, postcode;
        private string streetName, streetSuffix, city, state;

        // empty constructor
        public address()
        {

        }

        // ***** METHODS ***** //

        private void setCursor()
        {
            Console.WriteLine("> ");
            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(left + 2, top - 1);
        }
        
        // method to put the address's fields together into a string
        public string compileAddress()
        {
            if (this.unitNo == 0)
            {
                return $"{streetNo} {streetName} {streetSuffix}, {city} {state} {postcode}";
            }
            else
            {
                return $"{unitNo}/{streetNo} {streetName} {streetSuffix}, {city} {state} {postcode}";
            }
        }
        
        // method to get the unit number
        public void getUnitNo()
        {
            // get and verify the unit number
            bool validUnitNo = false;
            string unitNoString = "";
            int unitNo = -1;
            while (!validUnitNo)
            {
                Console.WriteLine("\nUnit number (0 = none):");
                setCursor();
                unitNoString = Console.ReadLine();
                if (int.TryParse(unitNoString, out unitNo) && unitNo >= 0)
                {
                    validUnitNo = true;
                }
                else
                {
                    Console.WriteLine("\tUnit number must be a non-negative integer.");
                }
            }
            this.unitNo = unitNo;
        }
        
        // method get the street address
        public void getStreetNo()
        {
            // get and verify the street number
            bool validStreetNo = false;
            string streetNoString = "";
            int streetNo = -1;
            while (!validStreetNo)
            {
                Console.WriteLine("\nStreet number:");
                setCursor();
                streetNoString = Console.ReadLine();
                if (int.TryParse(streetNoString, out streetNo) && streetNo > 0)
                {
                    validStreetNo = true;
                }
                else if (int.TryParse(streetNoString, out streetNo) && streetNo < 0)
                {
                    Console.WriteLine("\tStreet number must be a positive integer.");
                }
                else if (int.TryParse(streetNoString, out streetNo) && streetNo == 0)
                {
                    Console.WriteLine("\tStreet number must be greater than 0.");
                }
                else
                {
                    Console.WriteLine("\tStreet number must be a non-negative integer.");
                }
            }
            this.streetNo = streetNo;
        }
        
        // method to get the street name
        public void getStreetName()
        {
            // get and verify their street name
            bool validStreetName = false;
            string streetName = "";
            while (!validStreetName)
            {
                Console.WriteLine("\nStreet name:");
                setCursor();
                streetName = Console.ReadLine();
                if (!string.IsNullOrEmpty(streetName))
                {
                    validStreetName = true;
                }
                else
                {
                    Console.WriteLine("\tStreet name must not be blank.");
                }
            }
            this.streetName = streetName;
        }

        // method to get the street suffix
        public void getStreetSuffix()
        {
            // get and verify the street suffix
            bool validStreetSuffix = false;
            string streetSuffix = "";
            while (!validStreetSuffix)
            {
                Console.WriteLine("\nStreet suffix:");
                setCursor();
                streetSuffix = Console.ReadLine();
                if (!string.IsNullOrEmpty(streetSuffix))
                {
                    validStreetSuffix = true;
                }
                else
                {
                    Console.WriteLine("\tStreet suffix must not be blank.");
                }
            }
            this.streetSuffix = streetSuffix;
        }

        // method to get the city
        public void getCity()
        {
            // get and verify the city
            bool validCity = false;
            string city = "";
            while (!validCity)
            {
                Console.WriteLine("\nCity:");
                setCursor();
                city = Console.ReadLine();
                if (!string.IsNullOrEmpty(city))
                {
                    validCity = true;
                }
                else
                {
                    Console.WriteLine("\tCity must not be blank.");
                }
            }
            this.city = city;
        }

        // method to get the state
        public void getState()
        {
            // get and verify the state
            bool validState = false;
            string state = "";
            while (!validState)
            {
                Console.WriteLine("\nState (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                setCursor();
                state = Console.ReadLine().ToUpper();
                if (!string.IsNullOrEmpty(state) && (state == "ACT" || state == "NSW" || state == "NT" || state == "QLD" || state == "SA" ||
                        state == "TAS" || state == "VIC" || state == "WA"))
                {
                    validState = true;
                }
                else if (string.IsNullOrEmpty(state))
                {
                    Console.WriteLine("\tState must not be blank.");
                }
                else
                {
                    Console.WriteLine("\tState must be ACT, NSW, NT, QLD, SA, TAS, VIC, or WA");
                }
            }
            this.state = state;
        }

        // method to get the postcode
        public void getPostcode()
        {
            // get and verify the postcode
            bool validPostCode = false;
            string postCodeString = "";
            int postCode = 999;
            while (!validPostCode)
            {
                Console.WriteLine("\nPostcode (1000 .. 9999):");
                setCursor();
                postCodeString = Console.ReadLine();
                if (int.TryParse(postCodeString, out postCode) && postCode >= 1000 && postCode <= 9999)
                {
                    validPostCode = true;
                }
                else
                {
                    Console.WriteLine("\tPostcode must be between 1000 and 9999.");
                }
            }
            this.postcode = postCode;
        }
    }
}