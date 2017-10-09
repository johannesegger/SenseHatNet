using System;
using System.Reactive;
using System.Reactive.Linq;
using Sense.Led;
using Sense.Stick;

namespace SenseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // var text = args.Length > 0 ? args[0] : "Hello World";
            // LedMatrix.ShowMessage(text);

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
    }
}
