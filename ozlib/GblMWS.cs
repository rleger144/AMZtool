using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonMWS1
{
   public static class GblMWS
    {

        public static String MWS_accessKeyId = "AKIAJUZLOKGNMX5QM7PQ";
        public static String MWS_secretAccessKey = "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";
        public static String MWS_merchantId = "ABF6QR8S79RK1";
        public static String MWS_marketplaceId = "ATVPDKIKX0DER";

        public static String MWS_ApplicationName = "";
        public static String MWS_applicationVersion = "1.0";

        
        public static string Gbl_MWS_accessKeyId
        {
            get { return MWS_accessKeyId; }
        }

        public static string Gbl_MWS_secretAccessKey
        {
            get { return MWS_secretAccessKey; }
        }

        public static string Gbl_MWS_merchantId
        {
            get { return MWS_merchantId; }
        }

        public static string Gbl_MWS_marketplaceId
        {
            get { return MWS_marketplaceId; }
        }

        public static void SetMWS_AccessKey(string value)
        {
            MWS_accessKeyId = value;
        }

        public static void SetMWS_SecretKey(string value)
        {
            MWS_secretAccessKey = value;
        }

        public static void SetMWS_MerchantId(string value)
        {
            MWS_merchantId = value;
        }

        public static void SetMWS_MarketplaceId(string value)
        {
            MWS_marketplaceId = value;
        }
    }
}
