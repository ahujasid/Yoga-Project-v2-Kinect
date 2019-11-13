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
    class TableTopPose
    {
        public double AngleRightElbow;
        public double AngleRightShoulder;
        public double AngleLeftElbow;
        public double AngleLeftShoulder;
        public double AngleLeftShin;
        public double AngleRightShin;
        public double AngleLeftThigh;
        public double AngleRightThigh;
        public double AngleSpine;

        //Flags
        public string speechString = "";
        public string poseString = "";

        bool thighsStraight;
        bool shouldersStraight;
        bool spineCorrect;


        public TableTopPose()
        {
            shouldersStraight = false;
            thighsStraight = false;
            spineCorrect = false;
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
           
            Vector3D RightShoulder = new Vector3D(skeleton.Joints[JointType.ShoulderRight].Position.X, skeleton.Joints[JointType.ShoulderRight].Position.Y, skeleton.Joints[JointType.ShoulderRight].Position.Z);
            Vector3D LeftShoulder = new Vector3D(skeleton.Joints[JointType.ShoulderLeft].Position.X, skeleton.Joints[JointType.ShoulderLeft].Position.Y, skeleton.Joints[JointType.ShoulderLeft].Position.Z);
            Vector3D RightElbow = new Vector3D(skeleton.Joints[JointType.ElbowRight].Position.X, skeleton.Joints[JointType.ElbowRight].Position.Y, skeleton.Joints[JointType.ElbowRight].Position.Z);
            Vector3D LeftElbow = new Vector3D(skeleton.Joints[JointType.ElbowLeft].Position.X, skeleton.Joints[JointType.ElbowLeft].Position.Y, skeleton.Joints[JointType.ElbowLeft].Position.Z);
            Vector3D LeftHand = new Vector3D(skeleton.Joints[JointType.WristLeft].Position.X, skeleton.Joints[JointType.WristLeft].Position.Y, skeleton.Joints[JointType.WristLeft].Position.Z);
            Vector3D RightHand = new Vector3D(skeleton.Joints[JointType.WristRight].Position.X, skeleton.Joints[JointType.WristRight].Position.Y, skeleton.Joints[JointType.WristRight].Position.Z);

            Vector3D ShoulderCenter = new Vector3D(skeleton.Joints[JointType.SpineShoulder].Position.X, skeleton.Joints[JointType.SpineShoulder].Position.Y, skeleton.Joints[JointType.SpineShoulder].Position.Z);
            Vector3D SpineMid = new Vector3D(skeleton.Joints[JointType.SpineMid].Position.X, skeleton.Joints[JointType.SpineMid].Position.Y, skeleton.Joints[JointType.SpineMid].Position.Z);
            Vector3D SpineBase = new Vector3D(skeleton.Joints[JointType.SpineBase].Position.X, skeleton.Joints[JointType.SpineBase].Position.Y, skeleton.Joints[JointType.SpineBase].Position.Z);

            Vector3D HipLeft = new Vector3D(skeleton.Joints[JointType.HipLeft].Position.X, skeleton.Joints[JointType.HipLeft].Position.Y, skeleton.Joints[JointType.HipLeft].Position.Z);
            Vector3D HipRight = new Vector3D(skeleton.Joints[JointType.HipRight].Position.X, skeleton.Joints[JointType.HipRight].Position.Y, skeleton.Joints[JointType.HipRight].Position.Z);
            Vector3D KneeLeft = new Vector3D(skeleton.Joints[JointType.KneeLeft].Position.X, skeleton.Joints[JointType.KneeLeft].Position.Y, skeleton.Joints[JointType.KneeLeft].Position.Z);
            Vector3D KneeRight = new Vector3D(skeleton.Joints[JointType.KneeRight].Position.X, skeleton.Joints[JointType.KneeRight].Position.Y, skeleton.Joints[JointType.KneeRight].Position.Z);
            Vector3D AnkleLeft = new Vector3D(skeleton.Joints[JointType.AnkleLeft].Position.X, skeleton.Joints[JointType.AnkleLeft].Position.Y, skeleton.Joints[JointType.AnkleLeft].Position.Z);
            Vector3D AnkleRight = new Vector3D(skeleton.Joints[JointType.AnkleRight].Position.X, skeleton.Joints[JointType.AnkleRight].Position.Y, skeleton.Joints[JointType.AnkleRight].Position.Z);

            Vector3D YUpVector = new Vector3D(0.0, 1.0, 0.0);
            Vector3D XRightVector = new Vector3D(1.0, 0.0, 0.0);
            Vector3D ZBackVector = new Vector3D(0.0, 0.0, 1.0);


            AngleRightShoulder = calcAngle(RightShoulder - RightElbow, RightElbow - RightHand);
            AngleLeftShoulder = calcAngle(LeftShoulder - LeftElbow, LeftElbow - LeftHand);

            AngleSpine = calcAngle(ShoulderCenter - SpineMid, SpineMid - SpineBase);

            AngleLeftThigh = calcAngle(SpineBase - ShoulderCenter, HipLeft - KneeLeft);
            AngleRightThigh = calcAngle(SpineBase - ShoulderCenter, HipRight - KneeRight);
            AngleLeftShin = calcAngle(HipLeft - KneeLeft, KneeLeft - AnkleLeft);
            AngleRightShin = calcAngle(HipRight - KneeRight, KneeRight - AnkleRight);
            

        }

  

        public void checkTableTop() {
            //Right arm flag setting
            if ((AngleRightThigh > 80 && AngleRightThigh < 110) || (AngleLeftThigh > 80 && AngleLeftThigh < 110))
            {
                thighsStraight = true;
            }

            else thighsStraight = false;

            if ((AngleRightShoulder > 170 && AngleRightShoulder < 190) || (AngleLeftShoulder > 170 && AngleRightShoulder < 190)) {
                shouldersStraight = true;
            }

            else shouldersStraight = false;


            //Checking conditions

            if (!thighsStraight)
            {

                if (AngleLeftThigh < 80 || AngleRightThigh < 80) speechString = "move your knees backward so they are directly below your hips";
                else if (AngleLeftThigh > 110 || AngleRightThigh > 110) speechString = "move your knees forward so they are directly below your hips";
            }

            else if (!shouldersStraight) {
                if (AngleLeftShoulder < 160 || AngleRightShoulder < 160) speechString = "move your shoulders backward so they are directly below your hips";
                else if (AngleLeftShoulder > 190 || AngleRightShoulder > 190) speechString = "move your shoulders forward so they are directly below your hips";
            }
            else { speechString = "perfect!"; }
        }

        public void checkTableTopBalance() {
            if ((AngleRightThigh >160 && AngleRightThigh < 190) || (AngleLeftThigh > 160 && AngleLeftThigh < 190))
            {
                thighsStraight = true;
            }

            else thighsStraight = false;

            if ((AngleRightShoulder > 170 && AngleRightShoulder < 190) || (AngleLeftShoulder > 170 && AngleRightShoulder < 190))
            {
                shouldersStraight = true;
            }

            else shouldersStraight = false;

            if (!thighsStraight)
            {

                if (AngleLeftThigh < 160 || AngleRightThigh < 160) speechString = "move your legs upwards so they are directly below your hips";
                else if (AngleLeftThigh > 190 || AngleRightThigh > 190) speechString = "move your legs downwards so they are directly below your hips";
            }

            else if (!shouldersStraight)
            {
                if (AngleLeftShoulder < 160 || AngleRightShoulder < 160) speechString = "move your shoulders backward so they are directly below your hips";
                else if (AngleLeftShoulder > 190 || AngleRightShoulder > 190) speechString = "move your shoulders forward so they are directly below your hips";
            }
            else { speechString = "perfect!"; }
        }

        public void checkCatPose() {
            //Flag setting
            if ((AngleRightThigh > 80 && AngleRightThigh < 110) || (AngleLeftThigh > 80 && AngleLeftThigh < 110))
            {
                thighsStraight = true;
            }

            else thighsStraight = false;

            if ((AngleRightShoulder > 170 && AngleRightShoulder < 190) || (AngleLeftShoulder > 170 && AngleRightShoulder < 190))
            {
                shouldersStraight = true;
            }

            else shouldersStraight = false;

            if (AngleSpine < 90)
            {
                spineCorrect = true;
            }

            else spineCorrect = false;



            //Checking conditions

            if (!spineCorrect) {
                if (AngleSpine > 60) speechString = "move your spine upwards towards the ceiling to round it more";
            }

            else if (!thighsStraight)
            {

                if (AngleLeftThigh < 80 || AngleRightThigh < 80) speechString = "move your knees backward so they are directly below your hips";
                else if (AngleLeftThigh > 110 || AngleRightThigh > 110) speechString = "move your knees forward so they are directly below your hips";
            }

            else if (!shouldersStraight)
            {
                if (AngleLeftShoulder < 160 || AngleRightShoulder < 160) speechString = "move your shoulders backward so they are directly below your hips";
                else if (AngleLeftShoulder > 190 || AngleRightShoulder > 190) speechString = "move your shoulders forward so they are directly below your hips";
            }

            else { speechString = "perfect!"; }

      }

    }
}
