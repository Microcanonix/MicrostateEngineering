using Microsoft.Extensions.DependencyInjection;
using MoleculeFactory;
using MoleculeProcessFactory;
using MoleculeProcessService;
using MoleculeRepository;
using MoleculeServices;
using MoleculesViewer.ViewModels;
using ResearchDefinitionRepository;
using ResearchDefinitionService;
using System.Windows;
using UtilityServices;

namespace MoleculesViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider? Services { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            Register(serviceCollection, ServiceLifetime.Singleton);
            Services = serviceCollection.BuildServiceProvider();

            // Resolve MainWindow from DI so its constructor receives MainWindowViewModel
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        public IServiceCollection Register(IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            services.RegisterUtilities(serviceLifetime)
                            .RegisterResearchDefinitionRepo(serviceLifetime)
                            .RegisterResearchDefinitionSvc(serviceLifetime)
                            .RegisterMoleculeRepository(serviceLifetime)
                            .RegisterMoleculeFactory(serviceLifetime)
                            .RegisterMoleculeService(serviceLifetime)
                            .RegisterMoleculeProcessService(serviceLifetime)
                            .RegisterMoleculeProcessFactory(serviceLifetime)
                            .RegisterViewModels();

            services.AddSingleton<MainWindow>();

            return services;
        }

    }

}
