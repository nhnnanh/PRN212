using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqAndDelegates
{
    // Định nghĩa các class theo yêu cầu đề bài
    public class Pet
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class PetOwner
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Pets { get; set; } = new List<string>();
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // #1. Lọc số chẵn từ mảng
            Console.WriteLine("=== BÀI 1 ===");
            {
                int[] n1 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };       
                // nQuery is an IEnumerable<int>
                var nQuery = from tmp in n1
                             where (tmp % 2) == 0
                             select tmp;

                Console.WriteLine("Dữ liệu gốc: " + string.Join(", ", n1));
                Console.WriteLine("Kết quả (số chẵn): " + string.Join(", ", nQuery));
            }
            Console.WriteLine();

            // #2. Lọc số dương nằm trong khoảng (0, 12)
            Console.WriteLine("=== BÀI 2 ===");
            {
                int[] n1 = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14  };  
                var nQuery = from tmp in n1 
                             where tmp > 0 
                             where tmp < 12 
                             select tmp;

                Console.WriteLine("Dữ liệu gốc: " + string.Join(", ", n1));
                Console.WriteLine("Kết quả (số dương < 12): " + string.Join(", ", nQuery));
            }
            Console.WriteLine();

            // #3. Lọc tên động vật độ dài >= 5 và viết hoa
            Console.WriteLine("=== BÀI 3 ===");
            {
                List<string> animals = new List<string> { "zebra", "elephant", "cat", "dog", "rhino", "bat" };
                var selectedAnimals = animals.Where(s => s.Length >= 5).Select(x => x.ToUpper()); 

                Console.WriteLine("Dữ liệu gốc: " + string.Join(", ", animals));
                Console.WriteLine("Kết quả (độ dài >= 5 & viết hoa): " + string.Join(", ", selectedAnimals));
            }
            Console.WriteLine();

            // #4. Sắp xếp giảm dần và lấy 5 phần tử đầu tiên
            Console.WriteLine("=== BÀI 4 ===");
            {
                List<int> numbers = new List<int> { 6, 0, 999, 11, 443, 6, 1, 24, 54 };
                var top5 = numbers.OrderByDescending(x => x).Take(5);

                Console.WriteLine("Dữ liệu gốc: " + string.Join(", ", numbers));
                Console.WriteLine("Kết quả (5 số lớn nhất): " + string.Join(", ", top5));
            }
            Console.WriteLine();

            // #5. Sắp xếp mảng Pets theo tuổi
            Console.WriteLine("=== BÀI 5 ===");
            {
                Pet[] pets = { new Pet { Name="Barley", Age=8 },
                               new Pet { Name="Boots", Age=4 },
                               new Pet { Name="Whiskers", Age=1 } };
     
                IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

                Console.WriteLine("Danh sách Pets gốc:");
                foreach (var pet in pets)
                {
                    Console.WriteLine($"- {pet.Name} ({pet.Age} tuổi)");
                }

                Console.WriteLine("Kết quả sắp xếp tăng dần theo tuổi:");
                foreach (var pet in query)
                {
                    Console.WriteLine($"- {pet.Name} ({pet.Age} tuổi)");
                }
            }
            Console.WriteLine();

            // #6. Trải phẳng và lọc danh sách chủ và vật nuôi
            Console.WriteLine("=== BÀI 6 ===");
            {
                PetOwner[] petOwners = { new PetOwner { Name="Higa", Pets = new List<string>{ "Scruffy", "Sam" } },
                      new PetOwner { Name="Ashkenazi", Pets = new List<string>{ "Walker", "Sugar" } },
                      new PetOwner { Name="Price", Pets = new List<string>{ "Scratches", "Diesel" } },
                      new PetOwner { Name="Hines", Pets = new List<string>{ "Dusty" } } };

                var query = petOwners.SelectMany(
                    petOwner => petOwner.Pets, 
                    (petOwner, petName) => new { petOwner, petName }
                )
                .Where(ownerAndPet => ownerAndPet.petName.StartsWith("S"))
                .Select(ownerAndPet => new {
                    Owner = ownerAndPet.petOwner.Name,
                    Pet = ownerAndPet.petName
                });

                Console.WriteLine("Kết quả lọc thú cưng bắt đầu bằng chữ 'S' và chủ:");
                foreach (var item in query)
                {
                    Console.WriteLine($"- Chủ: {item.Owner} | Thú cưng: {item.Pet}");
                }
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
