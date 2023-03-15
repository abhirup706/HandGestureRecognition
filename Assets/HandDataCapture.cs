using UnityEngine;
using Leap;
using Leap.Unity;
using System.Collections.Generic;
using System;
using System.IO;


public class HandDataCapture : MonoBehaviour
{

    // Create a new Leap Motion controller object
    private Controller controller;

    public HandDataRequester handDataRequester;

    void Start()
    {

        handDataRequester.Start();


        // Initialize the controller
        controller = new Controller();
    }



    void OnDestroy()
    {
        handDataRequester.Stop();
    }

}


//class Program
//{
//    static void Main(string[] args)
//    {
//        // Open the file for writing
//        using (StreamWriter sw = new StreamWriter("output.txt"))
//        {
//            // Write the output to the file
//            sw.WriteLine("Hello, world!");
//        }
//    }
//}
