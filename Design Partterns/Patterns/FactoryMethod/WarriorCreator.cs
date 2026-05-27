namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Concrete Creator A
    /// Ghi đè Factory Method để quyết định tạo ra đối tượng Warrior.
    /// </summary>
    public class WarriorCreator : CharacterCreator
    {
        public override ICharacter CreateCharacter(string name)
        {
            return new Warrior(name);
        }
    }
}
