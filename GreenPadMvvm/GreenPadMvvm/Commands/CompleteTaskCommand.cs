using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.ViewModels;
using GreenPadMvvm.Data;
using System.ComponentModel;
using System.Windows.Data;

namespace GreenPadMvvm.Commands
{
    public class CompleteTaskCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public CompleteTaskCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            Task selectedTask = _mainWindow.GetSelectedTask();
            selectedTask.Done = true;

            bool? saved = null;
            using (TaskRepository repository = new TaskRepository())
            {
                saved = repository.Save(selectedTask);
            }

            if (saved == null)
            {
                return;
            }

            TaskListViewModel vm = _mainWindow.GetTaskListTabForProject(selectedTask.Project, true);
            vm.Mediator.NotifyColleagues(ViewModelMessages.TaskCompleted, selectedTask);
        }

        public override bool CanExecute(object parameter)
        {
            return _mainWindow.GetSelectedTask() != null;
        }
    }
}
