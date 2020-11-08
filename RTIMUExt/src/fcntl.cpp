#include <fcntl.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <errno.h>

#include <stdio.h>

extern "C" int fcntl_ioctl(const char* path, unsigned long request, unsigned long arg)
{
    int fd = open(path, O_RDWR);
    int ioctlErrorCode = ioctl(fd, request, arg);
    int closeErrorCode = close(fd);
    return ioctlErrorCode == -1 ? errno : ioctlErrorCode;
}