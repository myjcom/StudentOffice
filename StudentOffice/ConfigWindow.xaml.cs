using System.Windows;

namespace StudentOffice
{
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Owner).clientDataGrid.IsReadOnly = !(bool)enableEdit.IsChecked;
            ((MainWindow)Owner).appConfig.ContractDocFileName = pathCon.Text.Trim();
            ((MainWindow)Owner).appConfig.ReferenceDocFileName = pathSpr.Text.Trim();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pathSpr.Text = ((MainWindow)Owner).appConfig.ReferenceDocFileName;
            pathCon.Text = ((MainWindow)Owner).appConfig.ContractDocFileName;
            enableEdit.IsChecked = !((MainWindow)Owner).clientDataGrid.IsReadOnly;
        }
    }
}
