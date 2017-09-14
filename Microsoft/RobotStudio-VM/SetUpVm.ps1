try
{
  #Verify if PowerShellGet module is installed. If not install
  if (!(Get-Module -Name PowerShellGet))
  {

      netsh advfirewall firewall add rule name="Open Port 80" dir=in action=allow protocol=TCP localport=80
      New-Item $($env:TEMP  +'\HackZurich') -type directory
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/ABB/RobotStudio/HackZurich.rspag' -OutFile $([Environment]::GetFolderPath("Desktop") +'\HackZurich\HackZurich.rspag')
      Invoke-WebRequest 'https://raw.githubusercontent.com/Mandur/HackZurich2017/master/Microsoft/RobotStudio-VM/vcconf.xml' -OutFile $([Environment]::GetFolderPath("Desktop")+'\HackZurich\vcconf.xml') 
      Add-WindowsCapability –Online -Name NetFx3~~~~ –Source D:\sources\sxs
      Invoke-WebRequest 'http://cdn.robotstudio.com/install/RobotStudio_6.05.02.zip' -OutFile $($env:TEMP +'\HackZurich\RobotStudio_6.05.02.zip')
      expand-archive -path $($env:TEMP +'\HackZurich\RobotStudio_6.05.02.zip') -destinationpath $($env:TEMP+'\HackZurich')
      Start-Process $($env:TEMP +'\HackZurich\RobotStudio\ABB RobotStudio 6.05.02.msi') -ArgumentList "/qn" -Wait
      cd $($env:TEMP+'\HackZurich\RobotStudio\')
      .\setup.exe /s /v"/qb ADDLOCAL=ALL"

        
  }

}
catch
{
    Write-Output "Oops. Something failed"
}

try{
    
  #Verify if PSWindowsUpdate PowerShell Module is installed. If not install.
    if (!(Get-Module -Name PSWindowsUpdate -List)){
        Install-Module -Name PSWindowsUpdate -Scope AllUsers -Confirm:$false -Force
    }
    Get-WUInstall -WindowsUpdate -AcceptAll -AutoReboot -Confirm:$FALSE -ErrorAction stop

}