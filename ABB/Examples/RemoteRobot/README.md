# RemoteRobot
Remote Control an ABB Robot. Contains useful example code for using Robot Web Services.

There are examples for controlling the robot in two different ways.

In the first case, the robot is actively running and waiting for commands. By setting variables in the robots memory we can get it to do what we want.

In the second case, we are controlling the robot by directly telling it to execute certain pieces of code. We move the program pointer of the robot and then make it start program execution.

## Usage notes
1. Download the contents of this repository.
2. Edit example Program.cs with the url to your robot or virtual controller. Here you can also change way the robot is controlled.
3. Use RobotStudio to open P&G.
4. Start simulation from RobotStudio.
5. Run example program.
