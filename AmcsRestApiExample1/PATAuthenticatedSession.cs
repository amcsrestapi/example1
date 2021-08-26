using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Threading;

namespace AmcsRestApiExample1.AuthenticatedSession
{
    public class PATAuthenticatedSession : HttpClient
    {
        private const int TimeOutMins = 30;
        
        private readonly string PAT;
        private DateTime AuthTimeout;

        public bool IsAuthenticated => DateTime.Now < AuthTimeout;

        public PATAuthenticatedSession( Uri Domain, string PAT ) : base( new HttpClientHandler() { UseCookies = true } )
        {
            this.PAT = PAT;
            this.AuthTimeout = DateTime.Now;
            BaseAddress = Domain;
            Authenticate();
        }

        public async void Authenticate()
        {
            HttpResponseMessage Response = await SendAsync( 
                new HttpRequestMessage( HttpMethod.Post, BaseAddress ) {
                    Content = new StringContent(
                        "{ 'privatekey':'"+PAT+ "' }", Encoding.UTF8, "application/json"
                    )
                }
            );

            if( Response.StatusCode != HttpStatusCode.OK )
                throw new AuthenticationException();

            AuthTimeout = DateTime.Now.AddMinutes( TimeOutMins );
        }

        public override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            if( !IsAuthenticated )
                Authenticate();
            HttpResponseMessage Response = await base.SendAsync( request, cancellationToken );
            if( Response.StatusCode == HttpStatusCode.Unauthorized )
            {
                Authenticate();
                Response = await base.SendAsync( request, cancellationToken );
            }
            return Response;
        }
    }
}
