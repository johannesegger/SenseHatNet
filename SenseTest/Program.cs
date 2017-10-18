using System;
using System.Reactive;
using System.Reactive.Linq;
using Sense.Led;
using Sense.Stick;
using RTIMULibNet;

namespace SenseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestLedMessage(args.Length > 0 ? args[0] : "Hello World");
            // TestJoystick();
            TestRTIMULib();
        }

        private static void TestLedMessage(string text)
        {
            LedMatrix.ShowMessage(text);
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
                    case JoystickKeyState.Press: return Observable
                        .Return<Func<Pixels, Pixels>>(p => Transform(p, evt.Key));
                    case JoystickKeyState.Release: return Observable
                        .Never<Func<Pixels, Pixels>>();
                    case JoystickKeyState.Hold: return Observable
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
                .Subscribe(LedMatrix.SetPixels);
            using (d)
            {
                Console.WriteLine("Press enter to exit ...");
                Console.ReadLine();
            }
        }

        private static void TestRTIMULib()
        {
            var settingsHandle = NativeMethods.RTIMUSettings_createDefault();
            var imuHandle = NativeMethods.RTIMU_createIMU(settingsHandle);

            var pressureHandle = NativeMethods.RTPressure_createPressure(settingsHandle);
            var pressureInitialized = NativeMethods.RTPressure_pressureInit(pressureHandle);
            System.Console.WriteLine($"Pressure initialized: {pressureInitialized}");

            var humidityHandle = NativeMethods.RTHumidity_createHumidity(settingsHandle);
            var humidityInitialized = NativeMethods.RTHumidity_humidityInit(humidityHandle);
            System.Console.WriteLine($"Humidity initialized: {humidityInitialized}");
            while (true)
            {
                var pressureReadSuccess = NativeMethods.RTPressure_pressureRead(
                    imuHandle,
                    pressureHandle,
                    out bool pressureValid,
                    out float pressure,
                    out bool temperatureFromPressureValid,
                    out float temperatureFromPressure);
                Console.WriteLine($"Pressure read success: {pressureReadSuccess}");
                Console.WriteLine($"Pressure valid: {pressureValid}");
                Console.WriteLine($"Pressure: {pressure}");
                Console.WriteLine($"Temperature valid: {temperatureFromPressureValid}");
                Console.WriteLine($"Temperature: {temperatureFromPressure}");
                Console.WriteLine();

                var humidityReadSuccess = NativeMethods.RTHumidity_humidityRead(
                    imuHandle,
                    humidityHandle,
                    out bool humidityValid,
                    out float humidity,
                    out bool temperatureFromHumidityValid,
                    out float temperatureFromHumidity);
                Console.WriteLine($"Humidity read success: {humidityReadSuccess}");
                Console.WriteLine($"Humidity valid: {humidityValid}");
                Console.WriteLine($"Humidity: {humidity}");
                Console.WriteLine($"Temperature valid: {temperatureFromHumidityValid}");
                Console.WriteLine($"Temperature: {temperatureFromHumidity}");
                Console.WriteLine("===================================================");
                Console.ReadLine();
            }
        }
    }
}
