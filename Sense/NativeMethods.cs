using System;
using System.Runtime.InteropServices;

namespace Sense
{
    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RTIMUData
        {
            public ulong timestamp;
            public bool fusionPoseValid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] fusionPose;
            public bool fusionQPoseValid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] fusionQPose;
            public bool gyroValid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] gyro;
            public bool accelValid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] accel;
            public bool compassValid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] compass;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RTPressureData
        {
            public bool pressureValid;
            public float pressure;
            public bool temperatureValid;
            public float temperature;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RTHumidityData
        {
            public bool humidityValid;
            public float humidity;
            public bool temperatureValid;
            public float temperature;
        }

        private const string rtimuExtPath = "libRTIMUExt.so";

        [DllImport(rtimuExtPath)]
        public static extern IntPtr RTIMUSettings_createDefault();

        [DllImport(rtimuExtPath)]
        public static extern IntPtr RTIMUSettings_free(IntPtr settingsHandle);

        [DllImport(rtimuExtPath)]
        public static extern IntPtr RTIMU_create(IntPtr settingsHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTIMU_init(IntPtr imuHandle);

        [DllImport(rtimuExtPath)]
        public static extern int RTIMU_getPollInterval(IntPtr imuHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTIMU_read(IntPtr imuHandle);

        [DllImport(rtimuExtPath)]
        public static extern void RTIMU_getData(IntPtr imuHandle, out RTIMUData data);

        [DllImport(rtimuExtPath)]
        public static extern void RTIMU_free(IntPtr imuHandle);

        [DllImport(rtimuExtPath)]
        public static extern IntPtr RTPressure_create(IntPtr settingsHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTPressure_init(IntPtr pressureHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTPressure_read(
            IntPtr pressureHandle,
            out RTPressureData data);

        [DllImport(rtimuExtPath)]
        public static extern void RTPressure_free(IntPtr pressureHandle);

        [DllImport(rtimuExtPath)]
        public static extern IntPtr RTHumidity_create(IntPtr settingsHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTHumidity_init(IntPtr humidityHandle);

        [DllImport(rtimuExtPath)]
        public static extern bool RTHumidity_read(
            IntPtr humidityHandle,
            out RTHumidityData data);

        [DllImport(rtimuExtPath)]
        public static extern void RTHumidity_free(IntPtr humidityHandle);

        [DllImport(rtimuExtPath)]
        public static extern int fcntl_ioctl(string path, uint request, IntPtr arg);
    }
}
