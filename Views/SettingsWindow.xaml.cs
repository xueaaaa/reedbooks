using System.Diagnostics;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/xueaaaa/reedbooks/wiki");
        }
    }
}
