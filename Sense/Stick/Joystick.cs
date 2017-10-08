using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Sense.Stick
{
    public static class Joystick
    {
        private const string SENSE_HAT_EVDEV_NAME = "Raspberry Pi Sense HAT Joystick";
        private const int EV_KEY = 0x01;

        private static string GetEventDevicePath()
        {
            bool IsSenseHatEvDev(string dir)
            {
                var name = File.ReadAllText(Path.Combine(dir, "device", "name")).Trim();
                return name == SENSE_HAT_EVDEV_NAME;
            }

            string GetDevice(string file)
            {
                var frameBufferDevice = Path.Combine("/dev", "input", Path.GetFileName(file));
                return File.Exists(frameBufferDevice)
                    ? frameBufferDevice
                    : null;
            }

            return Directory
                .EnumerateFileSystemEntries("/sys/class/input/", "event*")
                .Where(IsSenseHatEvDev)
                .Select(GetDevice)
                .FirstOrDefault(p => p != null)
                ?? throw new Exception("Cannot detect Sense HAT joystick device");
        }

        private static Lazy<string> eventDevicePath = new Lazy<string>(GetEventDevicePath);

        private static string EventDevicePath => eventDevicePath.Value;

        public static IObservable<JoystickEvent> Events
        {
            get
            {
                return Observable.Create<JoystickEvent>(async (obs, ct) =>
                {
                    var stream = File.OpenRead(EventDevicePath);
                    
                    try
                    {
                        while (true)
                        {
                            var package = new byte[16];
                            var length = 0;
                            while (length < package.Length)
                            {
                                length = await stream.ReadAsync(package, length, package.Length - length, ct);
                                System.Console.WriteLine($"Read {length} bytes");
                            }

                            var timeSec = BitConverter.ToInt32(package, 0);
                            var timeMicroSec = BitConverter.ToInt32(package, 4);
                            var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                .AddSeconds(timeSec)
                                .AddMilliseconds(timeMicroSec / 1000);
                            var type = BitConverter.ToInt16(package, 8);
                            var code = BitConverter.ToInt16(package, 10);
                            var value = BitConverter.ToInt32(package, 12);
                            if(type == EV_KEY)
                            {
                                obs.OnNext(new JoystickEvent(time, (JoystickKey)code, (JoystickKeyState)value));
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }

                    return stream;
                });
            }
        }
    }
}