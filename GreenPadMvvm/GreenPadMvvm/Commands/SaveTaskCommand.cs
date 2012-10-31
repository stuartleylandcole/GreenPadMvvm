using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GreenPadMvvm.Data;
using GreenPadMvvm.ViewModels;

namespace GreenPadMvvm.Commands
{
    public class SaveTaskCommand : CustomCommand
    {
        private Task _task;
        private MainWindowViewModel _mainWindow;

        public SaveTaskCommand(Task task, MainWindowViewModel mainWindow)
        {
            _task = task;
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            bool? newTask = null;
            using (TaskRepository taskRepository = new TaskRepository())
            {
                newTask = taskRepository.Save(_task);
            }

            if (newTask == null)
            {
                return;
            }

            TaskListViewModel taskList = _mainWindow.GetTaskListTabForProject(_task.Project, true);
            if (newTask.Value)
            {
                taskList.Mediator.NotifyColleagues(ViewModelMessages.TaskAdded, _task);
            }
            else
            {
                taskList.Mediator.NotifyColleagues(ViewModelMessages.TaskAmended, _task);
            }
            
            //now close the tab and take us to the task list for this project.
            TaskDetailsViewModel taskDetails = _mainWindow.FindTaskDetailsTabForTask(_task);
            _mainWindow.Tabs.Remove(taskDetails);
            _mainWindow.SetActiveTab(taskList);
        }

        public override bool CanExecute(object parameter)
        {
            return _task.IsValid();
        }
    }
}
