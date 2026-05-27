namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Concrete Creator B
    /// Ghi đè Factory Method để quyết định tạo ra đối tượng Mage.
    /// </summary>
    public class MageCreator : CharacterCreator
    {
        public override ICharacter CreateCharacter(string name)
        {
            return new Mage(name);
        }
    }
}
