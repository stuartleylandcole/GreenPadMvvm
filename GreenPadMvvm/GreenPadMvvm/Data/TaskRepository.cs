using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace GreenPadMvvm.Data
{
    public class TaskRepository : IRepository
    {
        private DataContextDataContext _dataContext;

        public TaskRepository()
        {
            _dataContext = new DataContextDataContext();
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Task>(t => t.Project);
            dlo.LoadWith<Task>(t => t.Priority);
            _dataContext.LoadOptions = dlo;
        }

        public IQueryable<Task> GetAllTasks()
        {
            return _dataContext.Tasks.Where(t => t.Done == false);
        }

        public List<Task> GetForProject(Project project)
        {
            IQueryable<Task> tasks = GetAllTasks();
            return tasks.Where(t => t.Project == project && !t.Done).ToList();
        }

        public Task GetForKeyTable(int keyTable)
        {
            return _dataContext.Tasks.SingleOrDefault(t => t.KeyTable == keyTable);
        }

        /// <summary>
        /// Saves a task to the database either by inserting a new task or by updating an existing one.
        /// </summary>
        /// <param name="task">The task to save.</param>
        /// <returns>The return value is true if the task to be saved is a new one.
        /// The return value is false when an existing task has ben updated.</returns>
        public bool Save(Task task)
        {
            bool newTask = false;

            Task taskToSave = GetForKeyTable(task.KeyTable);
            if (taskToSave == null)
            {
                taskToSave = new Task();
                _dataContext.Tasks.InsertOnSubmit(taskToSave);
                newTask = true;
            }
            
            taskToSave.DateDue = task.DateDue;
            taskToSave.Description = task.Description;
            taskToSave.Notes = task.Notes;
            taskToSave.Priority = _dataContext.Priorities.SingleOrDefault(p => p.KeyTable == task.KeyPriority);
            taskToSave.Project = _dataContext.Projects.SingleOrDefault(p => p.KeyTable == task.KeyProject);
            taskToSave.Done = task.Done;
           
            _dataContext.SubmitChanges();

            //little friggy but need the keyTable on the task outside this method.
            task.KeyTable = taskToSave.KeyTable;

            return newTask;
        }

        public void DeleteTask(Task task)
        {
            task.Detach();
            _dataContext.Tasks.Attach(task);
            _dataContext.Tasks.DeleteOnSubmit(task);
            _dataContext.SubmitChanges();
        }

        public bool ContainsTask(Task task)
        {
            return GetForKeyTable(task.KeyTable) != null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        #endregion
    }
}
