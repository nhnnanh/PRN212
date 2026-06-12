using System.Windows;

namespace LayoutWPF
{
    /// <summary>
    /// Interaction logic for DemoCanvasPanel.xaml
    /// </summary>
    public partial class DemoCanvasPanel : Window
    {
        public DemoCanvasPanel()
        {
            InitializeComponent();
        }

        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            string carName = txtCarName.Text.Trim();
            string color = txtColor.Text.Trim();
            string brand = txtBrand.Text.Trim();

            if (string.IsNullOrEmpty(carName) || string.IsNullOrEmpty(color) || string.IsNullOrEmpty(brand))
            {
                MessageBox.Show("Please enter all fields (Car Name, Color, Brand)!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string info = $"Car Name: {carName} \n Color: {color} \n Brand: {brand}";
            MessageBox.Show(info, "Car Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
