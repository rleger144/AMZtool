using System;
using System.Collections.Generic;

namespace AmazonMWS1{

    public class MWSProductsMain{

        public static List<RetProductInfo> ProductInfo(List<String> _asinArr)
        {
            GetMatchingProduct mp = new GetMatchingProduct(GblMWS.MWS_accessKeyId, GblMWS.MWS_secretAccessKey, GblMWS.MWS_merchantId, GblMWS.MWS_marketplaceId, GblMWS.MWS_ApplicationName, GblMWS.MWS_applicationVersion);
            List<RetProductInfo> ObjList = mp.GetProductInformation(_asinArr);
            return ObjList;
        }


        public static List<RetLowestPrice> GetLowestPrice2(List<String> _asinArr)
       {
          // string[] asin = new string[] { "B008PDAF7C", "B00KBEY3B2", "B00AY4U4XU", "B001EOOC3W", "B004GYS2RE" };
           GetLowestOfferListingForAsin mp2 = new GetLowestOfferListingForAsin(GblMWS.MWS_accessKeyId, GblMWS.MWS_secretAccessKey, GblMWS.MWS_merchantId, GblMWS.MWS_marketplaceId, GblMWS.MWS_ApplicationName, GblMWS.MWS_applicationVersion);
           List<RetLowestPrice> ObjList = mp2.GetLowestPrice(_asinArr); 
           //Place your book ASIN here
           return ObjList;
       }

        //private static void Main(string[] args){

        //    /************************************************************************
        //     * Access Key ID and Secret Access Key ID
        //     ***********************************************************************/
        //    //String MWS_accessKeyId = "AKIAJUZLOKGNMX5QM7PQ";
        //    //String MWS_secretAccessKey = "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";
        //    //String MWS_merchantId = "ABF6QR8S79RK1";
        //    //String MWS_marketplaceId = "ATVPDKIKX0DER";

        //    /************************************************************************
        //     * The application name and version are included in each MWS call's
        //     * HTTP User-Agent field.
        //     ***********************************************************************/
        //    //const string applicationName = "";
        //    //const string applicationVersion = "1.0";


        //    /***********************************************************************
        //     * Amazon MWS access information will be passed to the class
        //     ***********************************************************************/
        //    //  GetMatchingProduct mp = new GetMatchingProduct(accessKeyId, secretAccessKey, merchantId, marketplaceId,applicationName,applicationVersion);
        //    //  mp.GetProductInformation("B00KBEY3B2");

        //    // product    ( asin array , ref
        //    //string[] asin = new string[] { "B008PDAF7C", "B00KBEY3B2", "B00AY4U4XU", "B001EOOC3W", "B004GYS2RE", "B00A1CK3S6", "B00J0LH044", "B006KA3510", "B000B6N6F4" };
        //    string[] asin = new string[] { "B008PDAF7C", "B00KBEY3B2", "B00AY4U4XU", "B001EOOC3W", "B004GYS2RE" };

        //   // GetLowestOfferListingForAsin mp = new GetLowestOfferListingForAsin(accessKeyId, secretAccessKey, merchantId, marketplaceId, applicationName, applicationVersion);
        //    // mp.GetLowestPrice(asin); //Place your book ASIN here
        //    GetMatchingProduct mp = new GetMatchingProduct(GblMWS.MWS_accessKeyId, GblMWS.MWS_secretAccessKey, GblMWS.MWS_merchantId, GblMWS.MWS_marketplaceId,GblMWS.MWS_ApplicationName ,GblMWS.MWS_applicationVersion);

        //    mp.GetProductInformation(asin);

        //    GetLowestOfferListingForAsin mp2 = new GetLowestOfferListingForAsin(GblMWS.MWS_accessKeyId, GblMWS.MWS_secretAccessKey, GblMWS.MWS_merchantId, GblMWS.MWS_marketplaceId, GblMWS.MWS_ApplicationName, GblMWS.MWS_applicationVersion);
        //    mp2.GetLowestPrice(asin); //Place your book ASIN here
            

        //    /***********************************************************************
        //     * Demo Stuff
        //     ***********************************************************************/
        //    Console.WriteLine();
        //    Console.WriteLine("Press Enter to Exit");
        //    Console.ReadLine();

        //}
    }
}