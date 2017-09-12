using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteRobotLib;
using System.Net.Http;

namespace RemoteControlRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = "192.168.54.108";
            string hostname = "localhost";
            var httpClient = RobotClientProvider.GetHttpClientAsync(hostname).Result;
            var rightArm = new RemoteRobotTask(hostname, "T_ROB_R", httpClient);
            var leftArm = new RemoteRobotTask(hostname, "T_ROB_L", httpClient);

            leftArm.Init().Wait();
            rightArm.Init().Wait();

            Console.WriteLine("Resetting to home position.");

            Task.WhenAll(
                rightArm.RunProcedure("Home"),
                leftArm.RunProcedure("Home"))
                .Wait();
            
            Console.WriteLine("Executing gestures.");

            Task.WhenAll(
                rightArm.RunProcedure("NoClue"),
                leftArm.RunProcedure("NoClue"))
                .Wait();

            Console.WriteLine("Done.");

            Console.ReadKey();
        }
    }
}
