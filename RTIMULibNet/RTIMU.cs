using System;
using System.Numerics;
using System.Threading;

namespace RTIMULibNet
{
    public sealed class RTIMU : IDisposable
    {
        private bool inited;
        private TimeSpan pollInterval;
        private readonly IntPtr imuHandle;

        internal RTIMU(IntPtr imuHandle)
        {
            this.imuHandle = imuHandle;
        }

        public bool Read()
        {
            EnsureInitialized();

            var attempts = 0;
            var success = false;

            while (!success && attempts < 3)
            {
                if (attempts++ > 0)
                {
                    Thread.Sleep(pollInterval);
                }
                success = NativeMethods.RTIMU_read(imuHandle);
            }

            return success;
        }

        public RTIMUData GetData()
        {
            if(!Read())
            {
                throw new Exception("Can't read from IMU");
            }

            NativeMethods.RTIMU_getData(imuHandle, out var data);
            return new RTIMUData(
                new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(data.timestamp / 1000)),
                data.fusionPoseValid,
                new Vector3(data.fusionPose[0], data.fusionPose[1], data.fusionPose[2]),
                data.fusionQPoseValid,
                new Quaternion(data.fusionQPose[0], data.fusionQPose[1], data.fusionQPose[2], data.fusionQPose[3]),
                data.gyroValid,
                new Vector3(data.gyro[0], data.gyro[1], data.gyro[2]),
                data.accelValid,
                new Vector3(data.accel[0], data.accel[1], data.accel[2]),
                data.compassValid,
                new Vector3(data.compass[0], data.compass[1], data.compass[2]));
        }

        public void Dispose()
        {
            NativeMethods.RTIMU_free(imuHandle);
        }

        private void EnsureInitialized()
        {
            if (!inited && !NativeMethods.RTIMU_init(imuHandle))
            {
                throw new Exception("Error while initializing IMU.");
            }
            inited = true;
            pollInterval = GetPollInterval();
        }

        private TimeSpan GetPollInterval()
        {
            var pollInterval = NativeMethods.RTIMU_getPollInterval(imuHandle);
            return TimeSpan.FromMilliseconds(pollInterval);
        }
    }
}