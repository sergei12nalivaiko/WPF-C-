
using System.Windows;

namespace AutoLocomotive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AutoLocomotiveViewModel();
        }      
    }
}
