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
        private readonly string PAT;

        public PATAuthenticatedSession( Uri Domain, string PAT ) : base( new HttpClientHandler() { UseCookies = true } )
        {
            this.PAT = PAT;
            BaseAddress = Domain;
        }

        public async Task<bool> Authentication()
        {
            var Response = await AuthenticationResponse();
            if( Response.StatusCode != HttpStatusCode.OK )
                throw new AuthenticationException();
            return true;
        }

        public async Task<HttpResponseMessage> AuthenticationResponse()
            => await SendAsync(
                new HttpRequestMessage( HttpMethod.Post, BaseAddress )
                {
                    Content = new StringContent(
                        "{ 'privatekey':'"+PAT+ "' }", Encoding.UTF8, "application/json"
                    )
                }
            );

        public override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            HttpResponseMessage Response = await base.SendAsync( request, cancellationToken );
            if( Response.StatusCode == HttpStatusCode.Unauthorized )
            {
                await Authentication();
                Response = await base.SendAsync( request, cancellationToken );
            }
            return Response;
        }
    }
}
