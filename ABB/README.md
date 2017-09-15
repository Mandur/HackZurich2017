
# Get started with RobotStudio and the Virtual Controller
RobotStudio is a simulation and programming tool for robots. For each virtual robot there is a **Virtual Controller (VC)** running the same software as the real robot. The VC controls robots motion and makes sure that is the same as for a real robot.

   * [Get RobotStudio](http://new.abb.com/products/robotics/robotstudio/downloads)
   * [Use a pre-installed Azure VM](https://github.com/Mandur/HackZurich2017/tree/master/Microsoft/RobotStudio-VM)
   
If you want to run RobotStudio in your own virtual machine you will need a license key. **Come to our booth and we will provide you with a free activation key**.

Learn more about RobotStudio by looking at the [tutorial videos](http://new.abb.com/products/robotics/robotstudio/tutorials). Here you can find out how to create your own movements and gestures. Also, come by our booth and we will help you!

## Get started with your Virtual YuMi
Open the [HackZurich.rspag](https://github.com/Mandur/HackZurich2017/tree/master/ABB/RobotStudio) (in your Azure VM it will be on the desktop) using RobotStudio. Just select the default options in the wizard and your virtual YuMi will appear shortly.

![alt text](image/3.png)

## Starting the virtual robot

Change to the Simulation tab and start the simulation by pressing Play. The robot is now ready to receive commands.

![alt text](image/5.png)

## Robot Web Services
Robot Web Services is an HTTP based API for controlling robots. You will use it to control both virtual and real robots.

   * [API Reference](http://developercenter.robotstudio.com/blobproxy/devcenter/Robot_Web_Services/html/index.html)

## Connecting to the Virtual Controller
The VC will listen on port 80 for inbound HTTP traffic as soon as it is started. It is possible to interact with it via this REST API. Few things to note:
* The VC requires [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) from the users with a default username/password of 'Default User'/'robotics' (without ' marks). Following classes have been tested to work with this communication:
    * [Requests](http://docs.python-requests.org/en/master/#) in Python. [Simple Example](Examples/PythonRobot)
    * [Request](https://www.npmjs.com/package/request) in Node.js [Example](../Misc/Javascript_Electron)
    * [Windows.Web.Http.HttpClient](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpclient) for C# Windows Store Application [Example](../Misc/UWP_C#)
    * [System.Net.Http.HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx) For C# and WinForms, WPF,... . [Example](Examples/RemoteRobot)

## Examples
  ## [RemoteRobot](https://github.com/Mandur/HackZurich2017/tree/master/ABB/Examples/RemoteRobot) (C#)
  A C# example with some (hopefully) reusable code.
  ## [PythonRobot](https://github.com/Mandur/HackZurich2017/tree/master/ABB/Examples/PythonRobot) (Python)
  A python example to get some communication set up with the robot.
  
There are also complete examples for [UWP](https://github.com/Mandur/HackZurich2017/tree/master/Misc/UWP_C%23) and in [JS](https://github.com/Mandur/HackZurich2017/tree/master/Misc/Javascript_Electron) that connect a robot with the Azure cloud services.

## Programming new gestures on the real robot

For programming the real robot see the [detailed instructions](https://github.com/Mandur/HackZurich2017/tree/master/ABB/YuMi).
