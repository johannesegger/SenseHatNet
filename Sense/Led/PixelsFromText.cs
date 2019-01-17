using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Sense.Led
{
    public static class PixelsFromText
    {
        public static Pixels Create(string text)
        {
            var width = text.Length * 10;
            using (var image = new Image<Rgba32>(Configuration.Default, width, 10))
            {
                var fontCollection = new FontCollection();
                var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var fontStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Sense.Resources.SCUMM-8px-unicode.ttf");
                var fontFamily = fontCollection.Install(fontStream);
                var font = fontFamily.CreateFont(8, FontStyle.Regular);
                image.Mutate(ctx => ctx.DrawText(text, font, Rgba32.Black, PointF.Empty));
                // using (var stream = System.IO.File.OpenWrite(@"output.png"))
                // {
                //     image.SaveAsPng(stream);
                // }
                var pixelSpan = image.GetPixelSpan();
                var textCells = new List<CellColor>();

                for (var idx = 0; idx < pixelSpan.Length; idx++)
                {
                    var pixel = pixelSpan[idx];
                    if (pixel.A > 200)
                    {
                        textCells.Add(new CellColor(
                            new Cell(idx / width, idx % width),
                            new Color(pixel.R, pixel.G, pixel.B)
                        ));
                    }
                }
                var minRow = textCells.Min(p => p.Cell.Row);
                var minColumn = textCells.Min(p => p.Cell.Column);
                return new Pixels(textCells.ToImmutableList())
                    .Shift(-minRow, -minColumn)
                    .SetColor(new Color(255, 255, 255));
            }
        }
    }
}
