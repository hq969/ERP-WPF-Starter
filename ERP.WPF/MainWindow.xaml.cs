using ERP.WPF.ViewModels;
using System.Windows;

namespace ERP.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
