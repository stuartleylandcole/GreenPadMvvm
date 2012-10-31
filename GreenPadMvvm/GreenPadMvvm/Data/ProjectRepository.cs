using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.Linq;

namespace GreenPadMvvm.Data
{
    public class ProjectRepository : IRepository
    {
        private DataContextDataContext _dataContext;
        
        public ProjectRepository()
        {
            _dataContext = new DataContextDataContext();

            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Project>(p => p.Tasks);
            dlo.LoadWith<Task>(t => t.Priority);
            _dataContext.LoadOptions = dlo;
        }
        
        public List<Project> GetAllProjects()
        {
            return _dataContext.GetTable<Project>().ToList();
        }

        public Project GetForKeyTable(int keyTable)
        {
            return _dataContext.Projects.SingleOrDefault(p => p.KeyTable == keyTable);
        }

        /// <summary>
        /// Saves a project to the database, either by updating an existing one or inserting a new one.
        /// </summary>
        /// <param name="project">The project to be saved.</param>
        /// <returns>If the return value is true then a new project has been added.
        /// If it's false then an existing project has been modified.</returns>
        public bool Save(Project project)
        {
            bool newProject = false;

            Project projectToSave = GetForKeyTable(project.KeyTable);
            if (projectToSave == null)
            {
                projectToSave = new Project();
                _dataContext.Projects.InsertOnSubmit(projectToSave);
                newProject = true;
            }

            projectToSave.Description = project.Description;
            projectToSave.Name = project.Name;

            _dataContext.SubmitChanges();

            //little bit friggy but need access to the keyTable that we've just assigned.
            project.KeyTable = projectToSave.KeyTable;

            return newProject;
        }

        public void DeleteProject(Project project)
        {
            Project existingProject = GetForKeyTable(project.KeyTable);
            _dataContext.Projects.DeleteOnSubmit(existingProject);
            _dataContext.SubmitChanges();
        }

        public bool ContainsProject(Project project)
        {
            return GetForKeyTable(project.KeyTable) != null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        #endregion
    }
}
