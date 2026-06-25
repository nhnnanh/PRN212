using System.Windows;
using BusinessObjects;
using WPFApplication.ViewModels;

namespace WPFApplication
{
    public partial class RoomDialog : Window
    {
        public RoomDialog(RoomInformation? room)
        {
            InitializeComponent();
            DataContext = new RoomDialogViewModel(room);
        }
    }
}
