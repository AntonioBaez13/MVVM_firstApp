using MVVM_firstApp.ViewModels;
using Stylet;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Screen
    {
        public VentasViewModel Ventas { get; private set; }

        public ShellViewModel()
        {
            Ventas = new VentasViewModel();
        }
    }
}
