namespace DesignPatternsDemo.Patterns.AbstractFactory
{
    /// <summary>
    /// Abstract Product A
    /// Đại diện cho trang bị Vũ khí chung.
    /// </summary>
    public interface IWeapon
    {
        string Name { get; }
        int AttackPower { get; }
        string GetDescription();
    }
}
