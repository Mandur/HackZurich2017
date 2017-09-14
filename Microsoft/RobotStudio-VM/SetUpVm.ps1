try
{
  #Verify if PowerShellGet module is installed. If not install
  if (!(Get-Module -Name PowerShellGet))
  {

      Invoke-WebRequest 'http://cdn.robotstudio.com/install/RobotStudio_6.05.02.zip' -OutFile $($env:temp +'\RobotStudio_6.05.02.zip')
      expand-archive -path $($env:temp +'\RobotStudio_6.05.02.zip') -destinationpath $($env:temp +'\RobotStudio')
      Start-Process $($env:temp +'\RobotStudio\RobotStudio\ABB RobotStudio 6.05.02.msi') -ArgumentList "/qn" -Wait
      netsh advfirewall firewall add rule name="Open Port 80" dir=in action=allow protocol=TCP localport=80
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/ABB/RobotStudio/HackZurich.rspag' -OutFile $([Environment]::GetFolderPath("Desktop") +'\HackZurich.rspag')
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/ABB/RobotStudio/HackZurich.rspag' -OutFile $([Environment]::GetFolderPath("Desktop") +'\vcconf.xml') 
  
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