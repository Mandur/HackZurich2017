using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RemoteRobotLib
{
    public class RemoteYumi
    {
        readonly string _hostname;
        readonly HttpClient _client;

        public RemoteYumi(string hostname, HttpClient client)
        {
            _hostname = hostname;
            _client = client;
            LeftArm = new RemoteRobotTask(hostname, "T_ROB_L", client);
            RightArm = new RemoteRobotTask(hostname, "T_ROB_R", client);
        }

        public RemoteRobotTask LeftArm { get; }
        public RemoteRobotTask RightArm { get; }

        /// <summary>
        /// Runs the same procedure for both arms.
        /// </summary>
        /// <param name="procedureName">The name of the procdure.</param>
        public async Task RunProcedureForBothArms(string procedureName)
        {
            await Task.WhenAll(
                LeftArm.RunProcedure(procedureName),
                RightArm.RunProcedure(procedureName));
        }

        public async Task StartExecution()
        {
            string url = $"http://{_hostname}/rw/rapid/execution?action=start";
            var parameters = new Dictionary<string, string>
            {
                { "regain", "clear" },
                { "execmode", "continue" },
                { "cycle", "once" },
                { "condition", "none" },
                { "stopatbp", "enabled" },
                { "alltaskbytsp", "true" },
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            response.EnsureSuccessStatusCode();
        }

        public async Task Init()
        {
            await Task.WhenAll(
                LeftArm.Init(), 
                RightArm.Init());
        }
    }
}
