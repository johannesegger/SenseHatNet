using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Sense
{
    public class LedPixels
    {
        public static readonly LedPixels Empty = new LedPixels(ImmutableList<CellColor>.Empty);

        public ImmutableList<CellColor> Pixels { get; }

        public LedPixels(ImmutableList<CellColor> pixels)
        {
            Pixels = pixels;
        }

        public LedPixels Set(CellColor pixel)
        {
            var equalityComparer = EqualityComparer
                .Create((CellColor p) => new { p.Cell.Row, p.Cell.Column });
            var newPixels = Pixels
                .Remove(pixel, equalityComparer)
                .Add(pixel);
            return new LedPixels(newPixels);
        }

        public LedPixels SetColor(Color color)
        {
            var newPixels = Pixels
                .Select(p => new CellColor(p.Cell, color))
                .ToImmutableList();
            return new LedPixels(newPixels);
        }

        public LedPixels Shift(int rows, int columns)
        {
            var newPixels = Pixels
                .Select(p => new CellColor(new Cell(p.Cell.Row + rows, p.Cell.Column + columns), p.Color))
                .ToImmutableList();
            return new LedPixels(newPixels);
        }
    }
}
