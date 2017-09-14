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
            try
            {
                //RunWithPcp().Wait();
                RunWithRunLoop().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static async Task RunWithPcp()
        {
            string hostname = "localhost";
            var httpClient = await RobotClientProvider.GetHttpClientAsync(hostname);
            var yumi = new RemoteYumi(hostname, httpClient);
            await yumi.LeftArm.SetPPToRoutine("Gestures","NoClue");
            await yumi.RightArm.SetPPToRoutine("Gestures", "NoClue");
            await yumi.StartExecution();
        }


        static async Task RunWithRunLoop()
        {
            string hostname = "localhost";
            var httpClient = await RobotClientProvider.GetHttpClientAsync(hostname);
            var yumi = new RemoteYumi(hostname, httpClient);

            await yumi.Init();

            Console.WriteLine("Resetting to home position.");
            await yumi.RunProcedureForBothArms("Home");
            Console.WriteLine("Executing gestures.");
            await yumi.RunProcedureForBothArms("NoClue");
            Console.WriteLine("Done.");
        }

        static async Task PrintExecutionActions()
        {
            string hostname = "localhost";
            var httpClient = await RobotClientProvider.GetHttpClientAsync(hostname);
            var yumi = new RemoteYumi(hostname, httpClient);
            await yumi.PrintExecutionActions();
        }

    }
}
