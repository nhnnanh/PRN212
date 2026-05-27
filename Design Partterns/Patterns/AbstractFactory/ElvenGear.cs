using DesignPatternsDemo.Patterns.Singleton;

namespace DesignPatternsDemo.Patterns.AbstractFactory
{
    // ==========================================
    // 1. CONCRETE PRODUCT A: Elven Weapon
    // ==========================================
    public class ElvenWeapon : IWeapon
    {
        public string Name => "Cung Thần Gió (Astra Bow)";
        public int AttackPower => 45;

        public string GetDescription()
        {
            return $"🏹 [Vũ khí] {Name} (Công vật lý: +{AttackPower}) - Được rèn từ gỗ cây cổ thụ ngàn năm, mang theo sức mạnh của gió.";
        }
    }

    // ==========================================
    // 2. CONCRETE PRODUCT B: Elven Armor
    // ==========================================
    public class ElvenArmor : IArmor
    {
        public string Name => "Giáp Tơ Bích Ngọc (Jade Silk Armor)";
        public int DefensePower => 25;

        public string GetDescription()
        {
            return $"🍃 [Giáp] {Name} (Phòng thủ: +{DefensePower}) - Nhẹ như tơ, có khả năng né tránh các đòn đánh phép thuật.";
        }
    }

    // ==========================================
    // 3. CONCRETE FACTORY: Elven Gear Factory
    // ==========================================
    public class ElvenGearFactory : IGearFactory
    {
        public IWeapon CreateWeapon()
        {
            GameLogger.Instance.LogEvent("AbstractFactory", "Rèn thành công: Cung Thần Gió tộc Tiên.", System.ConsoleColor.Green);
            return new ElvenWeapon();
        }

        public IArmor CreateArmor()
        {
            GameLogger.Instance.LogEvent("AbstractFactory", "May thành công: Giáp Tơ Bích Ngọc tộc Tiên.", System.ConsoleColor.Green);
            return new ElvenArmor();
        }
    }
}
