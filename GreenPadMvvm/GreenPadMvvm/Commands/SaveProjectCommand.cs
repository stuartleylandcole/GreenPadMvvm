using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GreenPadMvvm.Data;
using GreenPadMvvm.ViewModels;

namespace GreenPadMvvm.Commands
{
    public class SaveProjectCommand : CustomCommand
    {
        private Project _project;
        private ProjectDetailsViewModel _projectDetails;
        private MainWindowViewModel _mainWindow;

        public SaveProjectCommand(Project project, ProjectDetailsViewModel projectDetails, MainWindowViewModel mainWindow)
        {
            _project = project;
            _projectDetails = projectDetails;
            _mainWindow = mainWindow;
        }
        
        public override void Execute(object parameter)
        {
            bool? newProject = null;
            using (ProjectRepository projectRepository = new ProjectRepository())
            {
                newProject = projectRepository.Save(_project);
            }

            if (newProject.Value)
            {
                _mainWindow.Mediator.NotifyColleagues(ViewModelMessages.ProjectAdded, _project);
            }
            else
            {
                _mainWindow.Mediator.NotifyColleagues(ViewModelMessages.ProjectAmended, _project);
            }

            //now remove the project details tab and take us to the task list for the project.
            _mainWindow.Tabs.Remove(_projectDetails);
            TaskListViewModel taskList = _mainWindow.GetTaskListTabForProject(_project, true);
            _mainWindow.SetActiveTab(taskList);
        }

        public override bool CanExecute(object parameter)
        {
            return _project.IsValid();
        }
    }
}
