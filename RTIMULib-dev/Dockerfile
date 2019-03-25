FROM debian

RUN	apt-get update && \
    apt-get install -y git cmake

ENV CXX=arm-linux-gnueabihf-g++

WORKDIR /opt

RUN	git clone https://github.com/raspberrypi/tools.git && \
    ln -sf /opt/tools/arm-bcm2708/arm-rpi-4.9.3-linux-gnueabihf/bin/* /usr/local/bin/

RUN git clone https://github.com/RPi-Distro/RTIMULib.git

WORKDIR /opt/build

VOLUME /opt/RTIMULibWrapper
VOLUME /opt/bin

CMD cmake ../RTIMULibWrapper && \
    make -j4 && \
    cp RTIMULib/libRTIMULib.so.7 /opt/bin && \
    cp libRTIMULibWrapper.so /opt/bin
