namespace DesignPatternsDemo.Patterns.Prototype
{
    /// <summary>
    /// Prototype Interface
    /// Định nghĩa phương thức nhân bản mẫu đối tượng.
    /// Cho phép tùy chọn nhân bản nông (Shallow Clone) hoặc nhân bản sâu (Deep Clone).
    /// </summary>
    public interface IMonsterPrototype
    {
        IMonsterPrototype Clone(bool isDeep);
    }
}
