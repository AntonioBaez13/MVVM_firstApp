using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_firstApp.Views
{
    /// <summary>
    /// Interaction logic for ReportesView.xaml
    /// </summary>
    public partial class ReportesView : UserControl
    {
        public ReportesView()
        {
            InitializeComponent();
        }

        private void NoLettersAllowed(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
