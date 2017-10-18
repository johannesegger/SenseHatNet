docker run `
    --rm `
    -v "${pwd}/RTIMULibWrapper:/opt/RTIMULibWrapper" `
    -v "${pwd}/SenseTest/bin/Debug/netcoreapp2.0/linux-arm/publish/native:/opt/bin" `
    rtimulib