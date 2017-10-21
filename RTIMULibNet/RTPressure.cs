using System;

namespace RTIMULibNet
{
    public sealed class RTPressure : IDisposable
    {
        private bool inited;
        private readonly IntPtr pressureHandle;

        internal RTPressure(IntPtr pressureHandle)
        {
            this.pressureHandle = pressureHandle;
        }

        public RTPressureData Read()
        {
            EnsureInitialized();

            if (!NativeMethods.RTPressure_read(pressureHandle, out var result))
            {
                throw new Exception("Error while reading from pressure sensor.");
            }

            return new RTPressureData(
                result.pressureValid,
                result.pressure,
                result.temperatureValid,
                result.temperature);
        }

        public void Dispose()
        {
            NativeMethods.RTPressure_free(pressureHandle);
        }

        private void EnsureInitialized()
        {
            if (!inited && !NativeMethods.RTPressure_init(pressureHandle))
            {
                throw new Exception("Error while initializing pressure sensor.");
            }
            inited = true;
        }
    }
}