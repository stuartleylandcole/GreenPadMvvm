using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using GreenPadMvvm.Commands;
using GreenPadMvvm.Data;
using System.ComponentModel;
using System.Windows.Data;

namespace GreenPadMvvm.ViewModels
{
    public class MainWindowViewModel : MediatorEnabledViewModel<ViewModelMessages>
    {
        #region Fields

        private ObservableCollection<TabViewModel> _tabs;

        private Project _selectedProject;
        private ObservableCollection<Project> _projects;
        
        private NewProjectCommand _newProjectCommand;
        private AmendProjectCommand _amendProjectCommand;
        private DeleteProjectCommand _deleteProjectCommand;
        private NewTaskCommand _newTaskCommand;
        private AmendTaskCommand _amendTaskCommand;
        private DeleteTaskCommand _deleteTaskCommand;
        private CompleteTaskCommand _completeTaskCommand;        

        #endregion Fields

        #region Constructor

        public MainWindowViewModel()
        {
            using (ProjectRepository projectRepository = new ProjectRepository())
            {
                _projects = new ObservableCollection<Project>(projectRepository.GetAllProjects());
            }

            SubscribeToMessages();
            SetTabTitle();
        }

        #endregion Constructor

        public override void  SubscribeToMessages()
        {
            base.Mediator.Register(ViewModelMessages.ProjectAdded, p => OnProjectAdded(p as Project));
            base.Mediator.Register(ViewModelMessages.ProjectAmended, p => OnProjectAmended(p as Project));
            base.Mediator.Register(ViewModelMessages.ProjectDeleted, p => OnProjectDeleted(p as Project));
            base.Mediator.Register(ViewModelMessages.TaskDeleted, t => OnTaskDeleted(t as Task));
        }

        public override void SetTabTitle()
        {
            //This view model doesn't actually have a tab, it just contains everything else.
        }

        #region Tabs

        public ObservableCollection<TabViewModel> Tabs
        {
            get
            {
                if (_tabs == null)
                {
                    _tabs = new ObservableCollection<TabViewModel>();
                    _tabs.CollectionChanged += this.OnTabsChanged;
                }
                return _tabs;
            }
        }

        private void OnTabsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //we've got new tabs, display them.
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (TabViewModel tab in e.NewItems)
                {
                    tab.RequestClose += this.OnTabRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (TabViewModel tab in e.OldItems)
                {
                    tab.RequestClose -= this.OnTabRequestClose;
                }
            }
        }

        private void OnTabRequestClose(object sender, EventArgs e)
        {
            TabViewModel tab = sender as TabViewModel;
            Tabs.Remove(tab);
        }

