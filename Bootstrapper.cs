using MVVM_firstApp.Models;
using MVVM_firstApp.Pages;
using Stylet;
using StyletIoC;

namespace MVVM_firstApp
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<LotoContext>().ToSelf().InSingletonScope();
            builder.Bind<IDatabaseOperations>().To<DatabaseOperations>().InSingletonScope();
            builder.Bind<IPrintBehaviour>().To<PrintBehaviour>().InSingletonScope();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
