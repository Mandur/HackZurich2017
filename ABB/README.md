
# Connecting to RobotStudio
Robotstudio will listen port 80 fro inbound HTTP traffic as soon as it is started. It is possible to interact with it via this REST API. Few things to note:
* RobotStudio require an [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) from the users with a default username/password of 'Default User'/'robotics' (without ' marks). Following classes have been tested to work with this communication:
    * [Requests](http://docs.python-requests.org/en/master/#) in Python. [Simple Example](Examples/PythonRobot)
    * [Request](https://www.npmjs.com/package/request) in Node.js [Example](../Misc/Javascript_Electron)
    * [Windows.Web.Http.HttpClient](https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpclient) for C# Windows Store Application [Example](../Misc/UWP_C#)
    * [System.Net.Http.HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.118).aspx) For C# and WinForms, WPF,... . [Example](Examples/RemoteRobot)

# Triggering motion with RobotStudio
The default gesture already configured in RobotStudio are the following: 
    * Home 
    * Contempt
    * Kiss
    * No Clue
    * Hands Up
    * Suprised
    * To Diss
    * Anger
    * Excited
    * Give Me A Hug
    * Go Away
    * Happy
    * Powerfull
    * Scared