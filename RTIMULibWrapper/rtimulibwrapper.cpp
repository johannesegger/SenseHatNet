#include "../RTIMULib/RTIMULib/RTIMULib.h"

typedef struct {
    uint64_t timestamp;
    bool fusionPoseValid;
    RTFLOAT fusionPose[3];
    bool fusionQPoseValid;
    RTFLOAT fusionQPose[4];
    bool gyroValid;
    RTFLOAT gyro[3];
    bool accelValid;
    RTFLOAT accel[3];
    bool compassValid;
    RTFLOAT compass[3];
} RTIMUData;

typedef struct {
    bool pressureValid;
    RTFLOAT pressure;
    bool temperatureValid;
    RTFLOAT temperature;
} RTPressureData;

typedef struct {
    bool humidityValid;
    RTFLOAT humidity;
    bool temperatureValid;
    RTFLOAT temperature;
} RTHumidityData;

extern "C" void* RTIMUSettings_createDefault()
{
    return new RTIMUSettings();
}

extern "C" void RTIMUSettings_free(RTIMUSettings* p)
{
    delete p;
}

extern "C" void* RTIMU_create(RTIMUSettings* settings)
{
    return RTIMU::createIMU(settings);
}

extern "C" bool RTIMU_init(RTIMU* imu)
{
    return imu->IMUInit();
}

extern "C" int RTIMU_getPollInterval(RTIMU* imu)
{
    return imu->IMUGetPollInterval();
}

extern "C" bool RTIMU_read(RTIMU* imu)
{
    return imu->IMURead();
}

extern "C" void RTIMU_getData(RTIMU* imu, RTIMUData* result)
{
    RTIMU_DATA data = imu->getIMUData();

    result->timestamp = data.timestamp;
    result->fusionPoseValid = data.fusionPoseValid;
    result->fusionPose[0] = data.fusionPose.x();
    result->fusionPose[1] = data.fusionPose.y();
    result->fusionPose[2] = data.fusionPose.z();
    result->fusionQPoseValid = data.fusionQPoseValid;
    result->fusionQPose[0] = data.fusionQPose.x();
    result->fusionQPose[1] = data.fusionQPose.y();
    result->fusionQPose[2] = data.fusionQPose.z();
    result->fusionQPose[3] = data.fusionQPose.scalar();
    result->gyroValid = data.gyroValid;
    result->gyro[0] = data.gyro.x();
    result->gyro[1] = data.gyro.y();
    result->gyro[2] = data.gyro.z();
    result->accelValid = data.accelValid;
    result->accel[0] = data.accel.x();
    result->accel[1] = data.accel.y();
    result->accel[2] = data.accel.z();
    result->compassValid = data.compassValid;
    result->compass[0] = data.compass.x();
    result->compass[1] = data.compass.y();
    result->compass[2] = data.compass.z();
}

extern "C" void RTIMU_free(RTIMU* p)
{
    delete p;
}

extern "C" void* RTPressure_create(RTIMUSettings* settings)
{
    return RTPressure::createPressure(settings);
}

extern "C" bool RTPressure_init(RTPressure* pressure)
{
    return pressure->pressureInit();
}

extern "C" bool RTPressure_read(RTPressure* pressure, RTPressureData* result)
{
    RTIMU_DATA data;
    if (!pressure->pressureRead(data))
    {
        return false;
    }

    result->pressureValid = data.pressureValid;
    result->pressure = data.pressure;
    result->temperatureValid = data.temperatureValid;
    result->temperature = data.temperature;
}

extern "C" void RTPressure_free(RTPressure* p)
{
    delete p;
}

extern "C" void* RTHumidity_create(RTIMUSettings* settings)
{
    return RTHumidity::createHumidity(settings);
}

extern "C" bool RTHumidity_init(RTHumidity* humidity)
{
    return humidity->humidityInit();
}

extern "C" bool RTHumidity_read(RTHumidity* humidity, RTHumidityData* result)
{
    RTIMU_DATA data;
    if (!humidity->humidityRead(data))
    {
        return false;
    }

    result->humidityValid = data.humidityValid;
    result->humidity = data.humidity;
    result->temperatureValid = data.temperatureValid;
    result->temperature = data.temperature;
}

extern "C" void RTHumidity_free(RTHumidity* p)
{
    delete p;
}
