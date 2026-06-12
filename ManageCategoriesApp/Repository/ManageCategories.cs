using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Repository
{
    // Định nghĩa Record Category đại diện cho bảng Categories
    public record Category(int CategoryID, string CategoryName);

    public class ManageCategories
    {
        private readonly string ConnectionString = "Server=NgocAnh;Database=Mystore;User Id=sa;Password=ngocanh1203;TrustServerCertificate=True;Encrypt=False;";

        // Phương thức lấy danh sách danh mục
        public List<Category> GetCategories()
        {
            var list = new List<Category>();
            string query = "Select ID as CategoryID, CategoryName from Categories";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Category(
                                    reader.GetInt32(0),
                                    reader.GetString(1)
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách danh mục: " + ex.Message, ex);
            }

            return list;
        }

        public void InsertCategory(Category category)
        {
            string query = "Insert Categories (CategoryName) values(@CategoryName)";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm danh mục: " + ex.Message, ex);
            }
        }

        public void UpdateCategory(Category category)
        {
            string query = "Update Categories set CategoryName=@CategoryName where ID=@CategoryID";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật danh mục: " + ex.Message, ex);
            }
        }

        public void DeleteCategory(Category category)
        {
            string query = "Delete Categories where ID=@CategoryID";

            try
            {
                // Kiểm tra ràng buộc khóa ngoại trước khi thực hiện xóa
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                        connection.Open();
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa danh mục này vì đã có sản phẩm thuộc danh mục!");
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa danh mục: " + ex.Message, ex);
            }
        }
    }
}
