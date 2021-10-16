
Public Class Product
    Public Property csv As Int64()()
    Public Property categories As Int64()
    Public Property imagesCSV As String
    Public Property manufacturer As String
    Public Property title As String
    Public Property lastUpdate As Int64
    Public Property lastPriceChange As Int64
    Public Property rootCategory As Int64
    Public Property productType As Int64
    Public Property parentAsin As String
    Public Property variationCSV As Object
    Public Property asin As String
    Public Property domainId As Int64
    Public Property type As String
    Public Property hasReviews As Boolean
    Public Property ean As Long
    Public Property upc As Long
    Public Property mpn As String
    Public Property trackingSince As Int64
    Public Property brand As String
    Public Property label As String
    Public Property department As Object
    Public Property publisher As String
    Public Property productGroup As String
    Public Property partNumber As String
    Public Property studio As String
    Public Property genre As Object
    Public Property model As String
    Public Property color As String
    Public Property size As String
    Public Property edition As Object
    Public Property platform As Object
    Public Property format As Object
    Public Property packageHeight As Int64
    Public Property packageLength As Int64
    Public Property packageWidth As Int64
    Public Property packageWeight As Int64
    Public Property packageQuantity As Int64
    Public Property isAdultProduct As Boolean
    Public Property isEligibleForTradeIn As Boolean
    Public Property isEligibleForSuperSaverShipping As Boolean
    Public Property offers As Object
    Public Property buyBoxSellerIdHistory As Object
    Public Property isRedirectASIN As Boolean
    Public Property isSNS As Boolean
    Public Property author As Object
    Public Property binding As String
    Public Property numberOfItems As Int64
    Public Property numberOfPages As Int64
    Public Property publicationDate As Int64
    Public Property releaseDate As Int64
    Public Property languages As Object
    Public Property lastRatingUpdate As Int64
    Public Property eanList As String()
    Public Property upcList As String()
    Public Property liveOffersOrder As Int64()
    Public Property frequentlyBoughtTogether As String()
    Public Property stats As Object
    Public Property offersSuccessful As Boolean
    Public Property g As Int64
End Class

Public Class RootObject
    Public Property timestamp As Long
    Public Property tokensLeft As Int64
    Public Property refillIn As Int64
    Public Property refillRate As Int64
    Public Property tokenFlowReduction As Double
    Public Property products As Product()
End Class

