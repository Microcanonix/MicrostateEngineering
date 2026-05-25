using Microsoft.Extensions.DependencyInjection;

namespace MoleculesViewer.ViewModels
{
    public static class ViewModelExtensions
    {
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<WorkflowExecuterControlViewModel>();
            services.AddSingleton<MoleculeViewerControlViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            return services;
        }

    }
}
