#include "../RTIMULib/RTIMULib/RTIMULib.h"

extern "C" void* RTIMUSettings_createDefault()
{
    return new RTIMUSettings();
}

extern "C" void* RTIMU_createIMU(RTIMUSettings* settings)
{
    return RTIMU::createIMU(settings);
}

extern "C" void* RTPressure_createPressure(RTIMUSettings* settings)
{
    return RTPressure::createPressure(settings);
}

extern "C" bool RTPressure_pressureInit(RTPressure* pressure)
{
    return pressure->pressureInit();
}

extern "C" bool RTPressure_pressureRead(
    RTIMU* imu,
    RTPressure* pressure,
    bool* pressureValid,
    RTFLOAT* pressureValue,
    bool* temperatureValid,
    RTFLOAT* temperatureValue)
{
    RTIMU_DATA data = imu->getIMUData();
    if (!pressure->pressureRead(data))
    {
        return false;
    }
    *pressureValid = data.pressureValid;
    *pressureValue = data.pressure;
    *temperatureValid = data.temperatureValid;
    *temperatureValue = data.temperature;
}

extern "C" void* RTHumidity_createHumidity(RTIMUSettings* settings)
{
    return RTHumidity::createHumidity(settings);
}

extern "C" bool RTHumidity_humidityInit(RTHumidity* humidity)
{
    return humidity->humidityInit();
}

extern "C" bool RTHumidity_humidityRead(
    RTIMU* imu,
    RTHumidity* humidity,
    bool* humidityValid,
    RTFLOAT* humidityValue,
    bool* temperatureValid,
    RTFLOAT* temperatureValue)
{
    RTIMU_DATA data = imu->getIMUData();
    if (!humidity->humidityRead(data))
    {
        return false;
    }
    *humidityValid = data.humidityValid;
    *humidityValue = data.humidity;
    *temperatureValid = data.temperatureValid;
    *temperatureValue = data.temperature;
}
