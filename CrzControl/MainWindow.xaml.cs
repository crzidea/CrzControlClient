using System;
using System.Windows;

namespace CrzControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        CrzControlController controller = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (btnStart.Content.Equals("Logout"))
            {
                btnStart.Content = "Login";
                txtUsername.IsEnabled = true;
                txtPassword.IsEnabled = true;
            }
            else
            {
                btnStart.Content = "Logout";
                txtUsername.IsEnabled = false;
                txtPassword.IsEnabled = false;
                controller = new CrzControlController(this);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtConsole.Clear();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
