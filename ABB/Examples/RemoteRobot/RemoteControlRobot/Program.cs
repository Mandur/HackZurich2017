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
        // Service port
        const string hostname = "192.168.125.1";
        //const string hostname = "localhost";

        static void Main(string[] args)
        {
            try
            {
                //RunWithProgramPointer().Wait();
                RunWithRunLoop().Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static async Task RunWithProgramPointer()
        {
            // Using this method to get the HttpClient ensures that we are already logged in to the
            // robot when we try to send commands later.
            var httpClient = await RobotClientProvider.GetHttpClientAsync(hostname);
            // The RemoteYumi class represents the whole robot.
            var yumi = new RemoteYumi(hostname, httpClient);
            // We must set the program pointer to the routine we want to run for each arm.
            await yumi.LeftArm.SetPPToRoutine("Gestures","NoClue");
            await yumi.RightArm.SetPPToRoutine("Gestures", "NoClue");
            // To actually get the robot to move the execution must be started.
            await yumi.StartExecution();
        }

        static async Task RunWithRunLoop()
        {
            var httpClient = await RobotClientProvider.GetHttpClientAsync(hostname);
            var yumi = new RemoteYumi(hostname, httpClient);

            await yumi.Init();
            await yumi.RunProcedureForBothArms("NoClue");
        }

    }
}
