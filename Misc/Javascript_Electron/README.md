This simple Javascript project was made to show an end to end project using the congitive services and the connection with the YuMi robot. It uses Electron to run as a client application. It will detect your face and apply a color to the square around it based on your emotions, the emotions of all the faces in a picture will be added and a message will be sent to the robot, triggering a reaction based on this emotion.

To run it, please run the commands:
~~~~
npm install
bower install
npm start 
~~~~

Face recognition are computed using [tracking.js](https://trackingjs.com/) in order to get better latency, but you could extract the same information directly from the cognitive services API. 
* A call to the Emotion API is triggered every two seconds to get the emotions present on the faces (in the function getEmotion()). This populate the global variable emotionResults **Make sure to input the cognitive services API key here**
* This function calls the moveRobot() function that will handle communication and Robot's motion **Make sure to input the robot IP adress there**


NB. I am not a Node expert, so if you have enhancement or feedback, just do a PR or drop by the booth.
