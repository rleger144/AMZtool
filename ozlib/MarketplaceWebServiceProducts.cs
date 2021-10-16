/******************************************************************************* 
 *  Copyright 2008-2012 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  Marketplace Web Service Products CSharp Library
 *  API Version: 2011-10-01
 * 
 */


using MarketplaceWebServiceProducts.Model;

namespace AmazonMWS1
{

    /// <summary>
    /// This is the Products API section of the Marketplace Web Service.
    /// 
    /// </summary>
    public interface MarketplaceWebServiceProducts
    {
                
        /// <summary>
        /// Get Matching Product 
        /// </summary>
        /// <param name="request">Get Matching Product  request</param>
        /// <returns>Get Matching Product  Response from the service</returns>
        /// <remarks>
        /// GetMatchingProduct will return the details (attributes) for the
        /// given ASIN.
        ///   
        /// </remarks>
        GetMatchingProductResponse GetMatchingProduct(GetMatchingProductRequest request);

                
        /// <summary>
        /// Get Lowest Offer Listings For ASIN 
        /// </summary>
        /// <param name="request">Get Lowest Offer Listings For ASIN  request</param>
        /// <returns>Get Lowest Offer Listings For ASIN  Response from the service</returns>
        /// <remarks>
        /// Gets some of the lowest prices based on the product identified by the given
        /// MarketplaceId and ASIN.
        ///   
        /// </remarks>
        GetLowestOfferListingsForASINResponse GetLowestOfferListingsForASIN(GetLowestOfferListingsForASINRequest request);
                
        /// <summary>
        /// Get Competitive Pricing For ASIN 
        /// </summary>
        /// <param name="request">Get Competitive Pricing For ASIN  request</param>
        /// <returns>Get Competitive Pricing For ASIN  Response from the service</returns>
        /// <remarks>
        /// Gets competitive pricing and related information for a product identified by
        /// the MarketplaceId and ASIN.
        ///   
        /// </remarks>
        GetCompetitivePricingForASINResponse GetCompetitivePricingForASIN(GetCompetitivePricingForASINRequest request);

        GetMatchingProductForIdResponse GetMatchingProductForId(GetMatchingProductForIdRequest request);
    }
}
