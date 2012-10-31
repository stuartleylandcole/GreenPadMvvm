using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using GreenPadMvvm.ViewModels;
using System.ComponentModel;
using System.Windows.Data;
using GreenPadMvvm.Data;

namespace GreenPadMvvm.Commands
{
    public class DeleteTaskCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public DeleteTaskCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;            
        }

        public override void Execute(object parameter)
        {
            Task selectedTask = _mainWindow.GetSelectedTask();
            using (TaskRepository taskRepository = new TaskRepository())
            {
                taskRepository.DeleteTask(selectedTask);
            }
            _mainWindow.Mediator.NotifyColleagues(ViewModelMessages.TaskDeleted, selectedTask);
        }

        public override bool CanExecute(object parameter)
        {
            return _mainWindow.GetSelectedTask() != null;
        }
    }
}
