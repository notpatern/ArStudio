
using StereoKit;
using System;
using System.Timers;

namespace Scritps.Services
{
    public struct HandData {
        public Hand hand;
        public Vec3 position;
        public Vec3 relativePosition;
        public Vec2 angleDegree;
        public Vec2 angleRadian;
        public Vec3 velocity;
        public Vec3 relativeVelocity;
    }
    public class HandTracking
    {
        double fixedDeltaTime = 0.02;
        System.Timers.Timer handTrackingTimer = new System.Timers.Timer(0020); // hard coded fixed amount

        Vec3 headForward;
        Vec3 headPosition;

        Vec3 dirtyRightHandPosition = new Vec3();
        Vec3 dirtyLeftHandPosition = new Vec3();

        Vec3 dirtyRightHandRelativePosition = new Vec3();
        Vec3 dirtyLeftHandRelativePosition = new Vec3();

        HandData rightHandData = new HandData();
        HandData leftHandData = new HandData();

        public Action RightFastHand;
        public Action LeftHandDownFast;

        private float inputBuffer;

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
            rightHandData.hand = Input.Hand(Handed.Right);
            leftHandData.hand = Input.Hand(Handed.Left);

            rightHandData.position = rightHandData.hand.palm.position;
            leftHandData.position = leftHandData.hand.palm.position;

            rightHandData.relativePosition = rightHandData.position- headPosition;
            leftHandData.relativePosition = leftHandData.position - headPosition;

            rightHandData.velocity = CalculateVelocity(dirtyRightHandPosition, rightHandData.position);
            leftHandData.velocity = CalculateVelocity(dirtyLeftHandPosition, leftHandData.position);
            rightHandData.relativeVelocity = CalculateVelocity(dirtyRightHandRelativePosition, rightHandData.relativePosition);
            leftHandData.relativeVelocity = CalculateVelocity(dirtyLeftHandRelativePosition, leftHandData.relativePosition);

            dirtyRightHandPosition = rightHandData.position;
            dirtyLeftHandPosition = leftHandData.position;
            dirtyRightHandRelativePosition = rightHandData.relativePosition;
            dirtyLeftHandRelativePosition = leftHandData.relativePosition;
        }

        private Vec3 CalculateVelocity(Vec3 pointA, Vec3 pointB)
        {
            Vec3 velocity = new Vec3();

            if (Vec3.Distance(pointA, pointB) == 0)
            {
                return new Vec3();
            }

            velocity = (pointB - pointA) * (float)(1 / fixedDeltaTime) * 100;

            return velocity;
        }

        private void CalculateRadianAngles()
        {
            leftHandData.angleRadian.y = (float)((float)Math.Atan2(leftHandData.relativePosition.z, leftHandData.relativePosition.y) - Math.Atan2(headForward.z, headForward.y));
            rightHandData.angleRadian.y = (float)((float)Math.Atan2(rightHandData.relativePosition.z, rightHandData.relativePosition.y) - Math.Atan2(headForward.z, headForward.y));

            leftHandData.angleRadian.x = (float)((float)Math.Atan2(leftHandData.relativePosition.z, leftHandData.relativePosition.x) - Math.Atan2(headForward.z, headForward.x));
            rightHandData.angleRadian.x = (float)((float)Math.Atan2(rightHandData.relativePosition.z, rightHandData.relativePosition.x) - Math.Atan2(headForward.z, headForward.x));
        }

        private void CalculateDegreeAngles()
        {
            rightHandData.angleDegree.y = (float)(180 / Math.PI * rightHandData.angleRadian.y);
            leftHandData.angleDegree.y = (float)(180 / Math.PI * leftHandData.angleRadian.y);

            rightHandData.angleDegree.x = (float)(180 / Math.PI * rightHandData.angleRadian.x);
            leftHandData.angleDegree.x = (float)(180 / Math.PI * leftHandData.angleRadian.x);
        }
        #endregion

        private void UpdateHandTrackingChecks(object sender, ElapsedEventArgs e)
        {
            if (inputBuffer < 0.3f) {
                inputBuffer += (float)fixedDeltaTime;
                return;
            }

            // log commands
            if (Vec3.Distance(leftHandData.position, rightHandData.position) <= 0.07f)
            {
                if (leftHandData.velocity.x >= 100 && rightHandData.velocity.x <= 100)
                {
                    if (leftHandData.hand.IsPinched && rightHandData.hand.IsPinched)
                    {
                        SkyLogEvents.CancelOpenLog.Invoke();
                        inputBuffer = 0;
                    }
                }
            }

            if (Vec3.Dot(leftHandData.hand.palm.Forward, headForward) <= -0.60)
            {
                if (rightHandData.velocity.x >= 120 && rightHandData.angleDegree.x >= 25)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.NewLog.Invoke();
                        inputBuffer = 0;
                    }
                }

                if (rightHandData.velocity.x <= -120 && rightHandData.angleDegree.x <= -25)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.CloseLog.Invoke();
                        inputBuffer = 0;
                    }
                }

                if (rightHandData.velocity.y >= 120 && rightHandData.angleDegree.y >= 20)
                {
                    if (rightHandData.hand.IsPinched && (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, headForward.Normalized) >= 0.9)) {
                        SkyLogEvents.CancelLog.Invoke();
                        inputBuffer = 0;
                    }
                }
            }

            // player commands
            if (leftHandData.hand.IsGripped && leftHandData.angleDegree.y > -10) {
                if (rightHandData.velocity.y <= -100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.Pause.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.y >= 100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.Play.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.x >= 100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.RightFrame.Invoke();
                    inputBuffer = 0;
                }

                if (rightHandData.velocity.x <= -100 && rightHandData.angleDegree.x >= 30) {
                    SkyLogEvents.LeftFrame.Invoke();
                    inputBuffer = 0;
                }
            }

            if (Vec3.Dot(rightHandData.hand.palm.Forward.Normalized, leftHandData.hand.palm.Forward.Normalized) <= -0.60) {
                if (rightHandData.velocity.y >= 120) {
                    SkyLogEvents.BackToLive.Invoke();
                    inputBuffer = 0;
                }
            }

            if (leftHandData.velocity.x <= -100 && rightHandData.velocity.x >= 100) {
                if (leftHandData.hand.IsPinched && rightHandData.hand.IsPinched) {
                    SkyLogEvents.ClearMarkers.Invoke();
                    inputBuffer = 0;
                }
            }

            if (leftHandData.hand.IsGripped) {
                SkyLogEvents.CopyPlayerTimeCode.Invoke();
                // no need for input buffer as it sends space repetively so that the use can copy.
            }

            if (leftHandData.velocity.x >= 120 && leftHandData.angleDegree.x <= -30)
            {
                SkyLogEvents.Tab.Invoke();
                inputBuffer = 0;
            }
        }
    }
}
