using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.IO;

public class DataCapture : MonoBehaviour
{

    public LeapProvider leapProvider;
    private Controller controller; // Leap Motion controller

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
    }

    // Update is called once per frame
    void Update()
    {
        Hand hand = leapProvider.CurrentFrame.Hands[0];
        if (hand != null)
        {
            Vector3 position = new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, hand.PalmPosition.z);

            OnUpdateHand(hand, position);

        }
    }

    void OnUpdateHand(Hand _hand, Vector3 _handposition)
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


        using (StreamWriter sw = new StreamWriter("output.txt", true))
        {
            // Write the output to the file
            sw.WriteLine(feature_str);

        }


    }
}
