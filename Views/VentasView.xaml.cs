using MVVM_firstApp.ViewModels;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_firstApp.Views
{
    /// <summary>
    /// Interaction logic for VentasView.xaml
    /// </summary>
    public partial class VentasView : UserControl
    {
        public VentasView()
        {
            InitializeComponent();
        }

        private void NoLettersAllowed(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PuntosInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(JugadaInput.Text))
            {
                e.Handled = true;
                JugadaInput.Focus();
            }
            else if (e.Key == Key.Enter && !string.IsNullOrEmpty(PuntosInput.Text.Trim('0')))
            {
                AddValuesToCollection();
            }
        }

        private void JugadaInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(PuntosInput.Text))
            {
                e.Handled = true;
                PuntosInput.Focus();
            }
            else if (e.Key == Key.Enter && !string.IsNullOrEmpty(JugadaInput.Text) && !string.IsNullOrEmpty(PuntosInput.Text.Trim('0')))
            {
                AddValuesToCollection();
            }
        }

        private void AddValuesToCollection()
        {
            //TODO: Validate so that adding an space doesn't throw an exception
            int puntos = CleanTextBoxes.CleanPuntosInput(PuntosInput.Text);
            string jugada = CleanTextBoxes.CleanJugadaInput(JugadaInput.Text);

            if (jugada.Length > 2)
            {
                puntos = 1;
            }
            else if (puntos > 5)
            {
                puntos = 5;
            }

            ((VentasViewModel)DataContext).AddToCollection(puntos, jugada);

            VistaPrevia.ScrollIntoView(VistaPrevia.SelectedItem);
            EmptyTextBoxes();
        }

        private void EmptyTextBoxes()
        {
            PuntosInput.Clear();
            JugadaInput.Clear();
            PuntosInput.Focus();
        }

    }
}
