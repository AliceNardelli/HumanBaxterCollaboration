# Human Baxter Collaboration
## How to run the code 

To start with, please download the docker image at the link [here](https://hub.docker.com/r/hypothe/sofar_ros) and follow the instructions given. 
 

### On the ROS side
To start with, please download the docker image at the link [here](https://hub.docker.com/r/hypothe/sofar_ros). Then
1. Move to the `ros_ws/src` directory and remove all the packages already present
2. Build all the packages existing inside the `ROS_environment`
3. run
   ```
   roslaunch human_baxter_collaboration human_baxter_collaboration.launch
   ```



### On the Unity side 
Please, download the scene available on this repo,  at [Unity_environment](https://github.com/AliceNardelli/HumanBaxterCollaboration/Unity_environment) folder. Then, please follow the instruction reported  [here](https://hub.docker.com/r/hypothe/sofar_ros) to realize a ROS-Unity interface.
After having launched the `roslaunch` command, start the simulation by:
1. clicking on the tailored button `start simultaion`
2. clicking on `pick and place`
