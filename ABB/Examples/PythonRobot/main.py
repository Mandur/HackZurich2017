import requests
from requests.auth import HTTPDigestAuth


headers = {"Ocp-Apim-Subscription-Key": apiKey}

def moveRobot(arm,action):
    url = '[Enter RobotStudio URL]/rw'
    r1 = requests.post(url + '/rapid/symbol/data/RAPID/'+arm+'/Remote/'+action+'?action=set', auth=HTTPDigestAuth('Default User', 'robotics'))
    print(r1)
    r2 = requests.post(url + '/rapid/symbol/data/RAPID/'+arm+'/Remote/start?action=set',data={'value':'true'},auth=HTTPDigestAuth('Default User', 'robotics'))
    print(r2)
    return;



moveRobot('T_ROB_R','wavehigh')
