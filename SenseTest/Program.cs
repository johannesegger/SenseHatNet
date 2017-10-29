using System;
using System.Reactive;
using System.Reactive.Linq;
using Sense.Led;
using Sense.Stick;
using System.Threading;
using System.Linq;
using Sense.RTIMU;

namespace SenseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLedMessage(args.Length > 0 ? args[0] : "Hello World");
            // TestLowLight();
            // TestJoystick();
            // TestRTIMULib();
        }

        private static void TestLedMessage(string text)
        {
            Sense.Led.LedMatrix.ShowMessage(text);
        }

        private static void TestLowLight()
        {
            var pixels = Sense.Led.PixelsFromText
                .Create("A")
                .SetColor(new Color(255, 255, 255));
            Sense.Led.LedMatrix.SetPixels(pixels);
            while (true)
            {
                Sense.Led.LedMatrix.SetLowLight(true);
                System.Console.WriteLine("LowLight = true");
                System.Console.WriteLine($"Gamma: {string.Join(" ", Sense.Led.LedMatrix.GetGamma().Select(v => v.ToString("X")))}");
                Thread.Sleep(2000);

                Sense.Led.LedMatrix.SetLowLight(false);
                System.Console.WriteLine("LowLight = false");
                System.Console.WriteLine($"Gamma: {string.Join(" ", Sense.Led.LedMatrix.GetGamma().Select(v => v.ToString("X")))}");
                Thread.Sleep(2000);
                
                var buffer = new byte[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 10, 10 };
                Sense.Led.LedMatrix.SetGamma(buffer);
                System.Console.WriteLine("Light = custom");
                System.Console.WriteLine($"Gamma: {string.Join(" ", Sense.Led.LedMatrix.GetGamma().Select(v => v.ToString("X")))}");
                Thread.Sleep(2000);

                Sense.Led.LedMatrix.SetLowLight(false);
                System.Console.WriteLine("LowLight = false");
                System.Console.WriteLine($"Gamma: {string.Join(" ", Sense.Led.LedMatrix.GetGamma().Select(v => v.ToString("X")))}");
                Thread.Sleep(2000);
            }
        }

        private static void TestJoystick()
        {
            Pixels Transform(Pixels pixels, JoystickKey key)
            {
                switch (key)
                {
                    case JoystickKey.Down: return pixels.Shift(1, 0);
                    case JoystickKey.Enter: return pixels;
                    case JoystickKey.Left: return pixels.Shift(0, -1);
                    case JoystickKey.Right: return pixels.Shift(0, 1);
                    case JoystickKey.Up: return pixels.Shift(-1, 0);
                    default: throw new ArgumentOutOfRangeException(nameof(key));

                }
            }
            IObservable<Func<Pixels, Pixels>> GetTransformations(JoystickEvent evt)
            {
                switch (evt.State)
                {
                    case JoystickKeyState.Press:
                        return Observable
                            .Return<Func<Pixels, Pixels>>(p => Transform(p, evt.Key));
                    case JoystickKeyState.Release:
                        return Observable
                            .Never<Func<Pixels, Pixels>>();
                    case JoystickKeyState.Hold:
                        return Observable
                            .Interval(TimeSpan.FromMilliseconds(200))
                            .Select(_ => new Func<Pixels, Pixels>(p => Transform(p, evt.Key)));
                    default: throw new ArgumentOutOfRangeException(nameof(evt.State));
                }
            }
            var initialPixels = Pixels.Empty
                .Set(new CellColor(new Cell(3, 3), new Color(0xFF, 0xFF, 0xFF)));
            var d = Joystick.Events
                .DistinctUntilChanged(p => new { p.Key, p.State })
                .Select(GetTransformations)
                .Switch()
                .Scan(initialPixels, (p, fn) => fn(p))
                .StartWith(initialPixels)
                .Subscribe(Sense.Led.LedMatrix.SetPixels);
            using (d)
            {
                Console.WriteLine("Press enter to exit ...");
                Console.ReadLine();
            }
        }

        private static void TestRTIMULib()
        {
            using (var settings = RTIMUSettings.CreateDefault())
            using (var imu = settings.CreateIMU())
            using (var pressure = settings.CreatePressure())
            using (var humidity = settings.CreateHumidity())
            {
                while (true)
                {
                    var imuData = imu.GetData();
                    Console.WriteLine($"Timestamp: {imuData.Timestamp:O}");
                    Console.WriteLine($"FusionPose: Valid: {imuData.FusionPoseValid}, Value: {imuData.FusionPose}");
                    Console.WriteLine($"FusionQPose: Valid: {imuData.FusionQPoseValid}, Value: {imuData.FusionQPose}");
                    Console.WriteLine($"Gyro: Valid: {imuData.GyroValid}, Value: {imuData.Gyro}");
                    Console.WriteLine($"Accel: Valid: {imuData.AccelValid}, Value: {imuData.Accel}");
                    Console.WriteLine($"Compass: Valid: {imuData.CompassValid}, Value: {imuData.Compass}");
                    Console.WriteLine();

                    var pressureReadResult = pressure.Read();
                    Console.WriteLine($"Pressure valid: {pressureReadResult.PressureValid}");
                    Console.WriteLine($"Pressure: {pressureReadResult.Pressure}");
                    Console.WriteLine($"Temperature valid: {pressureReadResult.TemperatureValid}");
                    Console.WriteLine($"Temperature: {pressureReadResult.Temperatur}");
                    Console.WriteLine();

                    var humidityReadResult = humidity.Read();
                    Console.WriteLine($"Humidity valid: {humidityReadResult.HumidityValid}");
                    Console.WriteLine($"Humidity: {humidityReadResult.Humidity}");
                    Console.WriteLine($"Temperature valid: {humidityReadResult.TemperatureValid}");
                    Console.WriteLine($"Temperature: {humidityReadResult.Temperatur}");

                    Console.WriteLine("===================================================");
                    Console.ReadLine();
                }
            }
        }
    }
}
