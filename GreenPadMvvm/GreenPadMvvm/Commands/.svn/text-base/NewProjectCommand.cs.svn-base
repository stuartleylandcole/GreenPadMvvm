using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GreenPadMvvm.ViewModels;
using GreenPadMvvm.Data;

namespace GreenPadMvvm.Commands
{
    public class NewProjectCommand : CustomCommand
    {
        private MainWindowViewModel _viewModel;

        public NewProjectCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            ProjectDetailsViewModel vm = new ProjectDetailsViewModel(new Project(), _viewModel);
            _viewModel.Tabs.Add(vm);
            _viewModel.SetActiveTab(vm);
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
