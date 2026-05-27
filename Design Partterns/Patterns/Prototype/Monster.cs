using DesignPatternsDemo.Patterns.Singleton;

namespace DesignPatternsDemo.Patterns.Prototype
{
    /// <summary>
    /// Concrete Prototype
    /// Lớp Quái vật cụ thể có thể nhân bản.
    /// Minh họa chi tiết về sự khác biệt giữa Shallow Clone và Deep Clone.
    /// </summary>
    public class Monster : IMonsterPrototype
    {
        public string Name { get; set; }
        public int Health { get; set; }
        
        // Tọa độ là kiểu dữ liệu tham chiếu (Reference Type)
        public Position LocPosition { get; set; }

        public Monster(string name, int health, Position position)
        {
            Name = name;
            Health = health;
            LocPosition = position;
        }

        /// <summary>
        /// Phương thức nhân bản cốt lõi
        /// </summary>
        /// <param name="isDeep">
        /// true: Deep Clone (Nhân bản sâu - sao chép cả dữ liệu tham chiếu)
        /// false: Shallow Clone (Nhân bản nông - chỉ sao chép địa chỉ tham chiếu)
        /// </param>
        public IMonsterPrototype Clone(bool isDeep)
        {
            if (isDeep)
            {
                // 1. DEEP CLONE (Nhân bản sâu)
                // - Đầu tiên, thực hiện sao chép nông các trường nguyên thủy (Value types)
                Monster deepClone = (Monster)this.MemberwiseClone();
                
                // - Sau đó, tạo hẳn một vùng nhớ mới cho đối tượng tham chiếu LocPosition
                deepClone.LocPosition = new Position(this.LocPosition.X, this.LocPosition.Y);

                GameLogger.Instance.LogEvent("Prototype", $"Đã nhân bản SÂU (Deep Clone) quái vật '{Name}'. Tọa độ độc lập.", System.ConsoleColor.Blue);
                return deepClone;
            }
            else
            {
                // 2. SHALLOW CLONE (Nhân bản nông)
                // - Chỉ sử dụng MemberwiseClone() của C#.
                // - Trường LocPosition (Reference type) sẽ bị copy địa chỉ con trỏ vùng nhớ.
                // - Cả đối tượng gốc và đối tượng clone sẽ cùng trỏ vào MỘT vùng nhớ Position trên Heap.
                Monster shallowClone = (Monster)this.MemberwiseClone();

                GameLogger.Instance.LogEvent("Prototype", $"Đã nhân bản NÔNG (Shallow Clone) quái vật '{Name}'. Tọa độ bị chia sẻ vùng nhớ!", System.ConsoleColor.DarkYellow);
                return shallowClone;
            }
        }

        public override string ToString()
        {
            return $"👾 [Quái vật] {Name} | HP: {Health} | Tọa độ: {LocPosition} (HashCode của Position: {LocPosition.GetHashCode()})";
        }
    }
}
