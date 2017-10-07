using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Brushes;
using SixLabors.ImageSharp.Drawing.Pens;
using SixLabors.Primitives;

namespace SenseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = args.Length > 0 ? args[0] : "Hello World";
            var sense = Sense.Create();
            var ledPixels = LedPixelsFromText
                .Create(text)
                .SetColor(new Color(128, 128, 128));
            var maxShift = -ledPixels.Pixels.Max(p => p.Cell.Column) - 1;
            for (int i = 0; i >= maxShift; i--)
            {
                sense.SetPixels(ledPixels.Shift(0, i));
                Thread.Sleep(i == 0 ? 1000 : 100);
            }
        }
    }
}
