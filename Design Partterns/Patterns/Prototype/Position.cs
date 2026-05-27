namespace DesignPatternsDemo.Patterns.Prototype
{
    /// <summary>
    /// Reference Type dùng để minh họa Deep vs Shallow Clone.
    /// Đại diện cho tọa độ của Quái vật trong màn chơi.
    /// </summary>
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
