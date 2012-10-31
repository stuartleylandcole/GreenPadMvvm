using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Data;
using GreenPadMvvm.Commands;
using System.Windows.Input;

namespace GreenPadMvvm.ViewModels
{
    public class ProjectDetailsViewModel : TabViewModel//MediatorEnabledViewModel<ViewModelMessages>
    {
        #region Fields

        private readonly Project _project;
        private MainWindowViewModel _mainWindow;
        private SaveProjectCommand _saveCommand;

        #endregion

        #region Constructor

        public ProjectDetailsViewModel(Project project, MainWindowViewModel mainWindow)
        {
            _project = project;
            _mainWindow = mainWindow;

            SetTabTitle();
        }

        public override void SetTabTitle()
        {
            TabDescription = "New Project";
            if (!string.IsNullOrWhiteSpace(_project.Name))
            {
                TabDescription = _project.Name;
            }
            //base.OnPropertyChanged("TabDescription");
        }

        #endregion

        public Project Project
        {
            get
            {
                return _project;
            }
        }

        #region Project properties

        public string Name
        {
            get
            {
                return _project.Name;
            }
            set
            {
                if (value != _project.Name)
                {
                    _project.Name = value;
                    base.OnPropertyChanged("Name");
                    SetTabTitle();
                }
            }
        }

        public string Description
        {
            get
            {
                return _project.Description;
            }
            set
            {
                if (value != _project.Description)
                {
                    _project.Description = value;
                    base.OnPropertyChanged("Description");
                }
            }
        }

        #endregion Project properties

        #region Save Command

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new SaveProjectCommand(_project, this, _mainWindow);
                }
                return _saveCommand;
            }
        }

        #endregion Save Command
    }
}
