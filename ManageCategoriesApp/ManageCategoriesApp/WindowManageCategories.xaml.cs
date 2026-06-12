using Repository;
using System;
using System.Windows;

namespace ManageCategoriesApp
{
    /// <summary>
    /// Interaction logic for WindowManageCategories.xaml
    /// </summary>
    public partial class WindowManageCategories : Window
    {
        // Khởi tạo đối tượng ManageCategories từ tầng Repository
        private readonly ManageCategories categories = new ManageCategories();

        public WindowManageCategories()
        {
            InitializeComponent();
        }

        // Sự kiện nạp dữ liệu khi mở Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        // Tải danh sách danh mục từ DAL lên ListView
        private void LoadCategories()
        {
            try
            {
                lvCategories.ItemsSource = categories.GetCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện thêm danh mục mới (Insert)
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    MessageBox.Show("Bạn đang chọn một danh mục! Vui lòng nhấn Reset để làm sạch các trường trước khi thêm mới.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string name = txtCategoryName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Tên danh mục không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Với Insert, ID tự động tăng trong SQL Server nên để tạm là 0
                Category category = new Category(0, name);
                categories.InsertCategory(category);

                MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi thêm danh mục", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện cập nhật thông tin danh mục (Update)
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
                Category category = new Category(id, name);
                categories.UpdateCategory(category);

                MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật danh mục", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện xóa danh mục (Delete)
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
                string name = txtCategoryName.Text;

                var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{name}' (Mã: {id}) không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    Category category = new Category(id, name);
                    categories.DeleteCategory(category);

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
                MessageBox.Show(ex.Message, "Lỗi xóa danh mục", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện làm mới các ô nhập liệu (Reset)
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        // Xóa sạch các trường thông tin và bỏ chọn ListView
        private void ClearForm()
        {
            lvCategories.SelectedItem = null;
            txtCategoryID.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
        }
    }
}
