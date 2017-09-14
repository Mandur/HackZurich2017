﻿try
{
  #Verify if PowerShellGet module is installed. If not install
  if (!(Get-Module -Name PowerShellGet))
  {

      netsh advfirewall firewall add rule name="Open Port 80" dir=in action=allow protocol=TCP localport=80
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/ABB/RobotStudio/HackZurich.rspag' -OutFile $([Environment]::GetFolderPath("Desktop") +'\HackZurich.rspag')
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/Microsoft/RobotStudio-VM/vcconf.xml' -OutFile $([Environment]::GetFolderPath("Desktop") +'\vcconf.xml') 

      Invoke-WebRequest 'http://cdn.robotstudio.com/install/RobotStudio_6.05.02.zip' -OutFile $([Environment]::GetFolderPath("Desktop") +'\RobotStudio_6.05.02.zip')
      expand-archive -path $([Environment]::GetFolderPath("Desktop") +'\RobotStudio_6.05.02.zip') -destinationpath $([Environment]::GetFolderPath("Desktop"))
      Start-Process $([Environment]::GetFolderPath("Desktop")+'ABB RobotStudio 6.05.02.msi') -ArgumentList "/qn /a" -Wait
      Start-Process msiexec.exe -Wait -ArgumentList '/I C:\RobotStudio\ABB RobotStudio 6.05.02.msi /quiet'
        
  }
    
  #Verify if PSWindowsUpdate PowerShell Module is installed. If not install.
    if (!(Get-Module -Name PSWindowsUpdate -List)){
        Install-Module -Name PSWindowsUpdate -Scope AllUsers -Confirm:$false -Force
    }
    Get-WUInstall -WindowsUpdate -AcceptAll -AutoReboot -Confirm:$FALSE -ErrorAction stop
}
catch
{
    Write-Output "Oops. Something failed"
}