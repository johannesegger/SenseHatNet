using System;
using System.Runtime.InteropServices;

namespace RTIMULibNet
{
    public static class NativeMethods
    {
        private const string rtimuLibWrapperPath = "native/libRTIMULibWrapper.so";

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTIMUSettings_createDefault();

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTIMU_createIMU(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTPressure_createPressure(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTPressure_pressureInit(IntPtr pressureHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTPressure_pressureRead(
            IntPtr imuHandle,
            IntPtr pressureHandle,
            out bool pressureValid,
            out float pressure,
            out bool temperatureValid,
            out float temperature);

        [DllImport(rtimuLibWrapperPath)]
        public static extern IntPtr RTHumidity_createHumidity(IntPtr settingsHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTHumidity_humidityInit(IntPtr humidityHandle);

        [DllImport(rtimuLibWrapperPath)]
        public static extern bool RTHumidity_humidityRead(
            IntPtr imuHandle,
            IntPtr humidityHandle,
            out bool humidityValid,
            out float humidity,
            out bool temperatureValid,
            out float temperature);
    }
}
