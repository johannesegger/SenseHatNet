using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Sense.Utils;

namespace Sense.Led
{
    public class Pixels
    {
        public static readonly Pixels Empty = new Pixels(ImmutableList<CellColor>.Empty);

        public ImmutableList<CellColor> Items { get; }

        public Pixels(ImmutableList<CellColor> items)
        {
            Items = items;
        }

        public Pixels Set(CellColor pixel)
        {
            var equalityComparer = EqualityComparer
                .Create((CellColor p) => new { p.Cell.Row, p.Cell.Column });
            var newPixels = Items
                .Remove(pixel, equalityComparer)
                .Add(pixel);
            return new Pixels(newPixels);
        }

        public Pixels SetColor(Color color)
        {
            var newPixels = Items
                .Select(p => new CellColor(p.Cell, color))
                .ToImmutableList();
            return new Pixels(newPixels);
        }

        public Pixels Shift(int rows, int columns)
        {
            var newPixels = Items
                .Select(p => new CellColor(new Cell(p.Cell.Row + rows, p.Cell.Column + columns), p.Color))
                .ToImmutableList();
            return new Pixels(newPixels);
        }
    }
}
