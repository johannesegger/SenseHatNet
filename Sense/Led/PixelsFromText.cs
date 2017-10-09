using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
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
                var fontFamily = fontCollection.Install(Path.Combine(assemblyDir, "Resources", @"SCUMM-8px-unicode.ttf"));
                var font = fontFamily.CreateFont(8, FontStyle.Regular);
                image.Mutate(ctx => ctx.DrawText(text, font, Rgba32.Black, PointF.Empty));
                // using (var stream = System.IO.File.OpenWrite(@"output.png"))
                // {
                //     image.SaveAsPng(stream);
                // }
                var pixels = image
                    .SavePixelData()
                    .Buffer(4)
                    .Select((buffer, idx) =>
                    {
                        if (buffer[3] > 200)
                        {
                            return new CellColor(
                                new Cell(idx / width, idx % width),
                                new Color(buffer[0], buffer[1], buffer[2])
                            );
                        }
                        return null;
                    })
                    .Where(p => p != null)
                    .ToImmutableList();
                var minRow = pixels.Min(p => p.Cell.Row);
                var minColumn = pixels.Min(p => p.Cell.Column);
                return new Pixels(pixels)
                    .Shift(-minRow, -minColumn)
                    .SetColor(new Color(255, 255, 255));
            }
        }
    }
}
