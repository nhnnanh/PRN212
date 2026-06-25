using System.Windows;
using BusinessObjects;
using WPFApplication.ViewModels;

namespace WPFApplication
{
    public partial class CustomerWindow : Window
    {
        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(customer);
        }
    }
}
