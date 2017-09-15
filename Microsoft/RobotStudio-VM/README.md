Have a ready-to-use Windows VM with RobotStudio configured to work out of the box!

Follow this few steps to get activated:

* Activate your Azure account by grabbing a free voucher at our booth
* press the button here below, select West Europe as a location, pick a name and a password for your machine. Then accept the conditions and press ok.

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FMandur%2FHackZurich2017%2Fmaster%2FMicrosoft%2FRobotStudio-VM%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

* Wait c.a. 20 minutes for the VM to install RobotStudio and perform all configuration steps. The deployment will be marked as 'failed' but it will work anyway
* Connect to the Vm via RDP 
* All the required files for the next steps are located on C:\Windows\Temp\HackZurich
* Double click on the HackZurich.rspag file to extract the robot archive. Wait until it finish and save the the robot. 
* Close RobotStudi
* Edit the file vcconf.xml and insert your current IP adress 
* Copy the file to C:\Users\[Your User Name]]\AppData\Roaming\ABB Industrial IT\Robotics IT\RobVC 
* Start RobotStudio again
* from your local computer go to http://[IP adress of the VM]/rw, log in with credentials "Default User" / "Robotics" (without "), if you see a list of dots on the screen you are good to go! If you see a RAPI error come to see us on the booth