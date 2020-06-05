FROM debian:10

RUN	apt-get update && \
    apt-get install -y git cmake g++-arm-linux-gnueabihf g++-aarch64-linux-gnu

WORKDIR /opt

RUN git clone https://github.com/RPi-Distro/RTIMULib.git

WORKDIR /opt/build

VOLUME /opt/RTIMULibWrapper
VOLUME /opt/bin

CMD cmake ../RTIMULibWrapper && \
    make && \
    cp RTIMULib/libRTIMULib.so.7 /opt/bin && \
    cp libRTIMULibWrapper.so /opt/bin
