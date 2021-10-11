using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AmcsRestApiExample1
{
    class ApiRequest
    {
        public static HttpRequestMessage AuthWithPat( Uri WithDomainFrom, string PAT )
            => new HttpRequestMessage( HttpMethod.Post, ApiUri.WithDomainFrom( WithDomainFrom ).AuthWithPat( PAT ) );

        public static HttpRequestMessage ResouresFrom( Uri WithDomainFrom, string EndPoint )
            => new HttpRequestMessage( HttpMethod.Get, ApiUri.WithDomainFrom( WithDomainFrom ).ResourcesFrom( EndPoint ) );
    }
}
