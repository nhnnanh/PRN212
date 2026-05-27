using DesignPatternsDemo.Patterns.Singleton;

namespace DesignPatternsDemo.Patterns.AbstractFactory
{
    // ==========================================
    // 1. CONCRETE PRODUCT A: Shadow Weapon
    // ==========================================
    public class ShadowWeapon : IWeapon
    {
        public string Name => "Đao Quỷ Hủy Diệt (Doom Scythe)";
        public int AttackPower => 65;

        public string GetDescription()
        {
            return $"🪓 [Vũ khí] {Name} (Công vật lý: +{AttackPower}) - Đao quỷ rèn từ dung nham vực thẳm, hút sinh lực đối thủ.";
        }
    }

    // ==========================================
    // 2. CONCRETE PRODUCT B: Shadow Armor
    // ==========================================
    public class ShadowArmor : IArmor
    {
        public string Name => "Giáp Hắc Ám (Demonic Plate)";
        public int DefensePower => 45;

        public string GetDescription()
        {
            return $"👹 [Giáp] {Name} (Phòng thủ: +{DefensePower}) - Giáp sắt nặng rèn bằng thép đen ác quỷ, hấp thụ chấn động.";
        }
    }

    // ==========================================
    // 3. CONCRETE FACTORY: Shadow Gear Factory
    // ==========================================
    public class ShadowGearFactory : IGearFactory
    {
        public IWeapon CreateWeapon()
        {
            GameLogger.Instance.LogEvent("AbstractFactory", "Rèn thành công: Đao Quỷ Hủy Diệt Hắc Ám.", System.ConsoleColor.Red);
            return new ShadowWeapon();
        }

        public IArmor CreateArmor()
        {
            GameLogger.Instance.LogEvent("AbstractFactory", "Đúc thành công: Giáp Hắc Ám ác quỷ.", System.ConsoleColor.Red);
            return new ShadowArmor();
        }
    }
}
