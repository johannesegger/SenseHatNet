using System;
using System.Numerics;

namespace RTIMULibNet
{
    public class RTIMUData
    {
        public DateTime Timestamp { get; }
        public bool FusionPoseValid { get; }
        public Vector3 FusionPose { get; }
        public bool FusionQPoseValid { get; }
        public Quaternion FusionQPose { get; }
        public bool GyroValid { get; }
        public Vector3 Gyro { get; }
        public bool AccelValid { get; }
        public Vector3 Accel { get; }
        public bool CompassValid { get; }
        public Vector3 Compass { get; }

        public RTIMUData(
            DateTime timestamp,
            bool fusionPoseValid,
            Vector3 fusionPose,
            bool fusionQPoseValid,
            Quaternion fusionQPose,
            bool gyroValid,
            Vector3 gyro,
            bool accelValid,
            Vector3 accel,
            bool compassValid,
            Vector3 compass)
        {
            Timestamp = timestamp;
            FusionPoseValid = fusionPoseValid;
            FusionPose = fusionPose;
            FusionQPoseValid = fusionQPoseValid;
            FusionQPose = fusionQPose;
            GyroValid = gyroValid;
            Gyro = gyro;
            AccelValid = accelValid;
            Accel = accel;
            CompassValid = compassValid;
            Compass = compass;
        }
    }
}