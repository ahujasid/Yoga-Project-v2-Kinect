using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class PalmTreePose
    {
        public double AngleRightElbow;
        public double AngleRightShoulder;
        public double AngleLeftElbow;
        public double AngleLeftShoulder;

        //Flags
        public string speechString = "";

        bool rightShoulderStraight;
        bool leftShoulderStraight;
        bool rightArmStraight;
        bool leftArmStraight;


        public PalmTreePose()
        {
            rightShoulderStraight = false;
            leftShoulderStraight = false;
            rightArmStraight = false;
            leftArmStraight = false;
        }

        public double calcAngle(Vector3D vectorA, Vector3D vectorB)
        {
            double dotProduct;
            vectorA.Normalize();
            vectorB.Normalize();
            dotProduct = Vector3D.DotProduct(vectorA, vectorB);

            return (double)Math.Acos(dotProduct) / Math.PI * 180;
        }

        public void GetVector(Body skeleton)
        {
            Vector3D ShoulderCenter = new Vector3D(skeleton.Joints[JointType.SpineShoulder].Position.X, skeleton.Joints[JointType.SpineShoulder].Position.Y, skeleton.Joints[JointType.SpineShoulder].Position.Z);
            Vector3D RightShoulder = new Vector3D(skeleton.Joints[JointType.ShoulderRight].Position.X, skeleton.Joints[JointType.ShoulderRight].Position.Y, skeleton.Joints[JointType.ShoulderRight].Position.Z);
            Vector3D LeftShoulder = new Vector3D(skeleton.Joints[JointType.ShoulderLeft].Position.X, skeleton.Joints[JointType.ShoulderLeft].Position.Y, skeleton.Joints[JointType.ShoulderLeft].Position.Z);
            Vector3D RightElbow = new Vector3D(skeleton.Joints[JointType.ElbowRight].Position.X, skeleton.Joints[JointType.ElbowRight].Position.Y, skeleton.Joints[JointType.ElbowRight].Position.Z);
            Vector3D LeftElbow = new Vector3D(skeleton.Joints[JointType.ElbowLeft].Position.X, skeleton.Joints[JointType.ElbowLeft].Position.Y, skeleton.Joints[JointType.ElbowLeft].Position.Z);
            Vector3D RightWrist = new Vector3D(skeleton.Joints[JointType.WristRight].Position.X, skeleton.Joints[JointType.WristRight].Position.Y, skeleton.Joints[JointType.WristRight].Position.Z);
            Vector3D LeftWrist = new Vector3D(skeleton.Joints[JointType.WristLeft].Position.X, skeleton.Joints[JointType.WristLeft].Position.Y, skeleton.Joints[JointType.WristLeft].Position.Z);
            Vector3D RightHand = new Vector3D(skeleton.Joints[JointType.HandRight].Position.X, skeleton.Joints[JointType.HandRight].Position.Y, skeleton.Joints[JointType.HandRight].Position.Z);
            Vector3D LeftHand = new Vector3D(skeleton.Joints[JointType.HandLeft].Position.X, skeleton.Joints[JointType.HandLeft].Position.Y, skeleton.Joints[JointType.HandLeft].Position.Z);
            Vector3D YUpVector = new Vector3D(0.0, 1.0, 0.0);
            Vector3D XRightVector = new Vector3D(1.0, 0.0, 0.0);
            Vector3D ZBackVector = new Vector3D(0.0, 0.0, 1.0);

            AngleRightElbow = calcAngle(RightElbow - RightShoulder, RightElbow - RightWrist);
            AngleRightShoulder = calcAngle(YUpVector, RightShoulder - RightElbow);
            AngleLeftElbow = calcAngle(LeftElbow - LeftShoulder, LeftElbow - LeftWrist);
            AngleLeftShoulder = calcAngle(YUpVector, LeftShoulder - LeftElbow);

        }

        public void checkPose()
        {
            checkArms();
        }

        public void checkArms() {
            //Right arm flag setting
            if (AngleRightShoulder > 125)
            {
                rightShoulderStraight = true;
            }
            else { rightShoulderStraight = false; }

            if (AngleRightElbow > 140)
            {
                rightArmStraight = true;
            }

            else
            {
                rightArmStraight = false;
            }

            //Left arm flag setting
            if (AngleLeftShoulder > 125)
            {
                leftShoulderStraight = true;
            }
            else { leftShoulderStraight = false; }

            if (AngleLeftElbow > 140 && AngleLeftElbow < 190)
            {
                leftArmStraight = true;
            }

            else
            {
                leftArmStraight = false;
            }


            //Checking conditions
            if (!rightShoulderStraight && !leftShoulderStraight) { speechString = "raise your arms above your head"; }

            else if (!leftShoulderStraight)
            {
                speechString = "bring your left arm closer to your head";
            }

            else if (!rightShoulderStraight)
            {
                speechString = "bring your right arm closer to your head";
            }
            else if (!rightArmStraight && !leftArmStraight)
            {
                speechString = "make sure your arms are in a straight line and pointing directly upwards";
            }
            else if (!leftArmStraight)
            {
                speechString = "make sure your left arm is in a straight line and pointing directly upwards";
            }
            else if (!rightArmStraight)
            {
                speechString = "make sure your right arm is in a straight line and pointing directly upwards";
            }
            else { speechString = "your arms are perfect!"; }
        }
    }
}
