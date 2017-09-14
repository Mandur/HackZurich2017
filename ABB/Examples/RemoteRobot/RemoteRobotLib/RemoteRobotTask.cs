using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RemoteRobotLib
{
    public class RemoteRobotTask
    {
        readonly string _hostname;
        readonly HttpClient _client;
        readonly string _taskName;

        public RemoteRobotTask(string hostname, string taskName, HttpClient client)
        {
            _hostname = hostname;
            _taskName = taskName;
            _client = client;
        }

        /// <summary>
        /// Makes sure that no motion is already running and the the robot is ready for commands.
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            // The state is stored in this module on the robot controller.
            const string moduleName = "Remote";

            await WaitForBoolValue(moduleName, "bStart", false);
            await WaitForBoolValue(moduleName, "bRunning", false);
        }

        public async Task RunProcedure(string procedureName)
        {
            // The robot is waiting for us to tell it what to do.

            // The state is stored in this module on the robot controller.
            const string moduleName = "Remote";

            // First, let it know what procedure we want to run.
            await SetStringVariable(moduleName, "stName", $"\"{procedureName}\"");
            // Then, tell the robot to stop waiting and start.
            await SetBoolVariable(moduleName, "bStart", true);
            // Immediately after starting the robot will set the bStart boolean back to false.
            // By waiting we know it has started.
            await WaitForBoolValue(moduleName, "bStart", false);
            // When the robot is finished moving and is waiting for a new command it will set 
            // the bRunning variable back to false.
            await WaitForBoolValue(moduleName, "bRunning", false);
        }

        /// <summary>
        /// Set the program pointer to a routine.
        /// </summary>
        /// <param name="moduleName">The module containing the routine.</param>
        /// <param name="routineName">The name of the routine.</param>
        public async Task SetPPToRoutine(string moduleName, string routineName)
        {
            string url = $"http://{_hostname}/rw/rapid/tasks/{_taskName}/pcp?action=set-pp-routine";
            var parameters = new Dictionary<string, string>
            {
                {"module", moduleName },
                {"routine", routineName },
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            response.EnsureSuccessStatusCode();
        }

        async Task WaitForBoolValue(string moduleName, string name, bool value)
        {
            // Setting up subscriptions requires a websocket connection.
            // Let's use polling for now.
            while (await GetBoolVariable(moduleName, name) != value)
            {
                await Task.Delay(100);
            }
        }

        async Task<bool> GetBoolVariable(string moduleName, string name)
        {
            string urlString = $"http://{_hostname}/rw/rapid/symbol/data/RAPID/{_taskName}/{moduleName}/{name}?json=1";
            var response = await _client.GetAsync(urlString);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            string value = json["_embedded"]["_state"][0]["value"].ToString();
            return ParseBoolString(value);
        }

        async Task SetBoolVariable(string moduleName, string name, bool value)
        {
            await SetStringVariable(moduleName, name, GetBoolString(value));
        }

        async Task SetStringVariable(string moduleName, string name, string value)
        {
            var parameters = new Dictionary<string, string>
            {
                {"value", value },
            };
            var content = new FormUrlEncodedContent(parameters);
            string urlString = $"http://{_hostname}/rw/rapid/symbol/data/RAPID/{_taskName}/{moduleName}/{name}?action=set";
            var response = await _client.PostAsync(urlString, content);
            response.EnsureSuccessStatusCode();
        }
        string GetBoolString(bool value)
        {
            return value ? "TRUE" : "FALSE";
        }

        bool ParseBoolString(string value)
        {
            if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                throw new Exception("Unexpected bool value");
            }
        }
    }
}
