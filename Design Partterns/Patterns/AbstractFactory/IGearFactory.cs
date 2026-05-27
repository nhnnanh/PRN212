namespace DesignPatternsDemo.Patterns.AbstractFactory
{
    /// <summary>
    /// Abstract Factory
    /// Định nghĩa giao diện để khởi tạo các họ sản phẩm trang bị có quan hệ tương thích với nhau.
    /// </summary>
    public interface IGearFactory
    {
        IWeapon CreateWeapon();
        IArmor CreateArmor();
    }
}
