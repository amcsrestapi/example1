using System;
using System.Collections.Generic;
using System.Text;

namespace AmcsRestApiExample1
{
    class ApiUri : UriBuilder
    {
        public ApiUri( string EnvironmentName ) : base( "https://"+EnvironmentName+"-svc-core.amcsplatform.com" )
        {
        }
        public ApiUri( Uri BaseUri ) : base( BaseUri )
        {
        }

        public static ApiUri WithDomainFrom( Uri BaseUri )
            => new ApiUri( BaseUri );

        public static ApiUri ForEnvironment( string EnvironmentName )
            => new ApiUri( EnvironmentName );

        public Uri AuthWithPat( string PAT )
        {
            this.Path = "/erp/api/AuthTokens";
            this.Query = "PrivateKey=" + PAT;
            return this.Uri;
        }

        public Uri ResourcesFrom( string EndPoint )
        {
            this.Path = "/erp/api/integrator/erp/" + EndPoint;
            this.Query = "";
            return this.Uri;
        }
    }
}
