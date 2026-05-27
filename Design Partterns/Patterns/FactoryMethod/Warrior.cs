namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Concrete Product A
    /// Đại diện cho lớp nhân vật Chiến Binh (Warrior).
    /// </summary>
    public class Warrior : ICharacter
    {
        public string Name { get; }
        public string ClassName => "Chiến Binh (Warrior)";
        public int BaseHealth => 150;
        public int BaseMana => 30;

        public Warrior(string name)
        {
            Name = name;
        }

        public string ExecuteSkill()
        {
            return $"💥 {Name} sử dụng [Đại Địa Chấn - Earthshaker]! Gây sát thương vật lý cực khủng và làm choáng kẻ thù xung quanh.";
        }

        public string GetDetails()
        {
            return $"🛡️ Nhân vật: {Name} | Lớp: {ClassName} | HP: {BaseHealth} | MP: {BaseMana}";
        }
    }
}
