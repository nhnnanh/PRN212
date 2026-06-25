using System.Windows;
using BusinessObjects;
using WPFApplication.ViewModels;

namespace WPFApplication
{
    public partial class BookingDialog : Window
    {
        public BookingDialog(BookingReservation? booking)
        {
            InitializeComponent();
            DataContext = new BookingDialogViewModel(booking);
        }
    }
}
