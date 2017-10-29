using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sense.RTIMU;
using Sense.Utils;

namespace Sense.Led
{
    public static class LedMatrix
    {
        private const string SENSE_HAT_FRAMEBUFFER_NAME = "RPi-Sense FB";

        private const int Rows = 8;
        private const int Columns = 8;
        private static readonly IEnumerable<int> rowRange = Enumerable.Range(0, Rows);
        private static readonly IEnumerable<int> columnRange = Enumerable.Range(0, Columns);

        private static string GetFrameBufferDevicePath()
        {
            bool IsSenseFrameBuffer(string file)
            {
                var nameFile = Path.Combine(file, "name");
                if (!File.Exists(nameFile))
                {
                    return false;
                }

                var name = File.ReadAllText(nameFile).Trim();
                return name == SENSE_HAT_FRAMEBUFFER_NAME;
            }

            string GetDevice(string file)
            {
                var frameBufferDevice = Path.Combine("/dev", Path.GetFileName(file));
                return File.Exists(frameBufferDevice)
                    ? frameBufferDevice
                    : null;
            }

            return Directory
                .EnumerateFileSystemEntries("/sys/class/graphics/", "fb*")
                .Where(IsSenseFrameBuffer)
                .Select(GetDevice)
                .FirstOrDefault(p => p != null)
                ?? throw new Exception("Cannot detect Sense HAT LED matrix device");
        }

        private static readonly Lazy<string> frameBufferDevicePath =
            new Lazy<string>(GetFrameBufferDevicePath);

        private static string FrameBufferDevicePath => frameBufferDevicePath.Value;

        public static void ShowMessage(string text)
        {
            var pixels = PixelsFromText.Create(text);
            var maxShift = -pixels.Items.Max(p => p.Cell.Column) - 1;
            for (int i = 0; i >= maxShift; i--)
            {
                SetPixels(pixels.Shift(0, i));
                Thread.Sleep(TimeSpan.FromMilliseconds(i == 0 ? 1000 : 50));
            }
        }

        public static void SetPixels(Pixels pixels)
        {
            byte[] Pack(Color color)
            {
                var r = (color.Red >> 3) & 0x1F;
                var g = (color.Green >> 2) & 0x3F;
                var b = (color.Blue >> 3) & 0x1F;
                var bits16 = (short)((r << 11) + (g << 5) + b);
                return BitConverter.GetBytes(bits16);
            }

            var content = GetColors(pixels)
                .SelectMany(Pack)
                .ToArray();

            File.WriteAllBytes(FrameBufferDevicePath, content);
        }

        private static IEnumerable<Color> GetColors(Pixels pixels)
        {
            var equalityComparer = EqualityComparer
                .Create((CellColor p) => new { p.Cell.Row, p.Cell.Column });

            var defaultColor = new Color(0, 0, 0);
            return pixels.Items
                .Where(p => rowRange.Contains(p.Cell.Row) && columnRange.Contains(p.Cell.Column))
                .Union(
                    rowRange
                        .SelectMany(row => columnRange
                            .Select(column => new CellColor(new Cell(row, column), defaultColor))
                        ),
                    equalityComparer
                )
                .OrderBy(p => p.Cell.Row)
                .ThenBy(p => p.Cell.Column)
                .Select(p => p.Color);
        }

        public static void SetLowLight(bool value)
        {
            const uint SENSE_HAT_FB_FBIORESET_GAMMA = 61698u;
            const uint SENSE_HAT_FB_GAMMA_DEFAULT = 0u;
            const uint SENSE_HAT_FB_GAMMA_LOW = 1u;

            var errorCode = NativeMethods.fcntl_ioctl(
                FrameBufferDevicePath,
                SENSE_HAT_FB_FBIORESET_GAMMA,
                new IntPtr(value ? SENSE_HAT_FB_GAMMA_LOW : SENSE_HAT_FB_GAMMA_DEFAULT));

            if (errorCode < 0)
            {
                throw new Exception($"Can't set gamma value. Error code {errorCode} (see http://man7.org/linux/man-pages/man3/errno.3.html)");
            }
        }

        public static byte[] GetGamma()
        {
            const uint SENSE_HAT_FB_FBIOGET_GAMMA = 61696u;
            var buffer = new byte[32];

            var errorCode = DoWithBufferPointer(
                buffer,
                ptr => NativeMethods.fcntl_ioctl(
                    FrameBufferDevicePath,
                    SENSE_HAT_FB_FBIOGET_GAMMA,
                    ptr));

            if (errorCode < 0)
            {
                throw new Exception($"Can't get gamma value. Error code {errorCode} (see http://man7.org/linux/man-pages/man3/errno.3.html)");
            }
            
            return buffer;
        }

        public static void SetGamma(byte[] buffer)
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
                    FrameBufferDevicePath,
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
