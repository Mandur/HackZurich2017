using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteRobotLib;

namespace RemoteControlRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = "192.168.54.108";
            string url = "localhost";
            var rightArm = new RemoteRobot(url, "T_ROB_R", RobotClientProvider.GetHttpClient(url));
            var leftArm = new RemoteRobot(url, "T_ROB_L", RobotClientProvider.GetHttpClient(url));

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
