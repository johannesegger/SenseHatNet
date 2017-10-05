public class Letter
{
    private static Color color = new Color(0xFF, 0xFF, 0xFF);

    public static LedPixels A = LedPixels.Empty
        .Set(new CellColor(new Cell(6, 0), color))
        .Set(new CellColor(new Cell(5, 0), color))
        .Set(new CellColor(new Cell(4, 0), color))
        .Set(new CellColor(new Cell(3, 0), color))
        .Set(new CellColor(new Cell(2, 0), color))
        .Set(new CellColor(new Cell(1, 0), color))
        .Set(new CellColor(new Cell(0, 1), color))
        .Set(new CellColor(new Cell(0, 2), color))
        .Set(new CellColor(new Cell(0, 3), color))
        .Set(new CellColor(new Cell(3, 1), color))
        .Set(new CellColor(new Cell(3, 2), color))
        .Set(new CellColor(new Cell(3, 3), color))
        .Set(new CellColor(new Cell(6, 4), color))
        .Set(new CellColor(new Cell(5, 4), color))
        .Set(new CellColor(new Cell(4, 4), color))
        .Set(new CellColor(new Cell(3, 4), color))
        .Set(new CellColor(new Cell(2, 4), color))
        .Set(new CellColor(new Cell(1, 4), color));
}