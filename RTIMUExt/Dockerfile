FROM debian:10

RUN	apt-get update && \
    apt-get install -y git cmake g++-arm-linux-gnueabihf g++-aarch64-linux-gnu

WORKDIR /opt

RUN git clone https://github.com/RPi-Distro/RTIMULib.git

WORKDIR /opt/build

VOLUME /opt/RTIMUExt
VOLUME /opt/bin

CMD cmake ../RTIMUExt && \
    make && \
    cp libRTIMUExt.so /opt/bin
