CMAKE_MINIMUM_REQUIRED(VERSION 2.8.9)

INCLUDE_DIRECTORIES(${CMAKE_CURRENT_SOURCE_DIR}/../RTIMULib/RTIMULib)

SET(RTIMULibSrcDir ${CMAKE_CURRENT_SOURCE_DIR}/../RTIMULib/RTIMULib)
# see https://github.com/RPi-Distro/RTIMULib/blob/master/RTIMULib/CMakeLists.txt
SET(LIBRTIMU_SRCS
    ${RTIMULibSrcDir}/RTFusion.cpp
    ${RTIMULibSrcDir}/RTFusionRTQF.cpp
    ${RTIMULibSrcDir}/RTMath.cpp
    ${RTIMULibSrcDir}/RTFusionKalman4.cpp
    ${RTIMULibSrcDir}/RTIMUAccelCal.cpp
    ${RTIMULibSrcDir}/RTIMUHal.cpp
    ${RTIMULibSrcDir}/RTIMUMagCal.cpp
    ${RTIMULibSrcDir}/RTIMUSettings.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMU.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUGD20M303DLHC.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUGD20HM303DLHC.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUGD20HM303D.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMULSM9DS0.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMULSM9DS1.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUMPU9150.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUMPU9250.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUBMX055.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUBNO055.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTIMUNull.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTPressure.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTPressureBMP180.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTPressureLPS25H.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTPressureMS5611.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTPressureMS5637.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTHumidity.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTHumidityHTS221.cpp
    ${RTIMULibSrcDir}/IMUDrivers/RTHumidityHTU21D.cpp
)

SET(LIBRTIMUEXT_SRCS
    ${LIBRTIMU_SRCS}
    rtimulibwrapper.cpp
    fcntl.cpp
)

ADD_LIBRARY(RTIMUExt SHARED ${LIBRTIMUEXT_SRCS})
