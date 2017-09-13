
#Connecting to RobotStudio
Robotstudio will listen port 80 fro inbound HTTP traffic as soon as it is started. It is possible to interact with it via this REST API. Few things to note:
* RobotStudio require an [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) from the users with a default username/password of 'Default User'/'robotics' (without ' marks). Following classes have been tested to work with this communication:
    * [Requests](http://docs.python-requests.org/en/master/#) in Python. [Simple Example](Examples/PythonRobot)
    * [Request](https://www.npmjs.com/package/request) in Node.js
    * [CredentialCache](https://msdn.microsoft.com/en-us/library/system.net.credentialcache(v=vs.110).aspx) in C#. [Example](Examples/RemoteRobot)

#Triggering motion with RobotStudio
TODO 