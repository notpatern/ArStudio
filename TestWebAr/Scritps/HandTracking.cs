
using StereoKit;
using System;
using System.Timers;

namespace TestWebAr.Scritps
{
    public class HandTracking
    {
        Timer handTrackingTimer = new Timer(0020); // hard coded fixed amount

        Vec3 headForward;
        Vec3 headPosition;

        Vec3 rightHandPosition;
        Vec3 leftHandPosition;

        Vec3 rightHandRelativePosition;
        Vec3 leftHandRelativePosition;

        double radianRightHandVerticalAngle;
        double radianLeftHandVerticalAngle;

        double radianRightHandHorizontalAngle;
        double radianLeftHandHorizontalAngle;

        double degreeRightHandVerticalAngle;
        double degreeLeftHandVerticalAngle; 

        double degreeRightHandHorizontalAngle;
        double degreeLeftHandHorizontalAngle; 

        Vec2 rightHandAnglesDegree = new Vec2();
        Vec2 leftHandAnglesDegree = new Vec2();

        Vec2 rightHandAnglesRadian = new Vec2();
        Vec2 leftHandAnglesRadian = new Vec2();

        Vec2 rightHandVelocity = new Vec2();
        Vec2 leftHandVelocity = new Vec2();

        public HandTracking() 
        {
            Init();
        }

        private void Init()
        {
            handTrackingTimer.Elapsed += UpdateHandTracking;
            handTrackingTimer.Elapsed += UpdateHandTrackingChecks;
            handTrackingTimer.Enabled = true;
            handTrackingTimer.Start();
        }

        private void UpdateHandTracking(object sender, ElapsedEventArgs e)
        {
            GetHeadPositions();
            CalculateHandsPosition();
            CalculateRadianAngles();
            CalculateDegreeAngles();
        }

        #region Info
        private void GetHeadPositions()
        {
            headForward = Input.Head.Forward;
            headPosition = Input.Head.position;
        }

        private void CalculateHandsPosition()
        {
            rightHandPosition = Input.Hand(Handed.Right).palm.position;
            leftHandPosition = Input.Hand(Handed.Left).palm.position;

            rightHandRelativePosition = rightHandPosition - headPosition;
            leftHandRelativePosition = leftHandPosition - headPosition;
        }

        private void CalculateRadianAngles()
        {
            radianRightHandVerticalAngle = Math.Atan2(rightHandRelativePosition.z, rightHandRelativePosition.y) - Math.Atan2(headForward.z, headForward.y);
            radianLeftHandVerticalAngle = Math.Atan2(leftHandRelativePosition.z, leftHandRelativePosition.y) - Math.Atan2(headForward.z, headForward.y);

            radianRightHandHorizontalAngle = Math.Atan2(rightHandRelativePosition.z, rightHandRelativePosition.x) - Math.Atan2(headForward.z, headForward.x);
            radianLeftHandHorizontalAngle = Math.Atan2(leftHandRelativePosition.z, leftHandRelativePosition.x) - Math.Atan2(headForward.z, headForward.x);

            rightHandAnglesRadian.x = (float)radianRightHandHorizontalAngle;
            rightHandAnglesRadian.y = (float)radianRightHandVerticalAngle;
            leftHandAnglesRadian.x = (float)radianLeftHandHorizontalAngle;
            leftHandAnglesRadian.y = (float)radianLeftHandVerticalAngle;
        }

        private void CalculateDegreeAngles()
        {
            degreeRightHandVerticalAngle = (180 / Math.PI) * radianRightHandVerticalAngle;
            degreeLeftHandVerticalAngle = (180 / Math.PI) * radianLeftHandVerticalAngle;

            degreeRightHandHorizontalAngle = (180 / Math.PI) * radianRightHandHorizontalAngle;
            degreeLeftHandHorizontalAngle = (180 / Math.PI) * radianLeftHandHorizontalAngle;

            rightHandAnglesDegree.x = (float)degreeRightHandHorizontalAngle;
            rightHandAnglesDegree.y = (float)degreeRightHandVerticalAngle;
            leftHandAnglesDegree.x = (float)degreeLeftHandHorizontalAngle;
            leftHandAnglesDegree.y = (float)degreeLeftHandVerticalAngle;
        }
        #endregion

        private void UpdateHandTrackingChecks(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Xangle : {degreeRightHandHorizontalAngle}, Yangle : {degreeRightHandVerticalAngle}");
            if (rightHandAnglesDegree.x >= 25 && rightHandAnglesDegree.y >= 25)
            {
            }
        }
    }
}
