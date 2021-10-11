using System;
using AmcsRestApiExample1;

namespace AmcsRestApiExample1
{
    class Program
    {
        static void Main( string[] args )
        {
            try
            {
                Console.WriteLine( "INFO: AMCS REST API Example 1" );
                Console.WriteLine( "INFO: Logging onto the API with a PAT token" );

                var session = new PATAuthenticatedSession(
                    "rstapi-dev",
                    "6U57UJu7ghV0bsSpyc0rcET6U75QDAzm"
                    );

                var authTask = session.Authentication();
                authTask.Wait();

                Console.WriteLine( "INFO: Logged on" );

                var getTask = session.ResponseTo( ApiRequest.ResouresFrom( session.Domain, "directory/Customers/" ) );
                getTask.Wait();

                Console.WriteLine( "INFO: " + getTask.Result );

            }
            catch( Exception ex )
            {
                Console.Write( ex );
            }
        }
    }
}
