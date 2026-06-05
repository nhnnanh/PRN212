using System;
using System.Windows;
using System.Windows.Controls;
using Repository;
using Repository.Models;
using Category = Repository.Models.Category;

namespace ManageCategoriesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICategoryRepository _categoryRepository;

        public MainWindow()
        {
            InitializeComponent();
            _categoryRepository = new CategoryRepository();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryRepository.GetCategories();
                dgvCategories.ItemsSource = categories;
                txtStatus.Text = $"Số lượng: {categories.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách danh mục: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvCategories.SelectedItem is Category selectedCategory)
            {
                txtCategoryID.Text = selectedCategory.Id.ToString();
                txtCategoryName.Text = selectedCategory.CategoryName;
                txtDescription.Text = selectedCategory.Description;
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    MessageBox.Show("Vui lòng nhấn 'Làm mới' để bỏ chọn danh mục hiện tại trước khi thêm mới!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string name = txtCategoryName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var category = new Category
                {
                    CategoryName = name,
                    Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
                };

                _categoryRepository.AddCategory(category);
                MessageBox.Show("Thêm mới danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Thêm mới thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một danh mục từ danh sách để sửa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string name = txtCategoryName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Tên danh mục không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int id = int.Parse(txtCategoryID.Text);
                var category = new Category
                {
                    Id = id,
                    CategoryName = name,
                    Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim()
                };

                _categoryRepository.UpdateCategory(category);
                MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cập nhật thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một danh mục từ danh sách để xóa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int id = int.Parse(txtCategoryID.Text);
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục có mã {id} không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _categoryRepository.DeleteCategory(id);
                    MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    LoadCategories();
                    ClearForm();
                }
            }
            catch (InvalidOperationException ioEx)
            {
                MessageBox.Show(ioEx.Message, "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xóa thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtCategoryID.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            dgvCategories.SelectedItem = null;
        }
    }
}