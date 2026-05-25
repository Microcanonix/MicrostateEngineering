using MoleculesViewer.Common;
using MoleculesViewer.ViewModels;
using System.Windows.Input;

namespace MoleculesViewer
{
    public sealed class MainWindowViewModel : ObservableObject
    {
        private object? _currentViewModel;

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowWorkflowRunnerCommand { get; }

        public ICommand ShowMoleculeViewerCommand { get; }

        public MainWindowViewModel(MoleculeViewerControlViewModel moleculeViewerViewModel,
                                        WorkflowExecuterControlViewModel workflowExecuterControlViewModel)
        {
            ShowWorkflowRunnerCommand = new DelegateCommand(_ =>
            {
                CurrentViewModel = workflowExecuterControlViewModel;
            });

            ShowMoleculeViewerCommand = new DelegateCommand(_ =>
            {
                CurrentViewModel = moleculeViewerViewModel;
            });
        }

    }
}
