using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Data;
using GreenPadMvvm.ViewModels;

namespace GreenPadMvvm.Commands
{
    public class DeleteProjectCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public DeleteProjectCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            using (ProjectRepository repository = new ProjectRepository())
            {
                repository.DeleteProject(_mainWindow.SelectedProject);
            }
            _mainWindow.Mediator.NotifyColleagues(ViewModelMessages.ProjectDeleted, _mainWindow.SelectedProject);
        }

        public override bool CanExecute(object parameter)
        {
            return _mainWindow.SelectedProject != null;
        }
    }
}
