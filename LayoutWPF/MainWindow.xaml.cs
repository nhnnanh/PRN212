using System.Windows;

namespace LayoutWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCanvas_Click(object sender, RoutedEventArgs e)
        {
            DemoCanvasPanel canvasWindow = new DemoCanvasPanel();
            canvasWindow.Show();
        }

        private void OpenWrap_Click(object sender, RoutedEventArgs e)
        {
            DemoWrapPanel wrapWindow = new DemoWrapPanel();
            wrapWindow.Show();
        }

        private void OpenStack_Click(object sender, RoutedEventArgs e)
        {
            DemoStackPanel stackWindow = new DemoStackPanel();
            stackWindow.Show();
        }

        private void OpenGrid_Click(object sender, RoutedEventArgs e)
        {
            DemoGridPanel gridWindow = new DemoGridPanel();
            gridWindow.Show();
        }

        private void OpenDock_Click(object sender, RoutedEventArgs e)
        {
            DemoDockPanel dockWindow = new DemoDockPanel();
            dockWindow.Show();
        }
    }
}