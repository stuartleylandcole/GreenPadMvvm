﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Data;
using System.Windows.Data;
using GreenPadMvvm.Commands;
using System.Windows.Input;
using System.ComponentModel;

namespace GreenPadMvvm.ViewModels
{
    public class TaskDetailsViewModel : TabViewModel, IDataErrorInfo
    {
        #region Fields

        private Task _task;
        private MainWindowViewModel _mainWindow;
        private CollectionView _projects;
        
        private SaveTaskCommand _saveCommand;
        private CollectionView _priorities;

        #endregion Fields

        #region Constructor

        public TaskDetailsViewModel(Task task, MainWindowViewModel mainWindow)
        {
            using (ProjectRepository projectRepository = new ProjectRepository())
            {
                _projects = new CollectionView(projectRepository.GetAllProjects());
            }

            using (PriorityRepository priorityRepository = new PriorityRepository())
            {
                _priorities = new CollectionView(priorityRepository.GetAllPriorities());
            }

            _task = task;
            _mainWindow = mainWindow;

            SetTabTitle();
        }

        #endregion Constructor

        #region Overriden methods

        public override void SetTabTitle()
        {
            TabDescription = "New Task";
            if (!string.IsNullOrWhiteSpace(_task.Description))
            {
                TabDescription = _task.Description;
            }
        }

        #endregion

        #region Combo box bindings

        public CollectionView Projects
        {
            get
            {
                return _projects;
            }
            private set
            {
                if (_projects != value)
                {
                    _projects = value;
                    base.OnPropertyChanged("Projects");
                }
            }
        }

        public CollectionView Priorities
        {
            get
            {
                return _priorities;
            }
        }

        #endregion

        #region Public fields

        public Project Project
        {
            get
            {
                return _task.Project;
            }
            set
            {
                if (value != _task.Project)
                {
                    _task.Project = value;
                    base.OnPropertyChanged("Project");
                }
            }
        }

        public string Description
        {
            get
            {
                return _task.Description;
            }
            set
            {
                if (value != _task.Description)
                {
                    _task.Description = value;
                    base.OnPropertyChanged("Description");
                }
            }
        }

        public string Notes
        {
            get
            {
                return _task.Notes;
            }
            set
            {
                if (value != _task.Notes)
                {
                    _task.Notes = value;
                    base.OnPropertyChanged("Notes");
                }
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _task.DateDue;
            }
            set
            {
                if (value != _task.DateDue)
                {
                    _task.DateDue = value;
                    base.OnPropertyChanged("DueDate");
                }
            }
        }

        public Priority Priority
        {
            get
            {
                return _task.Priority;
            }
            set
            {
                if (value != _task.Priority)
                {
                    _task.Priority = value;
                    base.OnPropertyChanged("Priority");
                }
            }
        }

        public Task Task
        {
            get
            {
                return _task;
            }
        }

        #endregion Public fields

        #region Save command

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new SaveTaskCommand(_task, _mainWindow);
                }
                return _saveCommand;
            }
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
