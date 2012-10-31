using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Data;
using System.Collections.ObjectModel;
using GreenPadMvvm.Commands;
using System.Windows.Input;

namespace GreenPadMvvm.ViewModels
{
    public class TaskListViewModel : MediatorEnabledViewModel<ViewModelMessages>
    {
        private Project _project;
        private ObservableCollection<Task> _tasks;
        private Task _selectedTask;

        public TaskListViewModel(Project project)
        {
            _project = project;
            using (TaskRepository repo = new TaskRepository())
            {
                _tasks = new ObservableCollection<Task>(repo.GetForProject(_project));
            }
            
            SubscribeToMessages();
            SetTabTitle();
        }

        #region Overriden methods

        public override void SubscribeToMessages()
        {
            base.Mediator.Register(ViewModelMessages.TaskAdded, t => OnTaskAdded(t as Task));
            base.Mediator.Register(ViewModelMessages.TaskAmended, t => OnTaskModified());
            base.Mediator.Register(ViewModelMessages.TaskDeleted, t => OnTaskDeleted(t as Task));
            base.Mediator.Register(ViewModelMessages.TaskCompleted, t => OnTaskCompleted(t as Task));
        }

        public override void SetTabTitle()
        {
            TabDescription = "Tasks - " + _project.Name;
        }

        #endregion

        public ObservableCollection<Task> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                base.OnPropertyChanged("Tasks");
            }
        }

        public Project Project
        {
            get
            {
                return _project;
            }
        }

        public Task SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (value != _selectedTask)
                {
                    _selectedTask = value;
                }
            }
        }

        #region Events

        private void OnTaskAdded(Task t)
        {
            Tasks.Add(t);
        }

        private void OnTaskModified()
        {
            using (TaskRepository repo = new TaskRepository())
            {
                Tasks = new ObservableCollection<Task>(repo.GetForProject(_project));
            }
        }

        private void OnTaskDeleted(Task task)
        {
            Tasks.Remove(task);
        }

        private void OnTaskCompleted(Task task)
        {
            Tasks.Remove(task);
        }

        #endregion
    }
}
