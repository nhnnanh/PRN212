using System;
using System.Collections.Generic;

namespace DesignPatternsDemo.Patterns.Singleton
{
    /// <summary>
    /// Singleton Pattern - GameLogger
    /// Đảm bảo chỉ có một instance duy nhất ghi nhận nhật ký của toàn bộ trò chơi.
    /// Sử dụng Lazy<T> để đảm bảo Thread-safe và Lazy Initialization một cách thanh lịch và tối ưu nhất trong C#.
    /// </summary>
    public sealed class GameLogger
    {
        // 1. Dùng Lazy<T> để tự động quản lý khởi tạo trễ (lazy) và an toàn đa luồng (thread-safe)
        private static readonly Lazy<GameLogger> _instance = 
            new Lazy<GameLogger>(() => new GameLogger());

        // Danh sách lưu trữ lịch sử log trong bộ nhớ (để chứng minh trạng thái được chia sẻ toàn cục)
        private readonly List<string> _logHistory = new List<string>();
        private readonly object _historyLock = new object();

        // 2. Private Constructor: Ngăn cản tuyệt đối việc khởi tạo từ bên ngoài bằng từ khóa 'new'
        private GameLogger()
        {
            LogDirect("[HỆ THỐNG] Khởi tạo GameLogger lần đầu tiên (Duy nhất)!", ConsoleColor.Yellow);
        }

        // 3. Public Static Property: Cổng truy cập toàn cục duy nhất
        public static GameLogger Instance => _instance.Value;

        /// <summary>
        /// Ghi nhận một sự kiện trong game
        /// </summary>
        public void LogEvent(string sender, string message, ConsoleColor color = ConsoleColor.Gray)
        {
            string formattedMessage = $"[{DateTime.Now:HH:mm:ss.fff}] [{sender}]: {message}";
            
            // Đảm bảo thread-safe khi ghi đè vào danh sách dùng chung
            lock (_historyLock)
            {
                _logHistory.Add(formattedMessage);
            }

            LogDirect(formattedMessage, color);
        }

        /// <summary>
        /// Phương thức phụ để hiển thị trực tiếp lên console
        /// </summary>
        private void LogDirect(string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Lấy ra toàn bộ lịch sử log để kiểm tra tính toàn vẹn dữ liệu
        /// </summary>
        public IReadOnlyList<string> GetLogHistory()
        {
            lock (_historyLock)
            {
                return _logHistory.AsReadOnly();
            }
        }
    }
}
