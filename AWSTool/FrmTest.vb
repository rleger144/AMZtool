Imports System.Xml
Imports System.Net
Imports System.Data.SQLite

Public Class FrmTest
    Dim DESTINATION As String = "webservices.amazon.com"
    Dim objval As New Oozee
    Public DBconnStr As String = "Data Source=|DataDirectory|\awsdb.db; Version=3;"

    Private Sub FrmTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbOperation.FillGloabalSetting()
        GETAWS_EAN()

    End Sub


    Public Function GETAWS_EAN() As Boolean

        Dim _RequestURL As String = ""
        Dim helper As New SignedRequestHelper(GblAwsAccessKey, GblAwsSecretKey, DESTINATION)

        Dim r1 As IDictionary(Of String, String) = New Dictionary(Of String, [String])()
        r1("Service") = "AWSECommerceService"
        r1("Operation") = "ItemLookup"
        r1("ItemLookup.1.ItemId") = "0029521551983"
        'If _searchItems2.Length > 0 Then
        '    r1("ItemLookup.1.ItemId") = _searchItems1
        '    r1("ItemLookup.2.ItemId") = _searchItems2
        'Else
        '    r1("ItemLookup.1.ItemId") = _searchItems1
        'End If
        'r1("ResponseGroup") = "Full"
        r1("ResponseGroup") = "ItemAttributes,Offers,OfferFull,OfferListings,SalesRank"
        r1("ItemLookup.Shared.IdType") = "EAN"
        r1("IdType") = "EAN"
        r1("AssociateTag") = GblAwsSellerID


        _RequestURL = helper.Sign(r1)
        Dim urlx As [String] = ""
        Dim request As WebRequest = HttpWebRequest.Create(_RequestURL)
        Dim response As WebResponse = request.GetResponse()

        Dim reader As New XmlTextReader(response.GetResponseStream())
        reader.WhitespaceHandling = WhitespaceHandling.None

        Dim RewardXmlDoc As New XmlDocument()
        Dim isItemLookExist As Boolean = False

        While reader.Read()
            If reader.Name = "ItemLookupResponse" Then
                RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                reader.Close()
                isItemLookExist = True
            End If
        End While

        If isItemLookExist = True Then
            Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:Item", nsmgr)
            For Each node As XmlNode In nd
                Dim dd As String = node.InnerXml
                Dim _iAsin As XmlNode = node.SelectSingleNode("./rate:ASIN", nsmgr)
                Dim _iDetailURL As XmlNode = node.SelectSingleNode("./rate:DetailPageURL", nsmgr)
                Dim _iTitle As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:Title", nsmgr)
                Dim _iCategory As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:ProductGroup", nsmgr)
                Dim _iNewprice As XmlNode = node.SelectSingleNode("./rate:OfferSummary/rate:LowestNewPrice/rate:FormattedPrice", nsmgr)
                Dim _iUsedprice As XmlNode = node.SelectSingleNode("./rate:OfferSummary/rate:LowestUsedPrice/rate:FormattedPrice", nsmgr)
                Dim _iLivePrice As XmlNode = node.SelectSingleNode("./rate:Offers/rate:OfferListing/rate:Price/rate:FormattedPrice", nsmgr)
                Dim _iSaleRank As XmlNode = node.SelectSingleNode("./rate:SalesRank", nsmgr)
                Dim _iTradeinValue As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:TradeInValue/rate:FormattedPrice", nsmgr)
                Dim _iNumOfPages As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:NumberOfPages", nsmgr)
                Dim _iNumOfDisc As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:NumberOfDiscs", nsmgr)
                Dim _iUPC As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:UPC", nsmgr)

                Dim con As New SQLiteConnection(DBconnStr)


                Dim cmd As New SQLiteCommand()
                cmd.CommandText = "update db_products set product_name=@product_name,last_updated=@last_updated,live_price=@live_price," &
                    "live_newprice=@live_newprice,live_usedprice=@live_usedprice,product_url=@product_url,rank=@rank,trade_in_price=@trade_in_price,product_category=@product_category, " &
                    "live_newprice_pre=live_newprice,live_usedprice_pre=live_usedprice,num_of_pages=@num_of_pages,num_of_disc=@num_of_disc, " &
                    "upc = CASE upc WHEN '' THEN  @upc  ELSE upc END " &
                    " where product_asin=@product_asin"
                cmd.Connection = con
                cmd.CommandType = CommandType.Text

                ' glbErroLog.AppendLine("----------------")
                Dim newPrice As Double = 0
                Dim usedPrice As Double = 0
                Dim TradePrice As Double = 0
                Dim ebnprice As Double = 0
                Dim ebuprice As Double = 0
                Dim itmTitle As String = ""
                Dim itmCategory As String = ""
                Dim itmASIN As String = ""
                Dim itmURL As String = ""
                Dim numberOfPage As String = ""
                Dim numberOfDisc As String = ""
                Dim iupccode As String = ""


                If _iTitle Is Nothing Then
                    cmd.Parameters.AddWithValue("@product_name", "")
                Else
                    itmTitle = objval.ToString(_iTitle.InnerText)
                    cmd.Parameters.AddWithValue("@product_name", itmTitle)
                End If

                If _iUPC Is Nothing Then
                    cmd.Parameters.AddWithValue("@upc", "")
                Else
                    iupccode = objval.ToString(_iUPC.InnerText)
                    cmd.Parameters.AddWithValue("@upc", iupccode)
                End If

                If _iCategory Is Nothing Then
                    cmd.Parameters.AddWithValue("@product_category", "")
                Else
                    itmCategory = objval.ToString(_iCategory.InnerText)
                    cmd.Parameters.AddWithValue("@product_category", itmCategory)
                End If

                If _iSaleRank Is Nothing Then
                    cmd.Parameters.AddWithValue("@rank", "")
                Else
                    cmd.Parameters.AddWithValue("@rank", objval.ToString(_iSaleRank.InnerText))
                End If

                If IsNothing(_iLivePrice) Then
                    cmd.Parameters.AddWithValue("@live_price", 0)
                Else
                    cmd.Parameters.AddWithValue("@live_price", objval.ToDouble(_iLivePrice.InnerText.Replace("$", "")))
                End If

                If IsNothing(_iNewprice) Then
                    cmd.Parameters.AddWithValue("@live_newprice", 0.0)
                Else
                    newPrice = objval.ToDouble(_iNewprice.InnerText.Replace("$", ""))
                    cmd.Parameters.AddWithValue("@live_newprice", newPrice)
                End If

                If IsNothing(_iUsedprice) Then
                    cmd.Parameters.AddWithValue("@live_usedprice", 0.0)
                Else
                    usedPrice = objval.ToDouble(_iUsedprice.InnerText.Replace("$", ""))
                    cmd.Parameters.AddWithValue("@live_usedprice", usedPrice)
                End If

                If IsNothing(_iTradeinValue) Then
                    cmd.Parameters.AddWithValue("@trade_in_price", 0.0)
                Else
                    TradePrice = objval.ToDouble(_iTradeinValue.InnerText.Replace("$", ""))
                    cmd.Parameters.AddWithValue("@trade_in_price", TradePrice)
                End If

                If IsNothing(_iNumOfPages) Then
                    cmd.Parameters.AddWithValue("@num_of_pages", 0.0)
                Else
                    numberOfPage = objval.ToString(_iNumOfPages.InnerText)
                    cmd.Parameters.AddWithValue("@num_of_pages", numberOfPage)
                End If

                If IsNothing(_iNumOfDisc) Then
                    cmd.Parameters.AddWithValue("@num_of_disc", 0.0)
                Else
                    numberOfDisc = objval.ToString(_iNumOfDisc.InnerText)
                    cmd.Parameters.AddWithValue("@num_of_disc", numberOfDisc)
                End If

                If IsNothing(_iAsin) Then
                    cmd.Parameters.AddWithValue("@product_asin", "")
                Else
                    itmASIN = objval.ToString(_iAsin.InnerText)
                    cmd.Parameters.AddWithValue("@product_asin", itmASIN)
                End If

                If IsNothing(_iDetailURL) Then
                    cmd.Parameters.AddWithValue("@product_url", "")
                Else
                    itmURL = objval.ToString(_iDetailURL.InnerText)
                    cmd.Parameters.AddWithValue("@product_url", itmURL)
                End If


                con.Open()
                Try
                    Dim ddssdd As Integer = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Dim xsddd As Integer
                    xsddd = 0
                End Try
                cmd.Parameters.Clear()
                con.Close()
            Next

        End If

        Return True
    End Function

    Public Function GetAWSItem() As [Boolean]
        Dim requestUrl As [String] = ""
        Dim ExLog As [String] = ""
        Try

            '_price = 0.0
            '_newprice = 0.0
            '_usedprice = 0.0
            '_itemtitle = ""

            'Dim helper As New SignedRequestHelper(AWSAccessKey, AWSSecretKey, "webservices.amazon.com")

            'Dim r1 As IDictionary(Of String, String) = New Dictionary(Of String, [String])()
            'r1("Service") = "AWSECommerceService"
            'r1("Operation") = "ItemLookup"
            'r1("ItemId") = ASINNo
            ''r1("ItemLookup.1.ItemId") = "B00C6F60NI,B000FQISSU,B0009HI5KQ,B000O7864G,B0027UY8B8,B00DXLO086,B00KVS6YZQ,B00007M5HU,B001BN4WA4,B000002TSV"
            ''r1("ItemLookup.2.ItemId") = "B00002CF5X,B0000541WJ,B00005NX1J,B00006BSGS,B00007IG1M,B000083C6T,B0000AYL45,B0000B1OC4,B0001I5592"
            'r1("ResponseGroup") = "ItemAttributes,Offers,OfferFull,OfferListings"
            'r1("ItemLookup.Shared.IdType") = "ASIN"
            'r1("IdType") = "ASIN"
            'r1("AssociateTag") = AWSSellerID

            ''r1["Service"] = "AWSECommerceService";
            ''r1["Operation"] = "ItemLookup";
            'r1["ItemId"] = "B018FK66TU";
            'r1["ResponseGroup"] = "ItemAttributes,Offers,OfferFull,OfferListings";
            'r1["ItemLookup.Shared.IdType"] = "ASIN";
            'r1["IdType"] = "ASIN";
            'r1["AssociateTag"] = AWSSellerID;


            'requestUrl = helper.Sign(r1)
            'Dim urlx As [String] = ""
            'Dim request As WebRequest = HttpWebRequest.Create(requestUrl)
            'Dim response As WebResponse = request.GetResponse()

            Dim reader As New XmlTextReader("xmlbig.xml")
            reader.WhitespaceHandling = WhitespaceHandling.None

            Dim RewardXmlDoc As New XmlDocument()
            While reader.Read()
                If reader.Name = "ItemLookupResponse" Then
                    RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                    reader.Close()
                End If
            End While



            Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:Item", nsmgr)
            For Each node As XmlNode In nd

                Dim nd0 As XmlNode = node.SelectSingleNode("//rate:DetailPageURL", nsmgr)
                Dim nd1 As XmlNode = node.SelectSingleNode("//rate:ItemAttributes/rate:Title", nsmgr)
                Dim nd2 As XmlNode = node.SelectSingleNode("//rate:OfferSummary/rate:LowestNewPrice/rate:FormattedPrice", nsmgr)
                Dim nd3 As XmlNode = node.SelectSingleNode("//rate:OfferSummary/rate:LowestUsedPrice/rate:FormattedPrice", nsmgr)
                Dim nd4 As XmlNode = node.SelectSingleNode("//rate:Offers/rate:OfferListing/rate:Price/rate:FormattedPrice", nsmgr)

            Next






            ExLog = requestUrl
            'While reader.Read()
            '    Dim xxss As String = reader.Name
            '    If reader.Name = "Items" Then
            '        RewardXmlDoc.LoadXml(reader.ReadOuterXml())
            '        reader.Close()
            '    End If
            '    'If reader.Name = "ItemLookupResponse" OrElse reader.Name = "Items" OrElse reader.Name = "Item" Then
            '    '    If reader.LocalName = "Item" Then
            '    '        If reader.NodeType = XmlNodeType.Element AndAlso reader.LocalName = "Item" AndAlso reader.IsStartElement() = True Then
            '    '            Dim _itemWEBUrL As String = ProcessRewardNode2(reader, "DetailPageURL")
            '    '        End If
            '    '    End If

            '    'End If

            'End While

            'Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            'nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            'Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:Item", nsmgr)


            Return True
        Catch ex As Exception
            '_price = 0.0
            '_newprice = 0.0
            '_usedprice = 0.0
            '_itemtitle = ""
            ExLog = ex.Message.ToString() + vbLf + requestUrl
            Return False
        End Try
    End Function

    Public Function ProcessRewardNode2(RewardReader As XmlTextReader, tagName As String) As [String]
        Try
            Dim RewardXmlDoc As New XmlDocument()
            RewardXmlDoc.LoadXml(RewardReader.ReadOuterXml())

            Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            Dim RetVal As String = ""
            Dim priceStr As String = ""

            Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:DetailPageURL", nsmgr)
            Dim nd1 As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:ItemAttributes/rate:Title", nsmgr)
            Dim nd2 As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:OfferSummary/rate:LowestNewPrice/rate:FormattedPrice", nsmgr)
            Dim nd3 As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:OfferSummary/rate:LowestUsedPrice/rate:FormattedPrice", nsmgr)
            Dim nd4 As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:Offers/rate:OfferListing/rate:Price/rate:FormattedPrice", nsmgr)




            Dim ss As String
            ss = "sdf"


        Catch ex As Exception
            Return ""
        End Try
    End Function



    Public Function ProcessRewardNode(RewardReader As XmlTextReader, tagName As String) As [String]
        Try
            Dim RewardXmlDoc As New XmlDocument()
            RewardXmlDoc.LoadXml(RewardReader.ReadOuterXml())

            Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            Dim RetVal As String = ""
            Dim priceStr As String = ""

            If tagName = "LowestNewPrice" OrElse tagName = "LowestUsedPrice" Then
                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:LowestNewPrice", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        priceStr = nd.ChildNodes(2).InnerText
                    Else
                        priceStr = "-2"
                    End If
                Catch ex As Exception
                    priceStr = "-1"
                End Try

                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:LowestUsedPrice", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        priceStr = (priceStr & Convert.ToString("|")) + nd.ChildNodes(2).InnerText
                    Else
                        priceStr = priceStr & Convert.ToString("|-2")
                    End If
                Catch ex As Exception
                    priceStr = priceStr & Convert.ToString("|-1")

                End Try
            ElseIf tagName = "Title" Then
                RetVal = RewardXmlDoc.SelectSingleNode(Convert.ToString("//rate:") & tagName, nsmgr).InnerText
            ElseIf tagName = "Price" Then
                ' price
                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:Price", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        RetVal = nd.ChildNodes(2).InnerText
                    Else
                        RetVal = "-2"
                    End If
                Catch ex As Exception
                    RetVal = "-1"
                End Try
            Else
                RetVal = RewardXmlDoc.SelectSingleNode(Convert.ToString("//rate:") & tagName, nsmgr).InnerText
            End If

            If tagName = "Title" OrElse tagName = "Price" Then
                Return RetVal.Replace("$", "")
            ElseIf tagName = "DetailPageURL" Then
                Return RetVal
            Else
                Return priceStr.Replace("$", "")
            End If
        Catch ex As Exception

            If tagName = "Title" OrElse tagName = "Price" Then
                Return "-1"
            Else
                Return "-1|-1"
            End If
        End Try
    End Function


End Class