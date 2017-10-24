using System;
using System.Linq;

namespace Sense.RTIMU
{
    public static class LedMatrix
    {
        public static void SetLowLight(string frameBufferDevicePath, bool value)
        {
            const uint SENSE_HAT_FB_FBIORESET_GAMMA = 61698u;
            const uint SENSE_HAT_FB_GAMMA_DEFAULT = 0u;
            const uint SENSE_HAT_FB_GAMMA_LOW = 1u;

            var errorCode = NativeMethods.fcntl_ioctl(
                frameBufferDevicePath,
                SENSE_HAT_FB_FBIORESET_GAMMA,
                new IntPtr(value ? SENSE_HAT_FB_GAMMA_LOW : SENSE_HAT_FB_GAMMA_DEFAULT));

            if (errorCode < 0)
            {
                throw new Exception($"Can't set gamma value. Error code {errorCode} (see http://man7.org/linux/man-pages/man3/errno.3.html)");
            }
        }

        public static byte[] GetGamma(string frameBufferDevicePath)
        {
            const uint SENSE_HAT_FB_FBIOGET_GAMMA = 61696u;
            var buffer = new byte[32];

            var errorCode = DoWithBufferPointer(
                buffer,
                ptr => NativeMethods.fcntl_ioctl(
                    frameBufferDevicePath,
                    SENSE_HAT_FB_FBIOGET_GAMMA,
                    ptr));

            if (errorCode < 0)
            {
                throw new Exception($"Can't get gamma value. Error code {errorCode} (see http://man7.org/linux/man-pages/man3/errno.3.html)");
            }
            
            return buffer;
        }

        public static void SetGamma(string frameBufferDevicePath, byte[] buffer)
        {
            const uint SENSE_HAT_FB_FBIOSET_GAMMA = 61697u;

            if (buffer.Length != 32)
            {
                throw new Exception("Gamma buffer must be of length 32");
            }

            if (!buffer.All(v => v >= 0 && v < 32))
            {
                throw new Exception("Gamma values must be bewteen 0 incl. and 31 incl.");
            }

            var errorCode = DoWithBufferPointer(
                buffer,
                ptr => NativeMethods.fcntl_ioctl(
                    frameBufferDevicePath,
                    SENSE_HAT_FB_FBIOSET_GAMMA,
                    ptr));

            if (errorCode < 0)
            {
                throw new Exception($"Can't set gamma value. Error code {errorCode} (see http://man7.org/linux/man-pages/man3/errno.3.html)");
            }
        }

        private static T DoWithBufferPointer<T>(byte[] buffer, Func<IntPtr, T> action)
        {
            unsafe
            {
                fixed(byte* pByte = buffer)
                {
                    var bufferPtr = new IntPtr((void *) pByte);
                    return action(bufferPtr);
                }
            }
        }
    }
}