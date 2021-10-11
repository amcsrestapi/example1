using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Threading;

namespace AmcsRestApiExample1
{
    public class PATAuthenticatedSession
    {
        private readonly string PAT;
        private string AuthCookie;
        private HttpClient HttpClient;
        public Uri Domain {
            get => HttpClient.BaseAddress;
        }

        public PATAuthenticatedSession( string EnvironmentName, string PAT )
        {
            this.PAT = PAT;
            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new ApiUri( EnvironmentName ).Uri;
        }

        public async Task<bool> Authentication()
        {
            var Response = await AuthenticationResponse();
            if( Response.StatusCode != HttpStatusCode.OK )
                throw new AuthenticationException();
            foreach( var Cookie in Response.Content.Headers.GetValues("Set-Cookie") )
            {
                Console.WriteLine( Cookie );
            }
            return await Task.FromResult( true );
        }

        public async Task<HttpResponseMessage> AuthenticationResponse()
            => await HttpClient.SendAsync( ApiRequest.AuthWithPat( HttpClient.BaseAddress, this.PAT ) );

        public async Task<string> ResponseTo( HttpRequestMessage Request )
        {
            var Response = await HttpClient.SendAsync( Request );
            Response.EnsureSuccessStatusCode();
            string ResponseBody = await Response.Content.ReadAsStringAsync();
            return await Task.FromResult( ResponseBody );
        }

        public void LogCookies()
        {
        }
    }
}
