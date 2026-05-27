namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Concrete Product B
    /// Đại diện cho lớp nhân vật Pháp Sư (Mage).
    /// </summary>
    public class Mage : ICharacter
    {
        public string Name { get; }
        public string ClassName => "Pháp Sư (Mage)";
        public int BaseHealth => 85;
        public int BaseMana => 200;

        public Mage(string name)
        {
            Name = name;
        }

        public string ExecuteSkill()
        {
            return $"❄️ {Name} niệm chú [Bão Tuyết Băng Giá - Blizzard]! Đóng băng diện rộng và giảm tốc độ di chuyển của mọi đối thủ.";
        }

        public string GetDetails()
        {
            return $"🔮 Nhân vật: {Name} | Lớp: {ClassName} | HP: {BaseHealth} | MP: {BaseMana}";
        }
    }
}
