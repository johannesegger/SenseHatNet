using System.Collections.Immutable;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.Primitives;

public static class LedPixelsFromText
{
    public static LedPixels Create(string text)
    {
        var width = text.Length * 10;
        using (var image = new Image<Rgba32>(Configuration.Default, width, 10))
        {
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Install(@"SCUMM-8px-unicode.ttf");
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
            return new LedPixels(pixels).Shift(-minRow, -minColumn);
        }
    }
}