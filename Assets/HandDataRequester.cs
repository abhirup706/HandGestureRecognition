using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
///     Example of requester who only sends Hello. Very nice guy.
///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.
/// </summary>
[Serializable]
public class HandDataRequester : RunAbleThread
{
    /// <summary>
    ///     Request Hello message to server and receive message back. Do it 10 times.
    ///     Stop requesting when Running=false.
    /// </summary>
    /// 

    private Controller controller;

    public LeapProvider leapProvider;
    public string out_str;


    public override void Run()
    {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");

            for (int i = 0; i < 10 && Running; i++)
            {
                //Debug.Log("Sending Hello");
                //client.SendFrame("Hello");
                //// ReceiveFrameString() blocks the thread until you receive the string, but TryReceiveFrameString()
                //// do not block the thread, you can try commenting one and see what the other does, try to reason why
                //// unity freezes when you use ReceiveFrameString() and play and stop the scene without running the server
                ////                string message = client.ReceiveFrameString();
                ////                Debug.Log("Received: " + message);
                string message = null;
                bool gotMessage = false;

                Hand hand = leapProvider.CurrentFrame.Hands[i];
                Vector3 position = new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, hand.PalmPosition.z);

                Debug.Log("Hand" + hand + " position : " + position);

                out_str = OnUpdateHand(hand, position);

                Debug.Log("sending feature string : " + out_str);

                client.SendFrame(out_str);

                while (Running)
                {
                    gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                    if (gotMessage) break;
                }

                if (gotMessage) Debug.Log("Received " + message);
            }
        }

        NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use, not sure why yet
    }


    string OnUpdateHand(Hand _hand, Vector3 _handposition)
    {
        //Use _hand to Explicitly get the specified fingers from it
        Finger _thumb = _hand.GetThumb();
        Finger _index = _hand.GetIndex();
        Finger _middle = _hand.GetMiddle();
        Finger _ring = _hand.GetRing();
        Finger _pinky = _hand.GetPinky();
        string feature_str = "";



        for (int j = 0; j < 4; j++)
        {
            Vector3 jointPosition = new Vector3(_thumb.Bone((Bone.BoneType)j).NextJoint.x, _thumb.Bone((Bone.BoneType)j).NextJoint.y, _thumb.Bone((Bone.BoneType)j).NextJoint.z);

            jointPosition -= _handposition;

            Debug.Log("Joint position : " + j + jointPosition);
            string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += ",";




        }

        Vector3 thumb_direction = _thumb.Direction;
        feature_str += string.Format("{0}, {1}, {2}", thumb_direction.x, thumb_direction.y, thumb_direction.z);
        feature_str += ",";

        for (int j = 0; j < 4; j++)
        {
            Vector3 jointPosition = new Vector3(_index.Bone((Bone.BoneType)j).NextJoint.x, _index.Bone((Bone.BoneType)j).NextJoint.y, _index.Bone((Bone.BoneType)j).NextJoint.z);

            jointPosition -= _handposition;

            Debug.Log("Joint position : " + j + jointPosition);
            feature_str += string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += ",";

        }

        Vector3 index_direction = _index.Direction;
        feature_str += string.Format("{0}, {1}, {2}", index_direction.x, index_direction.y, index_direction.z);
        feature_str += ",";

        for (int j = 0; j < 4; j++)
        {
            Vector3 jointPosition = new Vector3(_middle.Bone((Bone.BoneType)j).NextJoint.x, _middle.Bone((Bone.BoneType)j).NextJoint.y, _middle.Bone((Bone.BoneType)j).NextJoint.z);

            jointPosition -= _handposition;

            Debug.Log("Joint position : " + j + jointPosition);
            feature_str += string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += ",";


        }

        Vector3 middle_direction = _middle.Direction;
        feature_str += string.Format("{0}, {1}, {2}", middle_direction.x, middle_direction.y, middle_direction.z);
        feature_str += ",";

        for (int j = 0; j < 4; j++)
        {
            Vector3 jointPosition = new Vector3(_ring.Bone((Bone.BoneType)j).NextJoint.x, _ring.Bone((Bone.BoneType)j).NextJoint.y, _ring.Bone((Bone.BoneType)j).NextJoint.z);

            jointPosition -= _handposition;

            Debug.Log("Joint position : " + j + jointPosition);
            feature_str += string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += ",";


        }

        Vector3 ring_direction = _ring.Direction;
        feature_str += string.Format("{0}, {1}, {2}", ring_direction.x, ring_direction.y, ring_direction.z);
        feature_str += ",";

        for (int j = 0; j < 4; j++)
        {
            Vector3 jointPosition = new Vector3(_pinky.Bone((Bone.BoneType)j).NextJoint.x, _pinky.Bone((Bone.BoneType)j).NextJoint.y, _pinky.Bone((Bone.BoneType)j).NextJoint.z);

            jointPosition -= _handposition;

            Debug.Log("Joint position : " + j + jointPosition);
            feature_str += string.Format("{0}, {1}, {2}", jointPosition.x, jointPosition.y, jointPosition.z);
            feature_str += ",";


        }

        Vector3 pinky_direction = _index.Direction;
        feature_str += string.Format("{0}, {1}, {2}", pinky_direction.x, pinky_direction.y, pinky_direction.z);


        return feature_str;

        //using (StreamWriter sw = new StreamWriter("output.txt",true))
        //{
        //    // Write the output to the file
        //    sw.WriteLine(feature_str);

        //}


    }


}