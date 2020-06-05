using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sense.RTIMU
{
    public sealed class RTIMUSettings : IDisposable
    {
        public static RTIMUSettings CreateDefault()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "libRTIMULib.so.7");
            var handle = NativeLibrary.Load(path);
            if (handle == IntPtr.Zero)
            {
                throw new DllNotFoundException($"Unable to load shared library '{Path.GetFileName(path)}'.");
            }
            var settingsHandle = NativeMethods.RTIMUSettings_createDefault();
            return new RTIMUSettings(settingsHandle);
        }

        private readonly IntPtr settingsHandle;

        private RTIMUSettings(IntPtr settingsHandle)
        {
            this.settingsHandle = settingsHandle;
        }

        public RTIMU CreateIMU()
        {
            var handle = NativeMethods.RTIMU_create(settingsHandle);
            return new RTIMU(handle);
        }

        public RTPressure CreatePressure()
        {
            var handle = NativeMethods.RTPressure_create(settingsHandle);
            return new RTPressure(handle);
        }

        public RTHumidity CreateHumidity()
        {
            var handle = NativeMethods.RTHumidity_create(settingsHandle);
            return new RTHumidity(handle);
        }

        public void Dispose()
        {
            NativeMethods.RTIMUSettings_free(settingsHandle);
        }
    }
}