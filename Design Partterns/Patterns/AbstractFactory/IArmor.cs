namespace DesignPatternsDemo.Patterns.AbstractFactory
{
    /// <summary>
    /// Abstract Product B
    /// Đại diện cho trang bị Giáp chung.
    /// </summary>
    public interface IArmor
    {
        string Name { get; }
        int DefensePower { get; }
        string GetDescription();
    }
}
