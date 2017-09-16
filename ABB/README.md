
# Get started with RobotStudio and the Virtual Controller
RobotStudio is a simulation and programming tool for robots. For each virtual robot there is a **Virtual Controller (VC)** running the same software as the real robot. The VC controls robots motion and makes sure that is the same as for a real robot.

   * [Get RobotStudio](http://new.abb.com/products/robotics/robotstudio/downloads)
   * [Use a pre-installed Azure VM](https://github.com/Mandur/HackZurich2017/tree/master/Microsoft/RobotStudio-VM)
   
If you want to run RobotStudio in your own virtual machine you will need a license key. **Come to our booth and we will provide you with a free activation key**.

Learn more about RobotStudio by looking at the [tutorial videos](http://new.abb.com/products/robotics/robotstudio/tutorials). Here you can find out how to create your own movements and gestures. Also, come by our booth and we will help you!

## Get started with your Virtual YuMi
Open the [HackZurich.rspag](https://github.com/Mandur/HackZurich2017/tree/master/ABB/RobotStudio) using RobotStudio. Just select the default options in the wizard and your virtual YuMi will appear shortly.

![alt text](image/3.png)

## Starting the virtual robot

Change to the Simulation tab and start the simulation by pressing Play. The robot is now ready to receive commands.

![alt text](image/5.png)

## Robot Web Services
Robot Web Services is an HTTP based API for controlling robots. You will use it to control both virtual and real robots.

   * [API Reference](http://developercenter.robotstudio.com/blobproxy/devcenter/Robot_Web_Services/html/index.html)

The robot is preconfigured with a program which makes it execute gestures based on external commands. Each of the arms is running independently. They are represented by the tasks `T_ROB_L` and `T_ROB_R`. There are 4 steps to getting the robot to execute a gesture for one arm:
1. Update the RAPID variable `Remote/stName` for the  with the quoted string name of the gestures you want to run.
2. Set the bool `Remote/bStart` to `TRUE`.
3. Wait for the `Remote/bStart` variable to become `FALSE`.
4. Wait for the `Remote/bRunning` variable to become `FALSE`.

Many gestures require both arms to move at the same time. In this case make sure to run steps 1. and 2. in parallell for both arms.

### Connecting to the Virtual Controller
The VC will listen on port 80 for inbound HTTP traffic as soon as it is started. It is possible to interact with it via this REST API. Few things to note:
* The VC requires [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) from the users with a default username `Default User` and password `robotics`. Following classes have been tested to work with this communication:
    * [Requests](http://docs.python-requests.org/en/master/#) in Python. [Simple Example](Examples/PythonRobot)
    * [Request](https://www.npmjs.com/package/request) in Node.js [Example](../Misc/Javascript_Electron)
    * [Windows.Web.Http.HttpClient](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpclient) for C# Windows Store Application [Example](../Misc/UWP_C#)
    * [System.Net.Http.HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx) For C# and WinForms, WPF,... . [Example](Examples/RemoteRobot)

### Examples
* [RemoteRobot](https://github.com/Mandur/HackZurich2017/tree/master/ABB/Examples/RemoteRobot) (C#)
  A C# example with some (hopefully) reusable code.
* [PythonRobot](https://github.com/Mandur/HackZurich2017/tree/master/ABB/Examples/PythonRobot) (Python)
  A python example to get some communication set up with the robot.
  
There are also complete examples for [UWP](https://github.com/Mandur/HackZurich2017/tree/master/Misc/UWP_C%23) and in [JS](https://github.com/Mandur/HackZurich2017/tree/master/Misc/Javascript_Electron) that connect a robot with the Azure cloud services.

## Programming new gestures on the real robot

For programming the real robot see the [detailed instructions](https://github.com/Mandur/HackZurich2017/tree/master/ABB/YuMi).

## How does the REST API work?

There are three set of global variable available on each arm of the robot.
* `stName` string, define the name of the method to run
* `bStart` boolean, define if the gesture should be started
* `bRunning` boolean, define if a gesture is currently running

To run a gesture you should set the movement to run, to do that, just hit the endpoint with a POST request http://[your IP Adress]/rw/rapid/symbol/data/RAPID/[THE ARM]/Remote/stName?action=set', with a form payload set to {"value":"[Gesture you want to run]"}

Then you need to set the boolean variable to start in order to set the gesture, to do that just hit with a POST request http://[your IP Adress]/rw/rapid/symbol/data/RAPID/[THE ARM]/Remote/bStart?action=set', with a form payload set to {"value":"true"}

To check if the gesture finished running you can hit the endpoint with a GET request : 
 http://[your IP Adress]/rw/rapid/symbol/data/RAPID/[THE ARM]/Remote/bRunning?json=1',

## Preprogrammed gestures already available in the robot

Gestures only available for the right arm:
* `Kiss`
* `SayHello`
* `SayNo`
* `ShakingHands`
* `IKillYou`

Gestures for both arms:
* `Home`
* `Contempt`
* `NoClue`
* `HandsUp`
* `Surprised`
* `ToDiss`
* `Anger`
* `Excited`
* `GiveMeAHug`
* `GoAway`
* `Happy`
* `Powerful`
* `Scared`
