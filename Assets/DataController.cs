using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using TMPro;

public class DataController : MonoBehaviour
{
    private const string HOST = "127.0.0.1";  // Localhost IP address
    private const int PORT = 5000;  // Port number for the socket server
    public LeapProvider leapProvider;

    private TcpClient client;
    private NetworkStream stream;
    private Controller controller; // Leap Motion controller

    public TextMeshProUGUI textMesh; // If using TextMeshPro



    void Start()
    {
        client = new TcpClient(HOST, PORT);
        stream = client.GetStream();
        controller = new Controller();
    }

    void Update()
    {


        Hand hand = leapProvider.CurrentFrame.Hands[0];

        if (hand != null)
        {
            Vector3 position = new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, hand.PalmPosition.z);

            //Debug.Log("Hand" + hand + " position : " + position);

            string data = OnUpdateHand(hand, position);

            //string data = "Hello from Unity";  // The data to be processed by Python

            //Debug.Log(data);
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            stream.Write(dataBytes, 0, dataBytes.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string result = Encoding.ASCII.GetString(buffer, 0, bytesRead);

// Debug.Log($"Result received from Python: {result}");
            
            if (Int32.Parse(result) == 0)
            {
                //Debug.Log("Gesture : OK");
                textMesh.text = "<size=10>Gesture : OK <size=11><sprite=0></size></size>"; // If using TextMeshPro
            }
            else if (Int32.Parse(result) == 1)
            {
                //Debug.Log("Gesture : Five");
                textMesh.text = "<size=10>Gesture : Five <size=11><sprite=2></size></size>";
            }
            else if (Int32.Parse(result) == 2)
            {
                //Debug.Log("Gesture : Peace");
                textMesh.text = "<size=10>Gesture : Peace <size=11><sprite=3></size></size>";
            }
            else if (Int32.Parse(result) == 3)
            {
                //Debug.Log("Gesture : Paw");
                textMesh.text = "<size=10>Gesture : Paw <size=11><sprite=1></size></size>";
            }
            else if (Int32.Parse(result) == 4)
            {
                //Debug.Log("Gesture : Shoot");
                textMesh.text = "<size=10>Gesture : Shoot <size=11><sprite=4></size></size>";
            }
            else
            {
                textMesh.text = "<size=10>Sorry! The gesture is not recognized</size>";
            }

            // Display the result in Unity (for example, update a UI element)
        }
        else
        {
            textMesh.text = "Please create one of the following hand gesture:\n1.OK < size = 8 >< sprite = 0 ></ size > 2.Peace < size = 8 >< sprite = 3 ></ size > 3.Five < size = 9 >< sprite = 2 ></ size >\n4.Paw < size = 9 >< sprite = 1 ></ size > 5.Shoot < size = 9 >< sprite = 4 ></ size > ";
        }

    }

    void OnDestroy()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
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

            //Debug.Log("Joint position : " + j + jointPosition);
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

            //Debug.Log("Joint position : " + j + jointPosition);
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

            //Debug.Log("Joint position : " + j + jointPosition);
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

            //Debug.Log("Joint position : " + j + jointPosition);
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

            //Debug.Log("Joint position : " + j + jointPosition);
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
