using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.ViewModels;
using GreenPadMvvm.Data;

namespace GreenPadMvvm.Commands
{
    public class AmendProjectCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public AmendProjectCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            ProjectDetailsViewModel vm = _mainWindow.FindProjectDetailsTabForProject(_mainWindow.SelectedProject);
            if (vm == null)
            {
                vm = new ProjectDetailsViewModel(_mainWindow.SelectedProject, _mainWindow);
                _mainWindow.Tabs.Add(vm);
            }

            _mainWindow.SetActiveTab(vm);
        }

        public override bool CanExecute(object parameter)
        {
            return _mainWindow.SelectedProject != null;
        }
    }
}
