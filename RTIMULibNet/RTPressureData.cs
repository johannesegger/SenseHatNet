namespace RTIMULibNet
{
    public sealed class RTPressureData
    {
        public RTPressureData(bool pressureValid, float pressure, bool temperatureValid, float temperatur)
        {
            PressureValid = pressureValid;
            Pressure = pressure;
            TemperatureValid = temperatureValid;
            Temperatur = temperatur;
        }

        public bool PressureValid { get; }
        public float Pressure { get; }
        public bool TemperatureValid { get; }
        public float Temperatur { get; }
    }
}