using System;
using System.Runtime.InteropServices;

namespace RTIMULibNet
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

        private const string rtimuLibWrapperPath = "native/libRTIMULibWrapper.so";

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTIMUSettings_createDefault();

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTIMUSettings_free(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTIMU_create(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTIMU_init(IntPtr imuHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern int RTIMU_getPollInterval(IntPtr imuHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTIMU_read(IntPtr imuHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern void RTIMU_getData(IntPtr imuHandle, out RTIMUData data);

        [DllImport(rtimuLibWrapperPath)]
        public static extern void RTIMU_free(IntPtr imuHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTPressure_create(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTPressure_init(IntPtr pressureHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTPressure_read(
            IntPtr pressureHandle,
            out RTPressureData data);

        [DllImport(rtimuLibWrapperPath)]
        public static extern void RTPressure_free(IntPtr pressureHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTHumidity_create(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTHumidity_init(IntPtr humidityHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTHumidity_read(
            IntPtr humidityHandle,
            out RTHumidityData data);

        [DllImport(rtimuLibWrapperPath)]
        public static extern void RTHumidity_free(IntPtr humidityHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern int fcntl_ioctl(string path, uint request, IntPtr arg);
    }
}
