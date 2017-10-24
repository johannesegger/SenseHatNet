namespace Sense.RTIMU
{
    public sealed class RTHumidityData
    {
        public RTHumidityData(bool humidityValid, float humidity, bool temperatureValid, float temperatur)
        {
            HumidityValid = humidityValid;
            Humidity = humidity;
            TemperatureValid = temperatureValid;
            Temperatur = temperatur;
        }

        public bool HumidityValid { get; }
        public float Humidity { get; }
        public bool TemperatureValid { get; }
        public float Temperatur { get; }
    }
}