using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MVVM_firstApp.Pages
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {

        public ShellView()
        {
            InitializeComponent();
        }

        // Only Allow numbers on the textboxes 
        private void NoLettersAllowed(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Method to handle what happens when Enter is pressed on the textbox
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

        // Method to handle what happens when Enter is pressed on the textbox
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
            int puntos = Int32.Parse(PuntosInput.Text);
            ((ShellViewModel)DataContext).Combinations.Add(new Combination() { Puntos = puntos, Jugada = JugadaInput.Text });
            EmptyTextBoxes();
        }

        //Clean the text boxes after the items have been added to the collection 
        private void EmptyTextBoxes()
        {
            PuntosInput.Clear();
            JugadaInput.Clear();
            PuntosInput.Focus();
        }
    }
}
