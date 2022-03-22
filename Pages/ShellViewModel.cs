using MVVM_firstApp.ViewModels;
using Stylet;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive 
    {
        public ShellViewModel(VentasViewModel ventas, ReportesViewModel reportes)
        {
            this.Items.Add(ventas);
            this.Items.Add(reportes);

            this.ActiveItem = ventas;
        }
    }
}
