using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MarketplaceWebServiceProducts.Model;

namespace AmazonMWS1
{
    public class RetLowestPrice
    {
        public String asin_key { get; set; }
        public Double new_price { get; set; }
        public Double used_price { get; set; }
    }
    
    public class GetLowestOfferListingForAsin
    {
        private static string title = null;
        private static string binding = null;
        private static string productGroup = null;
        private static string smallImageUrl = null;
        private static string height = null;
        private static string length = null;
        private static string width = null;
        private static string weight = null;
        private static string ASIN = null;
        private static string salesRank = null;
        private static string totalCount = null;
        private static string tradeInPrice = null;
        private static string merchantId = null;
        private static string marketplaceId = null;

        private static string[] newListingPrice = new string[3];
        private static string[] newShippingPrice = new string[3];
        private static string[] newConditions = new string[3];
        private static string[] newSubConditions = new string[3];
        private static string[] usedListingPrice = new string[3];
        private static string[] usedShippingPrice = new string[3];
        private static string[] usedConditions = new string[3];
        private static string[] usedSubConditions = new string[3];

        private MarketplaceWebServiceProductsConfig config = null;
        private MarketplaceWebServiceProductsClient service = null;

        public GetLowestOfferListingForAsin(string accessKeyId, string secretAccessKey, string merchantId,
                                  string marketplaceId, string applicationName, string applicationVersion)
        {
            config = new MarketplaceWebServiceProductsConfig();

            config.ServiceURL = "https://mws.amazonservices.com/Products/2011-10-01";

            GetLowestOfferListingForAsin.merchantId = merchantId;
            GetLowestOfferListingForAsin.marketplaceId = marketplaceId;

            service = new MarketplaceWebServiceProductsClient(
                applicationName, applicationVersion, accessKeyId, secretAccessKey, config);
        }

        public string[] getUsedSubConditions()
        {
            return usedSubConditions;
        }

        public string[] getUsedConditiions()
        {
            return usedConditions;
        }

        public string[] getUsedShippingPrice()
        {
            return usedShippingPrice;
        }

        public string[] getUsedListingPrice()
        {
            return usedListingPrice;
        }

        public string[] getNewSubConditions()
        {
            return newSubConditions;
        }

        public string[] getNewCoditions()
        {
            return newConditions;
        }

        public string[] getNewShippintPrice()
        {
            return newShippingPrice;
        }

        public string[] getNewListingPrice()
        {
            return newListingPrice;
        }

        public string getTradeInPrice()
        {
            return tradeInPrice;
        }

        public string getTotalCount()
        {
            return totalCount;
        }

        public string getSalesRank()
        {
            return salesRank;
        }

        public string getASIN()
        {
            return ASIN;
        }

        public string getWeight()
        {
            return weight;
        }

        public string getWidth()
        {
            return width;
        }

        public string getLength()
        {
            return length;
        }

        public string getHeight()
        {
            return height;
        }

        public string getSmallImageUrl()
        {
            return smallImageUrl;
        }

        public string getProductGroup()
        {
            return productGroup;
        }

        public string getBinding()
        {
            return binding;
        }

        public string getTitle()
        {
            return title;
        }

