using System;
using System.IO;
using System.Text;
using DesignPatternsDemo.Patterns.Singleton;
using DesignPatternsDemo.Patterns.FactoryMethod;
using DesignPatternsDemo.Patterns.AbstractFactory;
using DesignPatternsDemo.Patterns.Prototype;

namespace DesignPatternsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cấu hình console hiển thị tiếng Việt có dấu chuẩn xác
            Console.OutputEncoding = Encoding.UTF8;
            SafeClear();

            PrintHeader("BÀI TẬP LỚN SIMULATION: CREATIONAL DESIGN PATTERNS IN .NET", ConsoleColor.Magenta);
            Console.WriteLine("Chào mừng em đến với chương trình mô phỏng thế giới RPG Game!");
            Console.WriteLine("Đây là tài liệu code mẫu giúp em hiểu sâu sắc về 4 mẫu thiết kế khởi tạo.");
            Console.WriteLine("Hãy nhấn bất kỳ phím nào để bắt đầu hành trình khám phá từng Pattern... 😉");
            SafeReadKey();

            // =========================================================================
            // PART 1: SINGLETON PATTERN DEMO
            // =========================================================================
            PrintSectionHeader("PART 1: SINGLETON PATTERN (Ghi Nhật Ký Toàn Cục)", ConsoleColor.Yellow);
            Console.WriteLine("Lý thuyết thực tế:");
            Console.WriteLine(" - Hệ thống sử dụng lớp GameLogger để ghi lại nhật ký hoạt động.");
            Console.WriteLine(" - Lớp này được cài đặt bằng Lazy<T> giúp đảm bảo an toàn đa luồng (Thread-safe) tuyệt đối.");
            Console.WriteLine(" - Hãy kiểm chứng xem hai biến tham chiếu logger1 và logger2 có thực sự trỏ tới một vùng nhớ duy nhất không.");
            Console.WriteLine();

            // Gọi logger qua 2 biến khác nhau
            GameLogger logger1 = GameLogger.Instance;
            GameLogger logger2 = GameLogger.Instance;

            logger1.LogEvent("ClientMain", "logger1 đang gửi thông điệp kiểm tra...", ConsoleColor.White);
            logger2.LogEvent("ClientMain", "logger2 đang gửi thông điệp kiểm tra...", ConsoleColor.White);

            Console.WriteLine();
            Console.WriteLine($"🔍 HashCode của logger1: {logger1.GetHashCode()}");
            Console.WriteLine($"🔍 HashCode của logger2: {logger2.GetHashCode()}");

            if (ReferenceEquals(logger1, logger2))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=> CHỨNG MINH THÀNH CÔNG: logger1 và logger2 trỏ tới CÙNG MỘT đối tượng duy nhất trên Heap!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=> THẤT BẠI: Khởi tạo ra nhiều hơn một thực thể Logger!");
            }
            Console.ResetColor();
            PressAnyKeyToContinue();


            // =========================================================================
            // PART 2: FACTORY METHOD PATTERN DEMO
            // =========================================================================
            PrintSectionHeader("PART 2: FACTORY METHOD PATTERN (Khởi Tạo Nhân Vật)", ConsoleColor.Cyan);
            Console.WriteLine("Lý thuyết thực tế:");
            Console.WriteLine(" - Client chỉ làm việc qua interface ICharacter và lớp abstract CharacterCreator.");
            Console.WriteLine(" - Việc khởi tạo lớp Warrior hay Mage sẽ do các Creator con quyết định.");
            Console.WriteLine();

            // Tạo các Factory tương ứng
            CharacterCreator warriorFactory = new WarriorCreator();
            CharacterCreator mageFactory = new MageCreator();

            // Sinh nhân vật bằng Factory
            ICharacter hero1 = warriorFactory.SpawnCharacter("Arthur");
            ICharacter hero2 = mageFactory.SpawnCharacter("Gandalf");

            Console.WriteLine();
            Console.WriteLine("💡 Thông tin chi tiết các nhân vật vừa được tạo từ Factory:");
            Console.WriteLine(hero1.GetDetails());
            Console.WriteLine(hero2.GetDetails());

            Console.WriteLine();
            Console.WriteLine("💡 Kích hoạt kỹ năng đặc biệt của nhân vật (Đa hình):");
            Console.WriteLine(hero1.ExecuteSkill());
            Console.WriteLine(hero2.ExecuteSkill());

            PressAnyKeyToContinue();


            // =========================================================================
            // PART 3: ABSTRACT FACTORY PATTERN DEMO
            // =========================================================================
            PrintSectionHeader("PART 3: ABSTRACT FACTORY PATTERN (Chế Tạo Trang Bị Theo Bộ)", ConsoleColor.Green);
            Console.WriteLine("Lý thuyết thực tế:");
            Console.WriteLine(" - Người chơi cần trang bị trọn bộ (vũ khí + giáp) đồng bộ.");
            Console.WriteLine(" - Tránh trường hợp mặc lẫn lộn (ví dụ: Vũ khí Hắc Ám nhưng đi với Giáp Thần Gió nhẹ).");
            Console.WriteLine(" - Chúng ta sử dụng Abstract Factory (IGearFactory) để sản xuất cả bộ trang bị đồng bộ.");
            Console.WriteLine();

            // 1. Tạo bộ trang bị Tiên Tộc (Elven Set) cho Pháp sư Gandalf
            GameLogger.Instance.LogEvent("ClientMain", "Yêu cầu rèn trang bị bộ Elven Set cho Gandalf...", ConsoleColor.White);
            IGearFactory elvenFactory = new ElvenGearFactory();
            IWeapon elvenWeapon = elvenFactory.CreateWeapon();
            IArmor elvenArmor = elvenFactory.CreateArmor();

            Console.WriteLine();
            Console.WriteLine($"🧙‍♂️ Gandalf mặc trang bị:");
            Console.WriteLine($"   -> {elvenWeapon.GetDescription()}");
            Console.WriteLine($"   -> {elvenArmor.GetDescription()}");

            Console.WriteLine();
            // 2. Tạo bộ trang bị Hắc Ám (Shadow Set) cho Chiến binh Arthur
            GameLogger.Instance.LogEvent("ClientMain", "Yêu cầu rèn trang bị bộ Shadow Set cho Arthur...", ConsoleColor.White);
            IGearFactory shadowFactory = new ShadowGearFactory();
            IWeapon shadowWeapon = shadowFactory.CreateWeapon();
            IArmor shadowArmor = shadowFactory.CreateArmor();

            Console.WriteLine();
            Console.WriteLine($"🛡️ Arthur mặc trang bị:");
            Console.WriteLine($"   -> {shadowWeapon.GetDescription()}");
            Console.WriteLine($"   -> {shadowArmor.GetDescription()}");

            PressAnyKeyToContinue();


            // =========================================================================
            // PART 4: PROTOTYPE PATTERN DEMO
            // =========================================================================
            PrintSectionHeader("PART 4: PROTOTYPE PATTERN (Nhân Bản Quái Vật - Deep vs Shallow Copy)", ConsoleColor.Magenta);
            Console.WriteLine("Lý thuyết thực tế:");
            Console.WriteLine(" - Hệ thống cần spawn hàng ngàn quái vật Goblin.");
            Console.WriteLine(" - Việc 'new' liên tục và nạp cấu hình từ DB rất tốn kém hiệu năng.");
            Console.WriteLine(" - Giải pháp: Tạo một quái vật gốc (Prototype) và gọi Clone().");
            Console.WriteLine(" ⚠️ CẢNH BÁO QUAN TRỌNG: Tọa độ (Position) là kiểu Reference Type. Hãy xem điều gì xảy ra!");
            Console.WriteLine();

            // Khởi tạo quái vật gốc tại tọa độ (10, 10)
            Monster originGoblin = new Monster("Goblin Chúa", 500, new Position(10, 10));
            Console.WriteLine("--- QUÁI VẬT GỐC BAN ĐẦU ---");
            Console.WriteLine(originGoblin);
            Console.WriteLine();

            // -------------------------------------------------------------------------
            // Trường hợp A: SHALLOW CLONE (Sao chép nông)
            // -------------------------------------------------------------------------
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(">>> THỰC HIỆN SHALLOW CLONE (SAO CHÉP NÔNG) <<<");
            Console.ResetColor();
            Monster shallowGoblin = (Monster)originGoblin.Clone(isDeep: false);
            shallowGoblin.Name = "Goblin Đột Biến (Shallow)";
            
            Console.WriteLine("Sau khi Shallow Clone thành công:");
            Console.WriteLine($"   Origin: {originGoblin}");
            Console.WriteLine($"   Clone : {shallowGoblin}");

            Console.WriteLine("\nHành động: Thay đổi tọa độ của quái vật Clone (Shallow) thành (50, 50)...");
            shallowGoblin.LocPosition.X = 50;
            shallowGoblin.LocPosition.Y = 50;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("💥 HẬU QUẢ CỦA SHALLOW CLONE:");
            Console.ResetColor();
            Console.WriteLine($"   Origin (Gốc)   : {originGoblin}");
            Console.WriteLine($"   Clone (Sao chép): {shallowGoblin}");
            Console.WriteLine("👉 LƯU Ý: Vì cả 2 chia sẻ chung 1 vùng nhớ tọa độ (cùng HashCode), khi thay đổi tọa độ quái clone, quái gốc BỊ DI CHUYỂN THEO! Đây là lỗi logic game cực kỳ nghiêm trọng.");

            Console.WriteLine();
            PressAnyKeyToContinue();

            // Reset lại tọa độ gốc về (10, 10)
            originGoblin.LocPosition.X = 10;
            originGoblin.LocPosition.Y = 10;

            // -------------------------------------------------------------------------
            // Trường hợp B: DEEP CLONE (Sao chép sâu)
            // -------------------------------------------------------------------------
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(">>> THỰC HIỆN DEEP CLONE (SAO CHÉP SÂU) <<<");
            Console.ResetColor();
            Monster deepGoblin = (Monster)originGoblin.Clone(isDeep: true);
            deepGoblin.Name = "Goblin Đột Biến (Deep)";

            Console.WriteLine("Sau khi Deep Clone thành công:");
            Console.WriteLine($"   Origin: {originGoblin}");
            Console.WriteLine($"   Clone : {deepGoblin}");

            Console.WriteLine("\nHành động: Thay đổi tọa độ của quái vật Clone (Deep) thành (99, 99)...");
            deepGoblin.LocPosition.X = 99;
            deepGoblin.LocPosition.Y = 99;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("🎉 THÀNH CÔNG RỰC RỠ VỚI DEEP CLONE:");
            Console.ResetColor();
            Console.WriteLine($"   Origin (Gốc)   : {originGoblin}");
            Console.WriteLine($"   Clone (Sao chép): {deepGoblin}");
            Console.WriteLine("👉 LƯU Ý: Tọa độ của quái gốc vẫn được giữ nguyên vẹn ở (10, 10) vì quái clone sở hữu một vùng nhớ Position hoàn toàn riêng biệt trên Heap (khác HashCode)!");

            Console.WriteLine();
            PressAnyKeyToContinue();


            // =========================================================================
            // PART 5: TỔNG KẾT NHẬT KÝ GAME SYSTEM
            // =========================================================================
            PrintSectionHeader("PART 5: XUẤT LỊCH SỬ LOG TỪ SINGLETON", ConsoleColor.White);
            Console.WriteLine("Hãy cùng nhìn lại lịch sử log toàn cục đã được Singleton ghi lại trong bộ nhớ suốt quá trình chạy:");
            Console.WriteLine();

            var logs = GameLogger.Instance.GetLogHistory();
            foreach (var log in logs)
            {
                Console.WriteLine($" 🔹 {log}");
            }

            Console.WriteLine();
            PrintHeader("CHƯƠNG TRÌNH HOÀN THÀNH - CẢM ƠN EM ĐÃ THEO DÕI!", ConsoleColor.Magenta);
            Console.WriteLine("Hy vọng dự án này giúp em có cái nhìn trực quan nhất về Creational Patterns trong C#.");
            Console.WriteLine("Chúc em đạt điểm tối đa trong kỳ thi PRN212 sắp tới! Thầy tin em sẽ làm được! 💪");
            Console.WriteLine();
        }

        #region Safe Console Helpers
        static void SafeClear()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch (IOException) { }
        }

        static void SafeReadKey()
        {
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("[Chạy tự động: Đã bỏ qua lệnh nhấn phím]");
                }
            }
            catch (IOException) { }
        }

        static int GetSafeWindowWidth()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    return Console.WindowWidth;
                }
            }
            catch (IOException) { }
            return 80;
        }

        static void PrintHeader(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            int width = GetSafeWindowWidth();
            int lineLen = width > 0 ? width - 1 : 80;
            Console.WriteLine(new string('=', lineLen));
            Console.WriteLine($"   {text}");
            Console.WriteLine(new string('=', lineLen));
            Console.ResetColor();
        }

        static void PrintSectionHeader(string text, ConsoleColor color)
        {
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.WriteLine($"--- {text} ---");
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
        }

        static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\n[Nhấn một phím bất kỳ để tiếp tục phân tích bước tiếp theo...]");
            SafeReadKey();
            Console.WriteLine();
        }
        #endregion
    }
}
