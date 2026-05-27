using DesignPatternsDemo.Patterns.Singleton;

namespace DesignPatternsDemo.Patterns.FactoryMethod
{
    /// <summary>
    /// Creator (Abstract Class)
    /// Khai báo Factory Method để các lớp con ghi đè nhằm trả về sản phẩm cụ thể.
    /// Ngoài ra, lớp này có thể chứa một số logic nghiệp vụ quan trọng dựa trên sản phẩm đó.
    /// </summary>
    public abstract class CharacterCreator
    {
        // 1. Factory Method - Các lớp con bắt buộc phải override phương thức này
        public abstract ICharacter CreateCharacter(string name);

        // 2. Logic nghiệp vụ cốt lõi: Sử dụng sản phẩm mà không cần biết class cụ thể là gì
        public ICharacter SpawnCharacter(string name)
        {
            // Gọi Factory Method để tạo đối tượng
            ICharacter character = CreateCharacter(name);

            // Tận dụng sự hợp tác với Singleton GameLogger để ghi nhận sự kiện
            GameLogger.Instance.LogEvent(
                "FactoryMethod", 
                $"Đã sinh ra nhân vật mới thành công: '{character.Name}' thuộc hệ {character.ClassName}.", 
                System.ConsoleColor.Cyan
            );

            return character;
        }
    }
}
