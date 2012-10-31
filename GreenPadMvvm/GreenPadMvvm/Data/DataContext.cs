using System;
using System.Data.Linq;
namespace GreenPadMvvm.Data
{
    partial class Project
    {
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Name);
        }

        public void Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;
            this._Tasks = default(EntitySet<Task>);
        }
    }
}
