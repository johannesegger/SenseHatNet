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
            var ledPixels = LedPixelsFromText.Create(text).SetColor(new Color(255, 255, 255));
            var maxShift = -ledPixels.Pixels.Max(p => p.Cell.Column) - 1;
            var pixels = ImmutableList<CellColor>
                .Empty
                .Add(new CellColor(new Cell(0, 0), new Color(255, 0, 0)))
                .Add(new CellColor(new Cell(7, 7), new Color(0, 255, 0)));
            sense.SetPixels(new LedPixels(pixels));
            Thread.Sleep(1000);
            for (int i = 0; i >= maxShift; i--)
            {
                sense.SetPixels(ledPixels.Shift(0, i));
                Thread.Sleep(i == 0 ? 1000 : 100);
            }
        }
    }
}
