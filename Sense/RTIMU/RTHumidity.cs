using System;

namespace Sense.RTIMU
{
    public sealed class RTHumidity : IDisposable
    {
        private bool inited;
        private readonly IntPtr humidityHandle;

        internal RTHumidity(IntPtr humidityHandle)
        {
            this.humidityHandle = humidityHandle;
        }

        public RTHumidityData Read()
        {
            EnsureInitialized();
            
            if (!NativeMethods.RTHumidity_read(humidityHandle, out var result))
            {
                throw new Exception("Error while reading from humidity sensor.");
            }

            return new RTHumidityData(
                result.humidityValid,
                result.humidity,
                result.temperatureValid,
                result.temperature);
        }

        public void Dispose()
        {
            NativeMethods.RTHumidity_free(humidityHandle);
        }

        private void EnsureInitialized()
        {
            if (!inited && !NativeMethods.RTHumidity_init(humidityHandle))
            {
                throw new Exception("Error while initializing humidity sensor.");
            }
            inited = true;
        }
    }
}