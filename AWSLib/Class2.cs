using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AWSLib
{
    public class AWSItemLookup
    {
        ///private const string MY_AWS_ACCESS_KEY_ID = "AKIAJ3SLV3HDPPAEKOLA";
        //private const string MY_AWS_SECRET_KEY = "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";
        private const string DESTINATION = "webservices.amazon.com";

        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2009-03-31";
        //private const string ITEM_ID = "B00005JM5E";

        public Boolean GetAWSItem(String AWSSellerID, String AWSAccessKey, String AWSSecretKey, String ASINNo, ref Double _price, ref Double _newprice, ref Double _usedprice,ref String _itemtitle,ref String _itemWEBUrL , ref  String ExLog)
        {
            String requestUrl = "";
            try
            {
                _price = 0.0;
                _newprice = 0.0;
                _usedprice = 0.0;
                _itemtitle = "";

                SignedRequestHelper helper = new SignedRequestHelper(AWSAccessKey, AWSSecretKey, DESTINATION);

                IDictionary<string, string> r1 = new Dictionary<string, String>();
                r1["Service"] = "AWSECommerceService";
                r1["Operation"] = "ItemLookup";
                r1["ItemId"] = "B018FK66TU";
                r1["ItemLookup.1.ItemId"] = "B00C6F60NI,B000FQISSU,B0009HI5KQ,B000O7864G,B0027UY8B8,B00DXLO086,B00KVS6YZQ,B00007M5HU,B001BN4WA4,B000002TSV";
                r1["ItemLookup.2.ItemId"] = "B00002CF5X,B0000541WJ,B00005NX1J,B00006BSGS,B00007IG1M,B000083C6T,B0000AYL45,B0000B1OC4,B0001I5592";
                r1["ResponseGroup"] = "ItemAttributes,Offers,OfferFull,OfferListings";
                r1["ItemLookup.Shared.IdType"] = "ASIN";
                r1["IdType"] = "ASIN";
                r1["AssociateTag"] = AWSSellerID;

                //r1["Service"] = "AWSECommerceService";
                //r1["Operation"] = "ItemLookup";
                //r1["ItemId"] = "B018FK66TU";
                //r1["ResponseGroup"] = "ItemAttributes,Offers,OfferFull,OfferListings";
                //r1["ItemLookup.Shared.IdType"] = "ASIN";
                //r1["IdType"] = "ASIN";
                //r1["AssociateTag"] = AWSSellerID;


                requestUrl = helper.Sign(r1);
                String urlx = "";
                WebRequest request = HttpWebRequest.Create(requestUrl);
                WebResponse response = request.GetResponse();

                XmlTextReader reader = new XmlTextReader(response.GetResponseStream());
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (reader.Read())
                {
                    string xxss = reader.Name;

                    if (reader.Name == "ItemLookupResponse" || reader.Name == "Items" || reader.Name == "Item" || reader.Name == "DetailPageURL")
                    {
                        if (reader.LocalName == "DetailPageURL")
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "DetailPageURL" && reader.IsStartElement() == true)
                            {
                                _itemWEBUrL = ProcessRewardNode(reader, "DetailPageURL");
                            }
                        }
                    }
                    else
                    {
                        //if (reader.LocalName == "DetailPageURL")
                        //{
                        //    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "DetailPageURL" && reader.IsStartElement() == true)
                        //    {
                        //        urlx = ProcessRewardNode(reader, "DetailPageURL");
                        //    }
                        //}

                        if (reader.Name == "ItemAttributes" || reader.Name == "OfferSummary")
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ItemAttributes" && reader.IsStartElement() == true)
                            {
                                _itemtitle = ProcessRewardNode(reader, "Title");
                            }
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "OfferSummary" && reader.IsStartElement() == true)
                            {
                                string xx = ProcessRewardNode(reader, "LowestNewPrice");
                                String[] aa = xx.Split('|');
                                if (aa.Length == 2)
                                {
                                    if (aa[0] == "-1" || aa[0] == "-2")
                                    {
                                        _newprice = 0.00;
                                    }
                                    else
                                    {
                                        _newprice = Convert.ToDouble(aa[0]);
                                    }

                                    if (aa[1] == "-1" || aa[1] == "-2")
                                    {
                                        _usedprice = 0.00;
                                    }
                                    else
                                    {
                                        _usedprice = Convert.ToDouble(aa[1]);
                                    }
                                }
                                else
                                {
                                    _newprice = 0.00;
                                    _usedprice = 0.00;
                                }
                            }

                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Offers" && reader.IsStartElement() == true)
                            {
                                string xx = ProcessRewardNode(reader, "Price");
                                if (xx == "-1" || xx == "-2")
                                    _price = 0.0;
                                else
                                    _price = Convert.ToDouble(xx);

                               // reader.Close();
                            }
                        }
                        else
                        {
                            //reader.Skip();
                        }
                    }
                }

                ExLog = requestUrl;
                return true;
            }
            catch (Exception ex)
            {
                _price = 0.0;
                _newprice = 0.0;
                _usedprice = 0.0;
                _itemtitle = "";
                ExLog = ex.Message.ToString() + "\n" + requestUrl;
                return false;
            }
        }

        public String ProcessRewardNode(XmlTextReader RewardReader, string tagName)
        {
            try
            {
                XmlDocument RewardXmlDoc = new XmlDocument();
                RewardXmlDoc.LoadXml(RewardReader.ReadOuterXml());

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(RewardXmlDoc.NameTable);
                nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01");

                string RetVal = "";
                string priceStr = "";

                if (tagName == "LowestNewPrice" || tagName == "LowestUsedPrice")
                {
                    try
                    {
                        XmlNode nd = RewardXmlDoc.SelectSingleNode("//rate:LowestNewPrice", nsmgr);

                        if (nd.ChildNodes.Count == 3)
                        {
                            priceStr = nd.ChildNodes[2].InnerText;
                        }
                        else
                        {
                            priceStr = "-2";
                        }
                    }
                    catch (Exception ex)
                    {
                        priceStr = "-1";
                    }

                    try
                    {
                        XmlNode nd = RewardXmlDoc.SelectSingleNode("//rate:LowestUsedPrice", nsmgr);

                        if (nd.ChildNodes.Count == 3)
                        {
                            priceStr = priceStr + "|" + nd.ChildNodes[2].InnerText;
                        }
                        else
                        {
                            priceStr = priceStr + "|-2";
                        }
                    }
                    catch (Exception ex)
                    {
                        priceStr = priceStr + "|-1";
                    }

                }
                else if (tagName == "Title")
                {
                    RetVal = RewardXmlDoc.SelectSingleNode("//rate:" + tagName, nsmgr).InnerText;
                }
                else if (tagName == "Price")
                {
                    // price
                    try
                    {
                        XmlNode nd = RewardXmlDoc.SelectSingleNode("//rate:Price", nsmgr);

                        if (nd.ChildNodes.Count == 3)
                        {
                            RetVal = nd.ChildNodes[2].InnerText;
                        }
                        else
                        {
                            RetVal = "-2";
                        }
                    }
                    catch (Exception ex)
                    {
                        RetVal = "-1";
                    }
                }
                else
                {
                    RetVal = RewardXmlDoc.SelectSingleNode("//rate:" + tagName, nsmgr).InnerText;
                }

                if (tagName == "Title" || tagName == "Price")
                {
                    return RetVal.Replace("$", "");
                }
                else if (tagName == "DetailPageURL")
                {
                    return RetVal;
                }
                else
                {
                    return priceStr.Replace("$", "");
                }
            }
            catch (Exception ex)
            {

                if (tagName == "Title" || tagName == "Price")
                {
                    return "-1";
                }
                else
                {
                    return "-1|-1";
                }
            }
        }


        //private static string FetchTitle(string url)
        //{
        //    try
        //    {
        //        WebRequest request = HttpWebRequest.Create(url);
        //        WebResponse response = request.GetResponse();
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(response.GetResponseStream());

        //        XmlNodeList xnList = doc.SelectNodes("ItemLookupResponse");
        //        foreach (XmlNode xn in xnList)
        //        {
        //            XmlNode anode = xn.SelectSingleNode("ANode");
        //            if (anode != null)
        //            {
        //                string id = anode["ID"].InnerText;
        //                string date = anode["Date"].InnerText;
        //                XmlNodeList CNodes = xn.SelectNodes("ANode/BNode/CNode");
        //                foreach (XmlNode node in CNodes)
        //                {
        //                    XmlNode example = node.SelectSingleNode("Example");
        //                    if (example != null)
        //                    {
        //                        string na = example["Name"].InnerText;
        //                        string no = example["NO"].InnerText;
        //                    }
        //                }
        //            }
        //        }



        //        XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
        //        if (errorMessageNodes != null && errorMessageNodes.Count > 0)
        //        {
        //            String message = errorMessageNodes.Item(0).InnerText;
        //            return "Error: " + message + " (but signature worked)";
        //        }

        //        XmlNode titleNode = doc.GetElementsByTagName("Title", NAMESPACE).Item(0);
        //        string title = titleNode.InnerText;
        //        return title;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Console.WriteLine("Caught Exception: " + e.Message);
        //        System.Console.WriteLine("Stack Trace: " + e.StackTrace);
        //    }

        //    return null;
        //}
    }
}