        public List<RetLowestPrice> GetLowestPrice(List<String> varAsin)
        {
            GetLowestOfferListingsForASINRequest request = new GetLowestOfferListingsForASINRequest();
            GetLowestOfferListingsForASINRequest request2 = new GetLowestOfferListingsForASINRequest();

            List<RetLowestPrice> ObjList = new List<RetLowestPrice>();
            RetLowestPrice Obj = null;

            request2.ASINList = new ASINListType();
            foreach (string asi in varAsin)
            { request2.ASINList.ASIN.Add(asi); }

            request2.MarketplaceId = marketplaceId;
            request2.SellerId = merchantId;

            double uselisting = 0;
            double uselanded = 0;

            double newlanded = 0;
            double newlisting = 0;

            try
            {
                GetLowestOfferListingsForASINResponse response = service.GetLowestOfferListingsForASIN(request2);

                List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList =
                    response.GetLowestOfferListingsForASINResult;
                foreach (
                    GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in
                        getLowestOfferListingsForASINResultList)
                {
                    Obj = new RetLowestPrice();
                    if (getLowestOfferListingsForASINResult.IsSetProduct())
                    {
                        Product product = getLowestOfferListingsForASINResult.Product;
                        //getLowestOfferListingsForASINResult.ASIN
                        //Listing Price, shipping amount, conditions, and subconditions for new, used, all for the LOWEST 3.
                        Console.WriteLine("ASIN: {0}\n", getLowestOfferListingsForASINResult.ASIN);

                        if (product.IsSetLowestOfferListings())
                        {
                            LowestOfferListingList lowestOfferListings = product.LowestOfferListings;
                            List<LowestOfferListingType> lowestOfferListingList =
                                lowestOfferListings.LowestOfferListing;

                            uselanded = 0;
                            uselisting = 0;
                            newlisting = 0;
                            newlanded = 0;
                            foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList)
                            {
                                if (lowestOfferListing.IsSetQualifiers())
                                {
                                    QualifiersType qualifiers = lowestOfferListing.Qualifiers;
                                    PriceType price = lowestOfferListing.Price;



                                    if (qualifiers.ItemCondition.Contains("Used"))
                                    {
                                        if (uselisting == 0)
                                        {
                                            uselisting = Convert.ToDouble(price.ListingPrice.Amount);
                                            Console.WriteLine("Used Listing Price: {0}\n", uselisting);
                                        }

                                        if (uselanded == 0)
                                        {
                                            uselanded = Convert.ToDouble(price.LandedPrice.Amount);
                                            Console.WriteLine(
                                                "Used Landed Price: {0}\n",
                                                uselanded);
                                        }

                                    }

                                    if (qualifiers.ItemCondition.Contains("New"))
                                    {
                                        if (newlisting == 0)
                                        {
                                            newlisting = Convert.ToDouble(price.ListingPrice.Amount);
                                            Console.WriteLine("New Listing Price: {0}\n", newlisting);
                                        }

                                        if (newlanded == 0)
                                        {
                                            newlanded = Convert.ToDouble(price.LandedPrice.Amount);
                                            Console.WriteLine("New Landed Price: {0}\n", newlanded);
                                        }
                                    }
                                }

                                if (uselanded != 0 && uselisting != 0 && newlanded != 0 && newlanded != 0)
                                {
                                    break;
                                }
                            }

                            Obj.asin_key = getLowestOfferListingsForASINResult.ASIN;
                            Obj.new_price = newlisting;
                            Obj.used_price = uselisting;
                            ObjList.Add(Obj);
                        }
                    }
                }

                return ObjList;

            }
            catch (MarketplaceWebServiceProductsException ex)
            {
                return null;
                //Console.WriteLine("Caught Exception: " + ex.Message);
                //Console.WriteLine("Response Status Code: " + ex.StatusCode);
                //Console.WriteLine("Error Code: " + ex.ErrorCode);
                //Console.WriteLine("Error Type: " + ex.ErrorType);
                //Console.WriteLine("Request ID: " + ex.RequestId);
                //Console.WriteLine("XML: " + ex.XML);
                //Console.WriteLine("ResponseHeaderMetadata: " + ex.ResponseHeaderMetadata);
            }
            
        }

