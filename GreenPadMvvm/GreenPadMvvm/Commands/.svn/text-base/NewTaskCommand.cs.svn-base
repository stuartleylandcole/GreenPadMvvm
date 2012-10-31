using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.ViewModels;
using GreenPadMvvm.Data;

namespace GreenPadMvvm.Commands
{
    public class NewTaskCommand : CustomCommand
    {
        private MainWindowViewModel _mainWindow;

        public NewTaskCommand(MainWindowViewModel mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute(object parameter)
        {
            TaskDetailsViewModel vm = new TaskDetailsViewModel(new Task(), _mainWindow);
            _mainWindow.Tabs.Add(vm);
            _mainWindow.SetActiveTab(vm);
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
