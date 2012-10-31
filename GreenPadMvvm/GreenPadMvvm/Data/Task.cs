using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlTypes;

namespace GreenPadMvvm.Data
{
    partial class Task
    {
        public static Task CreateNewTask()
        {
            Task task = new Task();
            task.Project = null;
            task.Description = string.Empty;
            task.Notes = string.Empty;
            task.DateDue = DateTime.Now;
            task.Priority = null;
            task.Done = false;

            return task;
        }

        #region Validation

        public bool IsValid()
        {
            if (!string.IsNullOrWhiteSpace(GetValidationErrorProject()))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(GetValidationErrorPriority()))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(GetValidationErrorDescription()))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(GetValidationErrorNotes()))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(GetValidationErrorDueDate()))
            {
                return false;
            }

            return true;
        }

        public string GetValidationErrorProject()
        {
            if (Project == null)
            {
                return "You must select a project.";
            }

            return string.Empty;
        }

        public string GetValidationErrorPriority()
        {
            if (Priority == null)
            {
                return "You must select a priority.";
            }

            return string.Empty;
        }

        public string GetValidationErrorDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                return "You must enter a description.";
            }

            if (Description.Length > 100)
            {
                return "The description must be 100 characters or less.";
            }

            return string.Empty;
        }

        public string GetValidationErrorNotes()
        {
            if (Notes.Length > 250)
            {
                return "The notes must be 250 characters or less.";
            }

            return string.Empty;
        }

        public string GetValidationErrorDueDate()
        {
            if (!((DateDue > SqlDateTime.MinValue) && (DateDue < SqlDateTime.MaxValue)))
            {
                return "The due date must be between " + SqlDateTime.MinValue + " and " + SqlDateTime.MaxValue;
            }

            return string.Empty;
        }

        #endregion Validation

        #region Presentation

        public string DueDateFormatted
        {
            get
            {
                return string.Format("{0:dd MMM yyyy}", DateDue);
            }
        }

        public string PriorityDescription
        {
            get
            {
                string retval = string.Empty;
                if (Priority != null)
                {
                    retval = Priority.Description;
                }
                return retval;
            }
        }

        #endregion Presentation

        public void Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;
            this._Project = default(EntityRef<Project>);
            this._Priority = default(EntityRef<Priority>);
        }
    }
}
