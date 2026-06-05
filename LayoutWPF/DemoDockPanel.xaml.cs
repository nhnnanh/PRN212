using System.Windows;
using System.Windows.Controls;

namespace LayoutWPF
{
    /// <summary>
    /// Interaction logic for DemoDockPanel.xaml
    /// </summary>
    public partial class DemoDockPanel : Window
    {
        public DemoDockPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                MessageBox.Show($"You clicked the '{clickedButton.Content}' button (DockPanel layout position)!", "DockPanel Event", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
