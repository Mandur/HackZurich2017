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

        const string ModuleName = "Remote";

        public RemoteRobotTask(string hostname, string taskName, HttpClient client)
        {
            _hostname = hostname;
            _taskName = taskName;
            _client = client;
        }

        public async Task Init()
        {
            await WaitForBoolValue("bStart", false);
            await WaitForBoolValue("bRunning", false);
        }

        public async Task RunProcedure(string procedureName)
        {
            await SetStringVariable("stName", $"\"{procedureName}\"");
            await SetBoolVariable("bStart", true);
            await WaitForBoolValue("bStart", false);
            await WaitForBoolValue("bRunning", false);
        }

        public async Task SetPPToRoutine(string moduleName, string routineName)
        {
            string url = $"http://{_hostname}/rw/rapid/tasks/{_taskName}/pcp?action=set-pp-routine";
            var parameters = new Dictionary<string, string>
            {
                {"module", moduleName },
                {"routine", routineName },
                //{"userlevel", "false"},
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await _client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            response.EnsureSuccessStatusCode();
        }

        async Task WaitForBoolValue(string name, bool value)
        {
            // Setting up subscriptions requires a websocket connection.
            // Let's use polling for now
            while (await GetBoolVariable(name) != value)
            {
                await Task.Delay(100);
            }
        }

        async Task<bool> GetBoolVariable(string name)
        {
            string urlString = $"http://{_hostname}/rw/rapid/symbol/data/RAPID/{_taskName}/{ModuleName}/{name}?json=1";
            var response = await _client.GetAsync(urlString);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            string value= json["_embedded"]["_state"][0]["value"].ToString();
            return ParseBoolString(value);
        }

        async Task SetBoolVariable(string name, bool value)
        {
            await SetStringVariable(name, GetBoolString(value));
        }

        async Task SetStringVariable(string name, string value)
        {
            var parameters = new Dictionary<string, string>
            {
                {"value", value },
            };
            var content = new FormUrlEncodedContent(parameters);
            string urlString = $"http://{_hostname}/rw/rapid/symbol/data/RAPID/{_taskName}/{ModuleName}/{name}?action=set";
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