        public static void InvokeGetLowestOfferListingsForASIN(MarketplaceWebServiceProducts service,
                                                               GetLowestOfferListingsForASINRequest request)
        {

            double uselisting = 0;
            double uselanded = 0;

            double newlanded = 0;
            double newlisting = 0;

            try
            {
                GetLowestOfferListingsForASINResponse response = service.GetLowestOfferListingsForASIN(request);

                List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList =
                    response.GetLowestOfferListingsForASINResult;
                foreach (
                    GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in
                        getLowestOfferListingsForASINResultList)
                {
                    if (getLowestOfferListingsForASINResult.IsSetProduct())
                    {
                        Product product = getLowestOfferListingsForASINResult.Product;
                        //getLowestOfferListingsForASINResult.ASIN
                        //Listing Price, shipping amount, conditions, and subconditions for new, used, all for the LOWEST 3.
                        Console.WriteLine("ASIN: {0}\n", getLowestOfferListingsForASINResult.ASIN);

                        if (product.IsSetLowestOfferListings())
                        {
                            LowestOfferListingList lowestOfferListings = product.LowestOfferListings;
                            List<LowestOfferListingType> lowestOfferListingList =
                                lowestOfferListings.LowestOfferListing;

                            uselanded = 0;
                            uselisting = 0;
                            newlisting = 0;
                            newlanded = 0;
                            foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList)
                            {
                                if (lowestOfferListing.IsSetQualifiers())
                                {
                                    QualifiersType qualifiers = lowestOfferListing.Qualifiers;
                                    PriceType price = lowestOfferListing.Price;



                                    if (qualifiers.ItemCondition.Contains("Used"))
                                    {
                                        if (uselisting == 0)
                                        {
                                            uselisting = Convert.ToDouble(price.ListingPrice.Amount);
                                            Console.WriteLine(
                                                  "Used Listing Price: {0}\n",
                                                  uselisting);
                                        }


                                        if (uselanded == 0)
                                        {
                                            uselanded = Convert.ToDouble(price.LandedPrice.Amount);
                                            Console.WriteLine(
                                                "Used Landed Price: {0}\n",
                                                uselanded);
                                        }

                                    }

                                    if (qualifiers.ItemCondition.Contains("New"))
                                    {
                                        if (newlisting == 0)
                                        {
                                            newlisting = Convert.ToDouble(price.ListingPrice.Amount);
                                            Console.WriteLine(
                                                  "New Listing Price: {0}\n",
                                                  newlisting);
                                        }

                                        if (newlanded == 0)
                                        {
                                            newlanded = Convert.ToDouble(price.LandedPrice.Amount);
                                            Console.WriteLine(
                                                "New Landed Price: {0}\n",
                                                newlanded);
                                        }
                                    }






                                    //if (qualifiers.IsSetItemCondition()){
                                    //    if (newConditionCount < 3){
                                    //        if (qualifiers.ItemCondition.Equals("New")){
                                    //            newConditions[newConditionCount] = qualifiers.ItemCondition;
                                    //            if (qualifiers.IsSetItemSubcondition()){
                                    //                newSubConditions[newConditionCount] =
                                    //                    qualifiers.ItemSubcondition;
                                    //            }

                                    //            if (price.IsSetListingPrice()){
                                    //                MoneyType listingPrice = price.ListingPrice;
                                    //                newListingPrice[newConditionCount] =
                                    //                    Convert.ToString(listingPrice.Amount);
                                    //            }
                                    //            if (price.IsSetShipping()){
                                    //                MoneyType shippingPrice = price.Shipping;
                                    //                newShippingPrice[newConditionCount] =
                                    //                    Convert.ToString(shippingPrice.Amount);
                                    //            }

                                    //            Console.WriteLine(
                                    //                "NewConditions: {0} NewSubCondition: {1} NewListingPrice {2} NewShippingPrice: {3}\n",
                                    //                newConditions[newConditionCount],
                                    //                newSubConditions[newConditionCount],
                                    //                newListingPrice[newConditionCount],
                                    //                newShippingPrice[newConditionCount]);
                                    //            newConditionCount++;
                                    //        }
                                    //    }
                                    //    if (usedConditionCount < 3){
                                    //        if (qualifiers.ItemCondition.Equals("Used")){
                                    //            usedConditions[usedConditionCount] = qualifiers.ItemCondition;
                                    //            if (qualifiers.IsSetItemSubcondition()){
                                    //                usedSubConditions[usedConditionCount] =
                                    //                    qualifiers.ItemSubcondition;
                                    //            }
                                    //            if (price.IsSetListingPrice()){
                                    //                MoneyType listingPrice = price.ListingPrice;
                                    //                usedListingPrice[usedConditionCount] =
                                    //                    Convert.ToString(listingPrice.Amount);
                                    //            }
                                    //            if (price.IsSetShipping()){
                                    //                MoneyType shippingPrice = price.Shipping;
                                    //                usedShippingPrice[usedConditionCount] =
                                    //                    Convert.ToString(shippingPrice.Amount);
                                    //            }
                                    //            Console.WriteLine(
                                    //                "UsedConditions: {0} UsedSubCondition: {1} UsedListingPrice {2} UsedShippingPrice: {3}\n",
                                    //                usedConditions[usedConditionCount],
                                    //                usedSubConditions[usedConditionCount],
                                    //                usedListingPrice[usedConditionCount],
                                    //                usedShippingPrice[usedConditionCount]);
                                    //            usedConditionCount++;
                                    //        }
                                    //    }
                                    //}
                                    //totalOfferCount++;
                                }

                                if (uselanded != 0 && uselisting != 0 && newlanded != 0 && newlanded != 0)
                                {
                                    break;
                                }


                            }
                        }
                    }
                }
                //totalCount = Convert.ToString(totalOfferCount);
                //Console.WriteLine("The Total Count: " + totalCount);
            }
            catch (MarketplaceWebServiceProductsException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
                Console.WriteLine("XML: " + ex.XML);
                Console.WriteLine("ResponseHeaderMetadata: " + ex.ResponseHeaderMetadata);
            }
        }

        //public static void InvokeGetMatchingProductForId(MarketplaceWebServiceProducts service,
        //                                                 GetMatchingProductForIdRequest request)
        //{
        //    try
        //    {
        //        GetMatchingProductForIdResponse response = service.GetMatchingProductForId(request);

        //        List<GetMatchingProductForIdResult> getMatchingProductForIdResultList =
        //            response.GetMatchingProductForIdResult;
        //        foreach (
        //            GetMatchingProductForIdResult getMatchingProductForIdResult in getMatchingProductForIdResultList)
        //        {
        //            ProductList products = getMatchingProductForIdResult.Products;
        //            List<Product> productList = products.Product;

        //            foreach (Product product in productList)
        //            {
        //                if (getMatchingProductForIdResult.IsSetProducts())
        //                {
        //                    if (product.IsSetAttributeSets())
        //                    {
        //                        //Binding, Format, ASIN, Height, Length, Width, Weight, Product Group, Small Image URL, Title, Sales Rank

        //                        foreach (var attribute in product.AttributeSets.Any)
        //                        {
        //                            string xml = ProductsUtil.FormatXml((System.Xml.XmlElement)attribute);

        //                            XElement element = XElement.Parse(xml);

        //                            XNamespace ns2 =
        //                                "http://mws.amazonservices.com/schema/Products/2011-10-01/default.xsd";
        //                            title = element.Element(ns2 + "Title").Value;
        //                            binding = element.Element(ns2 + "Binding").Value;
        //                            productGroup = element.Element(ns2 + "ProductGroup").Value;
        //                            smallImageUrl =
        //                                element.Element(ns2 + "SmallImage").Element(ns2 + "URL").Value;
        //                            height = element.Element(ns2 + "ItemDimensions").Element(ns2 + "Height").Value;
        //                            length = element.Element(ns2 + "ItemDimensions").Element(ns2 + "Length").Value;
        //                            width = element.Element(ns2 + "ItemDimensions").Element(ns2 + "Width").Value;
        //                            weight = element.Element(ns2 + "ItemDimensions").Element(ns2 + "Weight").Value;
        //                        }
        //                    }

        //                    if (product.IsSetIdentifiers())
        //                    {
        //                        IdentifierType identifier = product.Identifiers;
        //                        if (identifier.IsSetMarketplaceASIN())
        //                        {
        //                            ASINIdentifier marketplaceASIN = identifier.MarketplaceASIN;
        //                            if (marketplaceASIN.IsSetASIN())
        //                            {
        //                                ASIN = marketplaceASIN.ASIN;
        //                            }
        //                        }
        //                    }

        //                    if (product.IsSetSalesRankings())
        //                    {
        //                        int i = 0;
        //                        SalesRankList rankings = product.SalesRankings;
        //                        List<SalesRankType> salesRankList = rankings.SalesRank;
        //                        foreach (SalesRankType saleRankings in salesRankList)
        //                        {
        //                            for (; i < 1; i++)
        //                            {
        //                                if (saleRankings.IsSetRank())
        //                                {
        //                                    salesRank = Convert.ToString(saleRankings.Rank);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    Console.WriteLine("Height: {0} \nLength: {1} \nWidth: {2} " +
        //                                      "\nWeight: {3} \nBinding: {4} \nProduct Group: {5} " +
        //                                      "\nURL: {6} \nTitle: {7} \nASIN: {8} \nSale Rank: {9} \n",
        //                                      height, length, width, weight, binding, productGroup, smallImageUrl,
        //                                      title, ASIN, salesRank);
        //                }
        //            }
        //        }
        //    }
        //    catch (MarketplaceWebServiceProductsException ex)
        //    {
        //        Console.WriteLine("Caught Exception: " + ex.Message);
        //        Console.WriteLine("Response Status Code: " + ex.StatusCode);
        //        Console.WriteLine("Error Code: " + ex.ErrorCode);
        //        Console.WriteLine("Error Type: " + ex.ErrorType);
        //        Console.WriteLine("Request ID: " + ex.RequestId);
        //        Console.WriteLine("XML: " + ex.XML);
        //        Console.WriteLine("ResponseHeaderMetadata: " + ex.ResponseHeaderMetadata);
        //    }
        //}

        //public static void InvokeGetCompetitivePricingForASIN(MarketplaceWebServiceProducts service,
        //                                                      GetCompetitivePricingForASINRequest request)
        //{
        //    try
        //    {
        //        GetCompetitivePricingForASINResponse response = service.GetCompetitivePricingForASIN(request);

        //        List<GetCompetitivePricingForASINResult> getCompetitivePricingForASINResultList =
        //            response.GetCompetitivePricingForASINResult;
        //        foreach (
        //            GetCompetitivePricingForASINResult getCompetitivePricingForASINResult in
        //                getCompetitivePricingForASINResultList)
        //        {
        //            if (getCompetitivePricingForASINResult.IsSetProduct())
        //            {
        //                Product product = getCompetitivePricingForASINResult.Product;

        //                if (product.IsSetCompetitivePricing())
        //                {
        //                    CompetitivePricingType competitivePricing = product.CompetitivePricing;
        //                    if (competitivePricing.IsSetTradeInValue())
        //                    {
        //                        MoneyType tradeInValue = competitivePricing.TradeInValue;

        //                        if (tradeInValue.IsSetAmount())
        //                        {
        //                            tradeInPrice = Convert.ToString(tradeInValue.Amount);
        //                            Console.WriteLine("Tradin Value: " + Convert.ToString(tradeInValue.Amount));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (MarketplaceWebServiceProductsException ex)
        //    {
        //        Console.WriteLine("Caught Exception: " + ex.Message);
        //        Console.WriteLine("Response Status Code: " + ex.StatusCode);
        //        Console.WriteLine("Error Code: " + ex.ErrorCode);
        //        Console.WriteLine("Error Type: " + ex.ErrorType);
        //        Console.WriteLine("Request ID: " + ex.RequestId);
        //        Console.WriteLine("XML: " + ex.XML);
        //        Console.WriteLine("ResponseHeaderMetadata: " + ex.ResponseHeaderMetadata);
        //    }
        //}
    }
}