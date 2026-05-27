namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Product Interface
    /// Định nghĩa giao diện chung cho tất cả các loại nhân vật (Warrior, Mage, v.v...)
    /// </summary>
    public interface ICharacter
    {
        string Name { get; }
        string ClassName { get; }
        int BaseHealth { get; }
        int BaseMana { get; }

        string ExecuteSkill();
        string GetDetails();
    }
}
