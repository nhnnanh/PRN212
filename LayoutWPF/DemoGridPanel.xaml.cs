using System.Windows;

namespace LayoutWPF
{
    /// <summary>
    /// Interaction logic for DemoGridPanel.xaml
    /// </summary>
    public partial class DemoGridPanel : Window
    {
        public DemoGridPanel()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string comment = txtComment.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please fill out all fields (Name, E-Mail, Comment)!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string info = $"Name: {name} \n E-Mail: {email} \n Comment: {comment}";
            MessageBox.Show(info, "Contact Information Submitted", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
