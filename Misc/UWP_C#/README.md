A Simple Windows store application to illustrate connection between the Cognitive Services and the YuMi Robot. An offline face tracking library provided by Windows is used to make the experience more smoothly and less laggy, note that the face information provided by the cognitive services could also have been used

To run the sample
* Make sure to fill the Robot IP adress and your Cognitive services API keys.
* Make sure the RobotStudio Station is configured as described in the Main page

Two different streams are running in the sample:

The offline face recognition is operating at 15 FPS  (every 66 ms) in the method *ProcessCurrentVideoFrame*. The method takes a snapshot in NV12 format (to be processed by the Face library) and convert it into BGRA format (to display faces in the *ShowDetectedFaces* method). 
This method will draw the faces rectangle and give them a color based on the detected emotion from this face. The harmonization of the two face array (offline and online) is done simply by ordering the two arrays based on the X coordinate. Note that this is widely suboptimal and should probably be improved in a future version. Note that this method run on the UI thread to be able to draw on the canvas.
Method *MoveRobot* is called on a background thread, as its idea is to compute the main emotion present on a picture (adding all the values of the faces and picking the highest one). It will then send a Rest Request to the Robot in order to make him move accordingly with an appropriate emotion!

The Cognitive Emotion detection is operating every 2 sec in the method *AnalyzeEmotion*. The method will simply make a call to the rest api and populate the *Emotion* array, that will be consumed by the *ShowDetectedFaces* method later on.

The two are synced by accessing/writing in the static *Emotion* array (to be thread safe).