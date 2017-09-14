#RobotStudio tutorial

##Connecting to RobotStudio
RobotStudio will listen port 80 for inbound HTTP traffic as soon as it is started. It is possible to interact with it via this REST API. Few things to note:
*	RobotStudio require an [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) from the users with a default username/password of 'Default User'/'robotics' (without ' marks). Following classes have been tested to work with this communication:
*	[Requests](http://docs.python-requests.org/en/master/) in Python. Simple Example
*	Request in Node.js Example
*	Windows.Web.Http.HttpClient for C# Windows Store Application Example
*	System.Net.Http.HttpClient For C# and WinForms, WPF,... . Example

##Triggering motion
To change data on the robot controller or start gestures, you must change some variables. How you can change a variable on the robot controller you can find it in the online manual of the web service API of ABB or the example above. 
You can find the important instructions and information’s in the online manual at the following slide:
Robot Web Service -> RobotWare Service -> RAPID Service -> Operations on RAPID data
To start a gesture you need to change the string with the gesture name stName and the bool variable bStart for the start trigger.
e.g.
´´´
curl –digest -u "Default User":robotics -d value="Home" http://{IP_of_robot}/rw/rapid/symbol/data/RAPID/T_ROB_R/Remote/stName?action=set
curl –digest -u "Default User":robotics -d value=TRUE http://{IP_of_ robot }/rw/rapid/symbol/data/RAPID/T_ROB_R/Remote/bStart?action=set
´´´
If there are gestures which need both arms, you must change the variables at both arms. The arms wait 5 second for the other one until it give an error. 
e.g. Gesture “Happy”
´´´
curl –digest -u "Default User":robotics -d value="Happy" http://{IP_of_ robot }/rw/rapid/symbol/data/RAPID/T_ROB_R/Remote/stName?action=set
curl –digest -u "Default User":robotics -d value="Happy" http://{IP_of_ robot }/rw/rapid/symbol/data/RAPID/T_ROB_L/Remote/stName?action=set
curl –digest -u "Default User":robotics -d value=TRUE http://{IP_of_ robot }/rw/rapid/symbol/data/RAPID/T_ROB_R/Remote/bStart?action=set
curl –digest -u "Default User":robotics -d value=TRUE http://{IP_of_ robot }/rw/rapid/symbol/data/RAPID/T_ROB_L/Remote/bStart?action=set
´´´

The response of one set-command should be NO_CONTENT. If other response are received, check the online manual or talk to the staff at the booth.
If a gesture was started, the variable bRunning was set to TRUE and after the gesture it returns to FALSE. You can read the value or subscribe on it. Have a look at the online manual for this.
If the robot give a response or try to say something, the text will be stored in the variable stAnswer. 
It is also possible to create more variables for further or more complex functions. For that, contact the stuff at the booth.
Points which should be in mind:
•	Gesture commands are always case sensitive.
•	No space is allowed in the command
The default gesture already configured in RobotStudio are the following:
Gestures only for the right arm:
•	Kiss
•	SayHello
•	SayNo
•	ShakingHands
•	IKillYou
Gestures with both arms:
•	Home
•	Contempt
•	NoClue
•	HandsUp
•	Suprised
•	ToDiss
•	Anger
•	Excited
•	GiveMeAHug
•	GoAway
•	Happy
•	Powerfull
•	Scared

