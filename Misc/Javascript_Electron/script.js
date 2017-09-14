var emotionResults = [];

function start() {
    var video = document.getElementById('video');
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');

    //get Emotion every two seconds
    window.setInterval(function(){
        getEmotion();
    },2000);


    var tracker = new tracking.ObjectTracker('face');
    tracker.setInitialScale(4);
    tracker.setStepSize(2);
    tracker.setEdgesDensity(0.1);

    tracking.track('#video', tracker, { camera: true });


    //offline Library
    tracker.on('track', function (event) {
         context.clearRect(0, 0, canvas.width, canvas.height);
        var i =0;
        event.data.forEach(function (rect) {
            context.strokeStyle =  emotionResults[i];
            context.strokeRect(rect.x, rect.y, rect.width, rect.height);
            context.font = '11px Helvetica';
            context.fillStyle = "#fff";
            i++;
        });
    });





}

//This function takes the total emotion present on an image
//and find the dominant emotion across all the faces.
function findDominantEmotion(totalEmotion) {
    var result = Object.keys(totalEmotion).reduce(function (a, b) { return totalEmotion[a] > totalEmotion[b] ? a : b });
    return result;
}

//combine the emotions of the different faces in a picture.
function sumObjectsByKey() {
    return Array.from(arguments).reduce((a, b) => {
        for (let k in b) {
            if (b.hasOwnProperty(k))
                a[k] = (a[k] || 0) + b[k];
        }
        return a;
    }, {});
}

//function that evaluate emotion of each faces and draw it to the screen
function drawFaces(canvas, context, faces) {
    
    context.clearRect(0, 0, canvas.width, canvas.height);
    var result = findDominantEmotion(faces.scores);
    console.log(result);
    var color;
    switch (result) {
        case 'anger':
            color = "#FF0000";
            break;
        case 'neutral':
            color = "#AFEEEE";
            break;
        case 'contempt':
            color = "#4169E1";
            break;
        case 'disgust':
            color = "#EE82EE";
            break;
        case 'fear':
            color = "#FFFF00";
            break;
        case 'happiness':
            color = "#7FFF00";
            break;
        case 'sadness':
            color = "#006400";
            break;
        case 'surprise':
            color = "#9932CC";
            break;


            emotionResults.push(color);

    }
    //get the face rectangle to draw
    var faceRect = faces.faceRectangle;

    context.strokeStyle = color;
    context.strokeRect(faceRect.left, faceRect.top, faceRect.height, faceRect.width);
    moveRobot('right', 'wavehigh');


}

function getEmotion() {
    var video = document.getElementById('video');
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');


    var frame = captureVideoFrame('video', 'png');
    var apiKey = "[Your API Key]";
    var request = new XMLHttpRequest();
    request.open('POST', 'https://api.projectoxford.ai/emotion/v1.0/recognize', true);
    request.setRequestHeader("Content-Type", "application/octet-stream");
    request.setRequestHeader("Ocp-Apim-Subscription-Key", apiKey);
    request.send(frame.blob);


    request.onreadystatechange = function () {
        if (request.readyState == XMLHttpRequest.DONE) {
            var answer = JSON.parse(request.responseText);
            //reset the emotion List
            emotionList=[];
            answer.forEach(function (faces) {
              
                
                drawFaces(canvas, context, faces);

                //get the emotions of all the persons present and put them in an array
                var totalEmotion = sumObjectsByKey(faces.scores);

                var dominant = findDominantEmotion(totalEmotion);
                switch (dominant) {
                    case 'anger':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'neutral':
                        moveRobot("right", "No clue");
                        moveRobot("left", "No clue");
                        break;
                    case 'contempt':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'disgust':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'fear':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'happiness':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'sadness':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                    case 'surprise':
                        moveRobot("right", "anger");
                        moveRobot("left", "anger");
                        break;
                }

            }, this);
        }
    }
}


//function handling communication with the REST API and the robot
function moveRobot(arm, action) {
    var request = require('request')

    var url = "[Robot IP Adress]/rw";
    if (arm.toLowerCase().indexOf("right") != -1) {
        arm = "T_ROB_R"
    } else if (arm.toLowerCase().indexOf("left") != -1) {
        arm = "T_ROB_L";
    }
    else {
        throw ("function MoveRobot accept input \"right\" or \"left\"");
    }

    //request #1 to set the required move on the server
    request.post(url + '/rapid/symbol/data/RAPID/' + arm + '/Remote/' + action + '?action=set',
        //request #2 to set the starting variable to true to start the motion
        function (error, response, body) {
            request.post(url + '/rapid/symbol/data/RAPID/' + arm + '/Remote/start?action=set',
                { form: { value: 'true' } },
                function (error, response, body) {
                    var boolean = false;
               // ping  the server
                }).auth('Default User', 'robotics', false);
        }).auth('Default User', 'robotics', false);

}




