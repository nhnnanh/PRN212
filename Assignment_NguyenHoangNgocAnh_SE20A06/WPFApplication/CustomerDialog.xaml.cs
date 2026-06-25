using System.Windows;
using BusinessObjects;
using WPFApplication.ViewModels;

namespace WPFApplication
{
    public partial class CustomerDialog : Window
    {
        public CustomerDialog(Customer? customer)
        {
            InitializeComponent();
            DataContext = new CustomerDialogViewModel(customer);
        }
    }
}
