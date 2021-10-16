Module hdModule

    Public GblAwsSellerID As String = ""
    Public GblAwsAccessKey As String = ""
    Public GblAwsSecretKey As String = ""

    Public GblMWSSellerID As String = ""
    Public GblMWSAccessKey As String = ""
    Public GblMWSSecretKey As String = ""
    Public GblMWSMarketplaceID As String = ""

    Public GblRefMinu As Int32 = 0
    Public GblIsAlertMail As Boolean = False
    Public GblAlertMailIDs As String = ""
    Public GblebAppID As String = ""
    Public GblebDevID As String = ""
    Public GblebCerID As String = ""
    Public GblShowNotification As Boolean = False
    Public GblShowNotification_type As Int32 = 0
    Public GblApiCalAWSInterval As Int32 = 0
    Public GblApiCalMWSInterval As Int32 = 0
    ' Sender email globale ids 
    Public _GblSenderMailID As String = ""
    Public _GblSenderMailPwd As String = ""
    Public _GblSenderPortNo As String = ""
    Public _GblSenderSMTP As String = ""
    Public _GblSenderIsSSL As Boolean = False
    Public _GblSendMailonEachscan As Boolean = False
    Public GblKeepaAccessKey As String = ""

    Public GblIsProcessStart As Boolean = False
    Public GblIsKeepaOpen As Boolean = False
    Public GblIsScheularEnabled As Boolean = False
    Public GblScheularTime As String = ""
    Public GblEnabledEbay As Boolean = False

    Public GblEnabledEbayBrowserACTIVE As Boolean = False
    Public GblEnabledEbayBrowserCOMPLETE As Boolean = False


End Module
