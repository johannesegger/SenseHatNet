public class CellColor
{
    public Cell Cell { get; }
    public Color Color { get; }

    public CellColor(Cell cell, Color color)
    {
        Cell = cell;
        Color = color;
    }
}