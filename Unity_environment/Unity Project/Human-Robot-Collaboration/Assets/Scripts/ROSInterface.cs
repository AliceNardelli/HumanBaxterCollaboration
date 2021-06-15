using JointState = RosMessageTypes.Sensor.JointState;
using RosMessageTypes.HumanBaxterCollaboration;
using Bool = RosMessageTypes.Std.Bool;
using UnityEngine;

public class ROSInterface : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    
    // Variables required for ROS communication
    public string trajectoryTopicName = "baxter_moveit_trajectory";
    public string unityTfTopicName = "unity_tf";
    public string jointStateTopicName = "baxter_joint_states";
    public string LeftGripperTopicName = "left_gripper_pose";
    public string RightGripperTopicName = "right_gripper_pose";
    public string RightcontrolTopicName = "open_close_right";
    public string LeftcontrolTopicName = "open_close_left";


    public GameObject baxter;
    public GameObject avatar;

    private BaxterController controller;
    private TFManager tfManager;

    private bool simStarted;
    private float timeElapsedTf;
    private float timeElapsedJS;
    private float timeElapsedESL;
    private float timeElapsedESR;
    private float publishTfFrequency;
    private float publishJSFrequency;
    private float publishESFrequencyL;
    private float publishESFrequencyR;

    void Start()
    {
        simStarted = false;
        timeElapsedTf = 0;
        timeElapsedJS = 0;
        timeElapsedESL = 0;
        timeElapsedESR = 0;
        publishTfFrequency = 0.05f;
        publishJSFrequency = 1.0f;
        publishESFrequencyL = 0.5f;
        publishESFrequencyR = 0.5f;

        // Get ROS connection static instance
        ros = ROSConnection.instance;

        // Instantiate Baxter Controller
        controller = gameObject.AddComponent<BaxterController>();
        controller.Init(baxter);

        // Subscribe to MoveIt trajectory topic
        ros.Subscribe<BaxterTrajectory>(trajectoryTopicName, controller.TrajectoryResponse);

        //subscriber for open_close_right
        ros.Subscribe<Bool>(RightcontrolTopicName, controller.OpenCloseRight);

        //subscriber for open_close_left
        ros.Subscribe<Bool>(LeftcontrolTopicName, controller.OpenCloseLeft);

        // Instantiate TF Manager component
        tfManager = gameObject.AddComponent<TFManager>();
        tfManager.Init(avatar, baxter.transform.Find("ground"));

    }

    public void RestCommand()
    {
        controller.GoToRestPosition("both");
        simStarted = true;
    }

    public void Update()
    {
        if (simStarted)
        {
            timeElapsedTf += Time.deltaTime;
            timeElapsedJS += Time.deltaTime;
            timeElapsedESL += Time.deltaTime;
            timeElapsedESR += Time.deltaTime;

            if (timeElapsedTf > publishTfFrequency)
            {
                UnityTf unityTfMsg = tfManager.GetUnityTfMessage();
                ros.Send(unityTfTopicName, unityTfMsg);

                timeElapsedTf = 0;
            }

            if (timeElapsedJS > publishJSFrequency)
            {
                JointState jointStateMsg = controller.GetBaxterJointState();
                ros.Send(jointStateTopicName, jointStateMsg);

                timeElapsedJS = 0;
            }
            if (timeElapsedESR > publishESFrequencyR)
            {
                var rightGripperPose = controller.GetRightGripper();
                ros.Send(RightGripperTopicName, rightGripperPose);

                timeElapsedESR = 0;
            }
            if (timeElapsedESL > publishESFrequencyL)
            {
                var leftGripperPose = controller.GetLeftGripper();
                ros.Send(LeftGripperTopicName, leftGripperPose);

                timeElapsedESL = 0;
            }

        }
    }
}