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
    public class AmendTaskCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public AmendTaskCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            Task selectedTask = _mainWindow.GetSelectedTask();

            //see if we've already got a task details tab open for this task.
            TaskDetailsViewModel taskDetails = _mainWindow.FindTaskDetailsTabForTask(selectedTask);
            if (taskDetails == null)
            {
                taskDetails = new TaskDetailsViewModel(selectedTask, _mainWindow);
                _mainWindow.Tabs.Add(taskDetails);
            }

            _mainWindow.SetActiveTab(taskDetails);
        }

        public override bool CanExecute(object parameter)
        {
            return _mainWindow.GetSelectedTask() != null;
        }
    }
}