        public void SetActiveTab(TabViewModel tab)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Tabs);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(tab);
            }
        }

        #endregion Tabs

        #region Project list

        public ObservableCollection<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
                OnPropertyChanged("Projects");
            }
        }

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                if (value != _selectedProject)
                {
                    _selectedProject = value;

                    //if you delete the last project then _selectedProject will be null so don't try to create a view model for it.
                    if (_selectedProject != null)
                    {
                        TaskListViewModel tabToSelect = GetTaskListTabForProject(_selectedProject, false);

                        if (tabToSelect == null)
                        {
                            tabToSelect = new TaskListViewModel(_selectedProject);
                            Tabs.Add(tabToSelect);
                        }
                        SetActiveTab(tabToSelect);
                    }
                }
            }
        }

        #endregion Project list

        #region Events

        private void OnProjectAdded(Project project)
        {
            Projects.Add(project);
        }

        private void OnProjectAmended(Project project)
        {
            using (ProjectRepository projectRepository = new ProjectRepository())
            {
                Projects = new ObservableCollection<Project>(projectRepository.GetAllProjects());
            }
        }

        private void OnProjectDeleted(Project project)
        {
            TaskListViewModel taskViewModel = GetTaskListTabForProject(project, false);
            if (taskViewModel != null)
            {
                Tabs.Remove(taskViewModel);
            }
            ProjectDetailsViewModel projectViewModel = FindProjectDetailsTabForProject(project);
            if (projectViewModel != null)
            {
                Tabs.Remove(projectViewModel);
            }
            Projects.Remove(project);
        }

        private void OnTaskDeleted(Task task)
        {
            TaskDetailsViewModel vm = FindTaskDetailsTabForTask(task);
            if (vm != null)
            {
                Tabs.Remove(vm);
            }
        }

        #endregion

        #region Commnds

        public ICommand NewProjectCommand
        {
            get
            {
                if (_newProjectCommand == null)
                {
                    _newProjectCommand = new NewProjectCommand(this);
                }
                return _newProjectCommand;
            }
        }

        public ICommand AmendProjectCommand
        {
            get
            {
                if (_amendProjectCommand == null)
                {
                    _amendProjectCommand = new AmendProjectCommand(this);
                }
                return _amendProjectCommand;
            }
        }

        public ICommand DeleteProjectCommand
        {
            get
            {
                if (_deleteProjectCommand == null)
                {
                    _deleteProjectCommand = new DeleteProjectCommand(this);
                }
                return _deleteProjectCommand;
            }
        }

        public ICommand NewTaskCommand
        {
            get
            {
                if (_newTaskCommand == null)
                {
                    _newTaskCommand = new NewTaskCommand(this);
                }
                return _newTaskCommand;
            }
        }

        public ICommand AmendTaskCommand
        {
            get
            {
                if (_amendTaskCommand == null)
                {
                    _amendTaskCommand = new AmendTaskCommand(this);
                }
                return _amendTaskCommand;
            }
        }

        public ICommand DeleteTaskCommand
        {
            get
            {
                if (_deleteTaskCommand == null)
                {
                    _deleteTaskCommand = new DeleteTaskCommand(this);
                }
                return _deleteTaskCommand;
            }
        }

        public ICommand CompleteTaskCommand
        {
            get
            {
                if (_completeTaskCommand == null)
                {
                    _completeTaskCommand = new CompleteTaskCommand(this);
                }
                return _completeTaskCommand;
            }
        }

        #endregion

        #region Helper methods

        public TaskListViewModel GetTaskListTabForProject(Project project, bool createIfNotExists)
        {
            TaskListViewModel tabToSelect = null;

            foreach (TabViewModel vm in Tabs)
            {
                if (vm is TaskListViewModel)
                {
                    TaskListViewModel tlvm = (TaskListViewModel)vm;
                    if (tlvm.Project.KeyTable == project.KeyTable)
                    {
                        tabToSelect = tlvm;
                        break;
                    }
                }
            }

            if (tabToSelect == null && createIfNotExists)
            {
                tabToSelect = new TaskListViewModel(project);
                Tabs.Add(tabToSelect);
            }

            return tabToSelect;
        }

        public ProjectDetailsViewModel FindProjectDetailsTabForProject(Project project)
        {
            ProjectDetailsViewModel tabToSelect = null;

            foreach (TabViewModel vm in Tabs)
            {
                if (vm is ProjectDetailsViewModel)
                {
                    ProjectDetailsViewModel pdvm = (ProjectDetailsViewModel)vm;
                    if (pdvm.Project == project)
                    {
                        tabToSelect = pdvm;
                        break;
                    }
                }
            }

            return tabToSelect;
        }

        public TaskDetailsViewModel FindTaskDetailsTabForTask(Task task)
        {
            TaskDetailsViewModel tabToSelect = null;

            foreach (TabViewModel vm in Tabs)
            {
                if (vm is TaskDetailsViewModel)
                {
                    TaskDetailsViewModel tdvm = (TaskDetailsViewModel)vm;
                    if (tdvm.Task == task)
                    {
                        tabToSelect = tdvm;
                        break;
                    }
                }
            }

            return tabToSelect;
        }

        public Task GetSelectedTask()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Tabs);
            if (!(collectionView.CurrentItem is TaskListViewModel))
            {
                return null;
            }

            TaskListViewModel selectedTaskList = collectionView.CurrentItem as TaskListViewModel;
            if (selectedTaskList.SelectedTask == null)
            {
                return null;
            }

            return selectedTaskList.SelectedTask;
        }

        #endregion
    }
}
