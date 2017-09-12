using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RemoteRobotLib
{
    public static class RobotClientProvider
    {
        static Dictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

        /// <summary>
        /// Createas an http client for a robot controller or returns a cached one. 
        /// The client will be logged in using default credentials.
        /// </summary>
        /// <param name="robotHostname">Hostname for robot.</param>
        /// <returns>HttpClient ready for requests to the robot.</returns>
        public static async Task<HttpClient> GetHttpClientAsync(string robotHostname)
        {
            HttpClient client;
            if (!_clients.TryGetValue(robotHostname, out client))
            {
                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri($"http://{robotHostname}/rw"), "Digest",
                    new NetworkCredential("Default User", "robotics"));
                var httpClientHandler = new HttpClientHandler();
                client = new HttpClient(new HttpClientHandler { Credentials = credentialCache });
                _clients.Add(robotHostname, client);

                // Log in
                var res = await client.GetAsync($"http://{robotHostname}/");
                res.EnsureSuccessStatusCode();
            }
            return client;
        }
    }
}
